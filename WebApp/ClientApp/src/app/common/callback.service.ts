import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class CallbackService {
    private updateNavSource = new Subject<boolean>();
    private paymentNavSource = new Subject<boolean>();

    updateNavObs$ = this.updateNavSource.asObservable();
    paymentNavObs$ = this.paymentNavSource.asObservable();

    updateNav(fpc: boolean) {
        this.updateNavSource.next(fpc);
    }

    paymentNav(show: boolean) {
        this.paymentNavSource.next(show);
    }
}
