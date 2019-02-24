import { throwError as observableThrowError, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Injectable()
export class ForgotPasswordService {
    private url : string = "api/ForgotPassword";

    constructor(private http: HttpClient) { }

    sendEmail(email: string): Observable<string> {
        return this.http.post<string>(this.url, { "email" : email }).pipe(catchError(this.handleError));
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
        return observableThrowError(errorMessage);
    }
}
