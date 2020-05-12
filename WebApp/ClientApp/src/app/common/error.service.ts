
import { throwError as observableThrowError, Observable, empty } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { Injectable, Injector } from '@angular/core';
import { Router } from "@angular/router";
import { AlertService } from "./alert/alert.service";
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor,
    HttpResponse,
    HttpErrorResponse
} from "@angular/common/http";
import { AuthenticationService } from "../login/authentication.service";

export class ApiError {
    get hasGlobalError(): boolean {
        if (this.globalError == null) return false;
        else return true;
    }

    globalError: string = null;

    get hasFormErrors(): boolean {
        if (this.formErrors.size == 0) return false;
        else return true;
    }

    hasFieldError(field: string): boolean {
        if (this.formErrors.has(field)) return true;
        else return false;
    }

    fieldError(field: string): string {
        return this.formErrors.get(field);
    }

    formErrors: Map<string, string>;
    httpError: HttpErrorResponse;

    constructor() {
        this.formErrors = new Map<string, string>();
    }
}

@Injectable()
export class HttpErrorInterceptor implements HttpInterceptor {

    constructor(private router: Router,
        private alertService: AlertService,
        private authenticationService: AuthenticationService
        ) { }

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        return next.handle(request).pipe(catchError(this.handleError));
    }

    private handleError(err: HttpErrorResponse) {
            let resultError = new ApiError();
            resultError.httpError = err;

            if (err.error instanceof Error) { // a network or programming error
                resultError.globalError = `An error occurred: ${err.error.message}`;
            } else { // the HTTP request was made but we got something other than 200 OK
                if (err.status == 404 || err.status == 504) {
                    resultError.globalError = `The requested resource was not found. The server could be down.`;
                }
                else if (err.status == 401) { // request not authorized
                    console.log(`HTTP: 401 redirecting to ${this.router?this.router.url : ""}`);
                    localStorage.removeItem('token');
                    if (this.router){
                        this.router.navigate(['/login', { returnUrl: this.router.url }]);
                        resultError.globalError = "Your session has expired. Please re-login.";
                        this.alertService.error(resultError.globalError, true);
                    }                    
                }
                else if (err.status >= 500 && err.status < 600) { // server errors
                    resultError.globalError = "A server error has occurred.";
                }
                else if (err.status == 400) { // bad request
                    // The web api received the request but the request wasn't correctly made.
                    // the error will either be one of:
                    // a string
                    // an RFC standard error object (https://tools.ietf.org/html/rfc7807)

                    if ((typeof err.error) === "string") {
                        resultError.globalError = err.error;
                    }
                    else {
                        resultError.globalError = "One or more errors occurred."
                        let errorList = err.error.errors;
                        for (var key in errorList) {
                            if (Array.isArray(errorList[key])) resultError.formErrors.set(key, errorList[key][0]);
                            else resultError.formErrors.set(key, errorList[key]);
                        }
                    }
                }
                else resultError.globalError = `Server returned code: ${err.status}, error message is: ${err.error}`;
            }

            // in the case of 401 prevent further error handlers from running so they don't need
            // to test for 401 to know whether to router.navigate or not
            if (err.status == 401) return empty();
            else return observableThrowError(resultError);
    }
}

