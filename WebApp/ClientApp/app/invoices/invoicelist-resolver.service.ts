import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

import { Observable } from 'rxjs/Observable';

import { IUser } from '../users/user';
import { IInvoice } from "./invoice";
import { InvoiceService } from "./invoice.service";

@Injectable()
export class InvoiceListResolver implements Resolve<IInvoice[]>
{
    constructor(private invoiceService: InvoiceService) { }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<IInvoice[]> {
        return this.invoiceService.getInvoices()
            .catch((error, caught) => {
                console.log('Cannot retrieve invoices: $(error)');
                return Observable.of(null);
            });
    }
}
