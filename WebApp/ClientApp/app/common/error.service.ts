import { Injectable } from '@angular/core';
import { Observable } from "rxjs/Observable";
import { HttpErrorResponse } from "@angular/common/http";
import { Router } from "@angular/router";
import { AlertService } from "./alert/alert.service";

export class apiError {
    get hasGlobalError(): boolean {
        if (this.globalError == null) return false;
        else return true;
    }

    globalError: string = null;

    get hasFormErrors(): boolean {
        if (this.formErrors.size == 0) return false;
        else return true;
    }

    hasFieldError(field: string) : boolean {
        if (this.formErrors.has(field)) return true;
        else return false;
    }

    fieldError(field: string) : string {
        return this.formErrors.get(field);
    }

    formErrors: Map<string, string>;

    constructor() {
        this.formErrors = new Map<string, string>();
    }
}

@Injectable()
export class ErrorService {

    constructor(private router: Router, private alertService: AlertService) { }

    handleError(err: HttpErrorResponse) {
        let resultError = new apiError();

        if (err.error instanceof Error) {
            // A client-side or network error occurred. Handle it accordingly.
            resultError.globalError = `An error occurred: ${err.error.message}`;
        } else {
            // The backend returned an unsuccessful response code.
            if (err.status == 401) {
                this.router.navigate(['/login'])
                this.alertService.error("You session has expired. Please re-login.");
            }
            else if (err.status >= 500 && err.status < 600) {
                resultError.globalError = "A server error has occurred.";
            }
            else if (err.status == 400) {
                // the error will either be one of:
                // a string
                // a list of key/value pairs where the values are strings
                // a list of key/value pairs where the values are an array of strings
                // in both cases keys are strings and the key "" indicates a global error

                if ((typeof err.error) === "string") {
                    resultError.globalError = err.error;
                }
                else {
                    for (var key in err.error) {
                        if (Array.isArray(err.error[key])) resultError.formErrors.set(key, err.error[key][0]);
                        else resultError.formErrors.set(key, err.error[key]);
                    }
                }
            }
            else resultError.globalError = `Server returned code: ${err.status}, error message is: ${err.error}`;
        }

        return Observable.throw(resultError);
    }
}
