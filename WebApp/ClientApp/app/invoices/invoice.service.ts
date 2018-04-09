import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/observable/of';

import { IInvoice, IInvoiceLine } from './invoice';

@Injectable()
export class InvoiceService {
    [x: string]: any;
    // private _invoiceUrl = 'assets/mockapi/invoices/invoices.json';
    private invoiceUrl = 'api/invoice';

    constructor(private http: HttpClient) { }

    getInvoices(): Observable<IInvoice[]> {
        return this.http.get<IInvoice[]>(this.invoiceUrl).catch(this.handleError);
    }

    getInvoice(invoiceNumber: string): Observable<IInvoice> {
        return this.http.get<IInvoice>(this.invoiceUrl + '/' + invoiceNumber)
              .catch(this.handleError);
    }

    getInvoiceByPaymentId(paymentId: string): Observable<IInvoice> {
        return this.http.get<IInvoice>(this.invoiceUrl + '/p/' + paymentId)
            .catch(this.handleError);
    }

    createNewInvoice(): Observable<IInvoice> {
        var today = new Date();
        var dueDate = new Date();
        dueDate.setDate(dueDate.getDate() + 14);

        var fakeInvoiceNumber = today.getFullYear()
            + ("0" + (today.getMonth() + 1)).slice(-2)
            + ("0" + today.getDate()).slice(-2)
            + "-xxx";

        return Observable.of({ invoiceNumber: fakeInvoiceNumber, clientName: "", clientContactPerson: "", clientContact: "", dateDue: dueDate, status: 'New', dateCreated: today, gstNumber: "xx-xxx-xxx", charitiesNumber: "xxxxxxx", "gstRate": 0.15, email: "", paymentId: "", "invoiceLine": [], subTotal: 0, grandTotal: 0 });
    }

    saveDraftInvoice(invoice: IInvoice): Observable<IInvoice> {
        var response: Observable<IInvoice>;

        // ensure line items remain in the correct order
        for (var i = 0; i < invoice.invoiceLine.length; i++) {
            invoice.invoiceLine[i].itemOrder = i;
        }

        if (this.isSaved(invoice)) response = this.updateInvoice(invoice);
        else response = this.createInvoice(invoice);

        return response;
    }

    computeGST(invoice: IInvoice): number
    {
        let total: number = this.computeTotal(invoice);
        return total - total / (1 + invoice.gstRate);
    }

    computeTotal(invoice: IInvoice) : number
    {
        return invoice.invoiceLine.reduce<number>(
            (acc: number, elem: IInvoiceLine): number => acc + +elem.amount, 0);
    }

    private createInvoice(invoice: IInvoice): Observable<IInvoice> {
        console.log('Post (send): ' + JSON.stringify(invoice));
        return this.http.post<IInvoice>(this.invoiceUrl, invoice)
            .do(data => console.log('Post (receive): ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private updateInvoice(invoice: IInvoice): Observable<IInvoice> {
        console.log('Put (send): ' + JSON.stringify(invoice));
        return this.http.put<IInvoice>(this.invoiceUrl + '/' + invoice.invoiceNumber, invoice)
            .do(data => console.log('Put (receive): ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private isSaved(invoice: IInvoice) {
        if (invoice.status.search("New") == -1) return true;
        else return false;
    }

    private handleError(err: HttpErrorResponse) {
        // in a real world app, we may send the server to some remote logging infrastructure
        // instead of just logging it to the console

        let errorMessage = '';
        if (err.error instanceof Error) {
            // A client-side or network error occurred. Handle it accordingly.
            errorMessage = `An error occurred: ${err.error.message}`;
        } else {
            // The backend returned an unsuccessful response code.
            // The response body may contain clues as to what went wrong
            if (err.status == 400) {
                let tmpMessage: string = '';

                tmpMessage = Object.keys(err.error)[0] + ': ' + err.error[Object.keys(err.error)[0]];
                if (tmpMessage[0] == '0') tmpMessage = err.error;

                errorMessage = `${tmpMessage}`;
            }
            else errorMessage = `Server returned code: ${err.status}, error message is: ${err.error}`;
        }

        console.error(errorMessage);
        return Observable.throw(errorMessage);
    }

}
