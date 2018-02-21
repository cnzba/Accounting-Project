import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';

@Injectable()
export class CallbackService {
    private updateNavSource = new Subject<boolean>();

    updateNavObs$ = this.updateNavSource.asObservable();

    updateNav(fpc: boolean) {
        this.updateNavSource.next(fpc);
    }
}
