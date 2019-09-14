import { Injectable } from '@angular/core';
import { InvoiceService } from "./invoice.service";
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot, ParamMap } from '@angular/router';
import { Observable, of } from 'rxjs';
import { take, catchError } from 'rxjs/operators';

import { IInvoice } from "./invoice";
import { AlertService } from "../common/alert/alert.service";
import { ApiError } from "../common/error.service";

@Injectable()
export class InvoiceResolverService {

    constructor(private invoiceService: InvoiceService,
        private alertService: AlertService,
        private router: Router) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<IInvoice> {
        let id = route.paramMap.get('id');
        let data: Observable<IInvoice>;

        let loginId = localStorage.getItem('LoginId');

        if (id == null) data = this.invoiceService.createNewInvoiceWithInvoiceNumber(loginId);
        else data = this.invoiceService.getInvoice(id);

        return data.pipe(take(1), catchError((err: ApiError) => {
            this.router.navigate(['/invoices']);
            this.alertService.error(err.globalError, true);
            return of(null);
        }));
    }
}
