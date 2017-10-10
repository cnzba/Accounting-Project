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
    // private _productUrl = './api/products/products.json';
    // private _productUrl = 'http://localhost:62650/api/products';
    private _invoiceUrl = 'assets/mockapi/invoices/invoices.json';

    constructor(private _http: HttpClient) { }

    getInvoices(): Observable<IInvoice[]> {
        return this._http.get<IInvoice[]>(this._invoiceUrl)
            .do(data => console.log('All: ' + JSON.stringify(data)))
            .catch(this.handleError);
    }

  /*  getProduct(id: number): Observable<IProduct> {
        return this.getProducts()
            .map((products: IProduct[]) => products.find(p => p.productId === id));
    }*/

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
