import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { InvoicesComponent } from './invoices/invoices.component';
import { InvoicedetailComponent } from '../invoices/invoicedetail.component';

const routes: Routes = [
    {
        path: '',
        component: InvoicesComponent
    },
    {
        path: 'invoices',
        component: InvoicedetailComponent,
        outlet:'invoices',
    }
        ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReceivablesRoutingModule { }
