import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class InvoicePaymentService {
    private url: string = "api/Payment";

    constructor(private http: HttpClient) { }

    chargeCard(body: any): Observable<string> {
        return this.http.post(this.url, body).map(res => JSON.stringify(res)).catch(this.handleError);
    }

    private handleError(err: HttpErrorResponse) {
        let errorMessage = '';
        if (err.error instanceof Error) {
            errorMessage = `An error occurred. Please try again.`;
            //errorMessage = `${err.error.message}`;
        } else {
            errorMessage = err.status == 504 ? `An error occurred. Please try again.` : `${err.error}`;
        }
        console.error(errorMessage);
        return Observable.throw(errorMessage);
    }
}
