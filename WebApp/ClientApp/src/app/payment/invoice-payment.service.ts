import { throwError as observableThrowError, Observable } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';

@Injectable()
export class InvoicePaymentService {
    private url: string = "api/Payment";

    constructor(private http: HttpClient) { }

    chargeCard(body: any): Observable<string> {
        return this.http.post(this.url, body).pipe(map(res => JSON.stringify(res)), catchError(this.handleError));
    }

    private handleError(err: HttpErrorResponse) {
        let errorMessage = ' ';
        if (err.error instanceof Error) {
            errorMessage = `An error occurred. Please try again.`;
            //errorMessage = `${err.error.message}`;
        } else {
            errorMessage = err.status == 504 ? `An error occurred. Please try again.` : `${err.error}`;
        }
        console.error(errorMessage);
        return observableThrowError(errorMessage);
    }
}
