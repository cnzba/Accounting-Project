import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { tap, map } from 'rxjs/operators';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IInvoice, IInvoiceLine, InvoiceForCreation, InvoiceForUpdate } from './invoice';
import { type } from 'os';

@Injectable()
export class InvoiceService {
    [x: string]: any;
    // private _invoiceUrl = 'assets/mockapi/invoices/invoices.json';
    private invoiceUrl = 'api/invoice';

    constructor(private http: HttpClient) { }

    getInvoices(): Observable<IInvoice[]> {
        return this.http.get<IInvoice[]>(this.invoiceUrl);
    }

    getInvoicesBySatus(status:string): Observable<IInvoice[]> {
        return this.http.get<IInvoice[]>(this.invoiceUrl+'/status/'+status);
    }

    getInvoice(invoiceNumber: string): Observable<IInvoice> {
        return this.http.get<IInvoice>(this.invoiceUrl + '/' + invoiceNumber);
    }

    getInvoiceByPaymentId(paymentId: string): Observable<IInvoice> {
        return this.http.get<IInvoice>(this.invoiceUrl + '/p/' + paymentId);
    }

    createNewInvoice(loginId: string): Observable<IInvoice> {
        var today = new Date();
        var dueDate = new Date();
        dueDate.setDate(dueDate.getDate() + 14);

        var fakeInvoiceNumber = today.getFullYear()
            + ("0" + (today.getMonth() + 1)).slice(-2)
            + ("0" + today.getDate()).slice(-2)
            + "-xxx";

        return this.getNewInvoiceNumber(loginId).pipe(
            map((invoiceNo) => {
                if (invoiceNo) {
                    return { invoiceNumber: invoiceNo, clientName: "", clientContactPerson: "", purchaseOrderNumber: "", clientContact: "", dateDue: dueDate, status: 'New', dateCreated: today, gstNumber: "xx-xxx-xxx", charitiesNumber: "xxxxxxx", "gstRate": 0.15, email: "", paymentId: "", "invoiceLine": [], subTotal: 0, grandTotal: 0, loginId: "" };
                } else {
                    return { invoiceNumber: fakeInvoiceNumber, clientName: "", clientContactPerson: "", purchaseOrderNumber: "", clientContact: "", dateDue: dueDate, status: 'New', dateCreated: today, gstNumber: "xx-xxx-xxx", charitiesNumber: "xxxxxxx", "gstRate": 0.15, email: "", paymentId: "", "invoiceLine": [], subTotal: 0, grandTotal: 0, loginId: "" };
                }
            }),
        );
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

    finaliseInvoice(invoiceNumber: string): Observable<IInvoice> {
        console.log(`Finalising invoice: ${invoiceNumber}`);
        let status = { status: "Issued" };

        console.log('Put (send): ' + JSON.stringify(status));
        return this.http.put<IInvoice>(this.invoiceUrl + '/' + invoiceNumber + '/status', status)
            .pipe(tap(data => console.log('Put (receive): ' + JSON.stringify(data))));
    }

    computeGST(invoice: IInvoice): number {
        let total: number = this.computeTotal(invoice);
        //return total - total / (1 + invoice.gstRate);
        return total * invoice.gstRate;
    }

    computeTotal(invoice: IInvoice): number {
        return invoice.invoiceLine.reduce<number>(
            (acc: number, elem: IInvoiceLine): number => acc + +elem.amount, 0);
    }

    private createInvoice(invoice: IInvoice): Observable<IInvoice> {
        let create = new InvoiceForCreation();

        create.clientContact = invoice.clientContact;
        create.clientContactPerson = invoice.clientContactPerson;
        create.clientName = invoice.clientName;
        create.dateDue = invoice.dateDue;
        create.email = invoice.email;
        create.invoiceLine = invoice.invoiceLine;
        create.loginId = invoice.loginId;
        create.purchaseOrderNumber = invoice.purchaseOrderNumber;

        console.log('Post (send): ' + JSON.stringify(create));
        return this.http.post<IInvoice>(this.invoiceUrl, create)
            .pipe(tap(data => console.log('Post (receive): ' + JSON.stringify(data))));
    }

    private updateInvoice(invoice: IInvoice): Observable<IInvoice> {
        let update = new InvoiceForUpdate();

        update.clientContact = invoice.clientContact;
        update.clientContactPerson = invoice.clientContactPerson;
        update.clientName = invoice.clientName;
        update.dateDue = invoice.dateDue;
        update.email = invoice.email;
        update.invoiceLine = invoice.invoiceLine;
        update.purchaseOrderNumber = invoice.purchaseOrderNumber;

        console.log('Put (send): ' + JSON.stringify(invoice));
        return this.http.put<IInvoice>(this.invoiceUrl + '/' + invoice.invoiceNumber, invoice)
            .pipe(tap(data => console.log('Put (receive): ' + JSON.stringify(data))));
    }

    private isSaved(invoice: IInvoice) {
        if (invoice.status.search("New") == -1) return true;
        else return false;
    }

    deleteInvoice(invoiceNumber: string): Observable<string> {
        console.log('Delete (send): ' + invoiceNumber);
        return this.http.delete<string>(this.invoiceUrl + '/' + invoiceNumber)
            .pipe(tap(data => console.log('Delete (receive): ' + JSON.stringify(data))));
    }

    getNewInvoiceNumber(loginId: string): Observable<string> {
        return this.http.get<string>(this.invoiceUrl + '/invoicenumber?login=' + loginId);
    }

    getPdfInvoice(invoiceNumber: string): Observable<any> {
        return this.http.get(this.invoiceUrl + "/getPdfInvoice/" + invoiceNumber,
            { responseType: "blob" })
            .pipe(tap(data => {
                var blob = new Blob([data], { type: 'application/pdf' })
                return blob;
            }));
    }
}
