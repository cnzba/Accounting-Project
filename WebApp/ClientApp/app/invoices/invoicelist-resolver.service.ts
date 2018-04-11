import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import { InvoiceService } from "./invoice.service";
import { IInvoice } from "./invoice";

@Injectable()
export class InvoiceListResolver implements Resolve<IInvoice[]>
{
    constructor(private invoiceService: InvoiceService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<IInvoice[]> {
        return this.invoiceService.getInvoices()
            .catch((error, caught) => {
                return Observable.of(null);
            });
    }
}
