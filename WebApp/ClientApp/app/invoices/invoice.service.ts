import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

import 'rxjs/add/observable/throw';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';

import { IInvoice } from './invoice';

@Injectable()
export class InvoiceService {
    // private _invoiceUrl = 'assets/mockapi/invoices/invoices.json';
    private invoiceUrl = 'api/invoice';

    constructor(private http: HttpClient) { }

    getInvoices(): Observable<IInvoice[]> {
        return this.http.get<IInvoice[]>(this.invoiceUrl)
            .do(data => console.log('GetAll: ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    getInvoice(invoiceNumber: string): Observable<IInvoice> {
        return this.http.get<IInvoice>(this.invoiceUrl + '/' + invoiceNumber)
            .do(data => console.log('Get1: ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    createNewInvoice(): Observable<IInvoice> {
        var today = new Date();
        var fakeInvoiceNumber = today.getFullYear()
            + ("0" + (today.getMonth() + 1)).slice(-2)
            + ("0" + today.getDate()).slice(-2)
            + "-xxx";

        return Observable.of({invoiceNumber: fakeInvoiceNumber, issueeOrganization: "", issueeCareOf: "", clientContact: "", dateDue: null, status: 'New', dateCreated: today, gstNumber: "xx-xxx-xxx", charitiesNumber: "xxxxxxx", "gstRate": 0.15, "invoiceLine": null, subTotal: 0, grandTotal: 0 });
    }

    saveDraftInvoice(invoice: IInvoice): Observable<IInvoice> {
        var response: Observable<IInvoice>;

        if (this.isSaved(invoice)) response = this.updateInvoice(invoice);
        else response = this.createInvoice(invoice);

        return response;
    }

    private createInvoice(invoice: IInvoice): Observable<IInvoice> {
        return this.http.post<IInvoice>(this.invoiceUrl, invoice)
            .do(data => console.log('Post: ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private updateInvoice(invoice: IInvoice): Observable<IInvoice> {
        return this.http.put<IInvoice>(this.invoiceUrl + '/' + invoice.invoiceNumber, invoice)
            .do(data => console.log('Put: ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    private isSaved(invoice: IInvoice) {
        if (invoice.invoiceNumber.search("xxx") == -1) return true;
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
            // The response body may contain clues as to what went wrong,
            errorMessage = `Server returned code: ${err.status}, error message is: ${err.message}`;
        }
        console.error(errorMessage);
        return Observable.throw(errorMessage);
    }

}
