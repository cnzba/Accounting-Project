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
    private _invoiceUrl = 'api/invoice';

    constructor(private _http: HttpClient) { }

    getInvoices(): Observable<IInvoice[]> {
        return this._http.get<IInvoice[]>(this._invoiceUrl)
            .do(data => console.log('All: ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

    getInvoice(id: number): Observable<IInvoice> {
        // needs to change get specific invoice from Web API
        // need to get by invoice number, not invoice ID
        return this.getInvoices()
            .map((invoices: IInvoice[]) => invoices.find(i => i.id === id));
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
