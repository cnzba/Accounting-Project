import { throwError as observableThrowError, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams } from '@angular/common/http';

@Injectable()
export class ChangePasswordService {
    private url: string = "api/ChangePassword";

    constructor(private http: HttpClient) { }

    changePassword(oldPassword: string, newPassword: string): Observable<string> {
        return this.http.post<string>(this.url, {
            "oldPassword": oldPassword,
            "newPassword": newPassword,
            "email": localStorage.getItem("LoginId")
        }).pipe(catchError(this.handleError));
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
