import { throwError as observableThrowError, Observable, of } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { isNullOrUndefined } from 'util';

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
        //{ observe: 'response' }).pipe(
        //    map(
        //        (data: any) => {
        //            return data;
        //        }), catchError(error => {
        //            return error;
        //        })
        //)
        //}).pipe(catchError(this.handleError));
    }

    private handleError(err: HttpErrorResponse) {

        let errorMessage = '';
        if (err.error instanceof Error) {
            errorMessage = `An error occurred. Please try again.`;
            //errorMessage = `${err.error.message}`;
        } else {
            errorMessage = err.status == 504 ? `An error occurred. Please try again.` : `${err.error}`;
        }
       // console.error(!isNullOrUndefined(errorMessage) ? errorMessage : err.message);
        return observableThrowError(err);
    }

    //private handleError<T>(operation = 'operation', result?: T) {
    //    return (error: any): Observable<T> => {

    //        // TODO: send the error to remote logging infrastructure
    //        console.error(error); // log to console instead

    //        // TODO: better job of transforming error for user consumption
    //        this.log(`${operation} failed: ${error.message}`);

    //        // Let the app keep running by returning an empty result.
    //        return of(result as T);
    //    };
    //}

    /** Log a HeroService message with the MessageService */
    private log(message: string) {
        this.messageService.add(`HeroService: ${message}`);
    }
}
