import { Injectable } from '@angular/core';
import { InvoiceService } from "./invoice.service";
import { Resolve, Router, ActivatedRouteSnapshot, RouterStateSnapshot, ParamMap } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/switchMap';
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

        if (id == null) data = this.invoiceService.createNewInvoice();
        else data = this.invoiceService.getInvoice(id);

        return data.take(1).catch((err: ApiError) => {
            this.router.navigate(['/invoices']);
            this.alertService.error(err.globalError, true);
            return Observable.of(null);
        });
    }
}
