import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { InvoiceService } from "./invoice.service";
import { IInvoice } from "./invoice";
import { AlertService } from "../common/alert/alert.service";
import { ApiError } from "../common/error.service";

@Injectable()
export class InvoiceListResolver implements Resolve<IInvoice[]>
{
    constructor(private invoiceService: InvoiceService,
        private alertService: AlertService,
        private router: Router) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<IInvoice[]> {
        return this.invoiceService.getInvoices().take(1).catch((err: ApiError) => {
            this.router.navigate(['/invoices']);
            this.alertService.error(err.globalError, true);
            return Observable.of(null);
        });
    }
}
