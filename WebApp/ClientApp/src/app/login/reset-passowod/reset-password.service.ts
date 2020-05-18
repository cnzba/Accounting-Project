import { throwError as observableThrowError, Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';

@Injectable({
    providedIn: 'root'
})
export class ResetPasswordService {
    [x: string]: any;
    private url = 'api/ResetPassword';

    constructor(private http: HttpClient) { }


    verifyToken(id: number, token: string): Observable<string> {
        return this.http.post<string>(this.url, {
            "id": id,
            "token": token
        });
    }

    changePassword(newPassword: string, confirmPassword: string, id: number, token: string): Observable<string> {
        return this.http.post<string>(this.url + "/PostChangePassword", {
            "confirmPassword": confirmPassword,
            "newPassword": newPassword,
            "id": id,
            "token": token
        }).pipe(catchError(this.handleError));
    }

    private handleError(err: HttpErrorResponse) {

        let errorMessage = '';
        if (err.error instanceof Error) {
            errorMessage = `An error occurred. Please try again.`;
        } else {
            errorMessage = err.status == 504 ? `An error occurred. Please try again.` : `${err.error}`;
        }
        return observableThrowError(err);
    }

    private log(message: string) {
        this.messageService.add(`HeroService: ${message}`);
    }
}
