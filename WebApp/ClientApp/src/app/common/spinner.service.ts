import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

@Injectable()
export class SpinnerService {
    private spinnerSource = new Subject<boolean>();
    private spinnerObs$ = this.spinnerSource.asObservable();

    constructor() { }

    getSpinner(): Observable<boolean> {
        return this.spinnerObs$;
    }

    showSpinner(): void {
        this.spinnerSource.next(true);
    }

    hideSpinner(): void {
        this.spinnerSource.next(false);
    }

}
