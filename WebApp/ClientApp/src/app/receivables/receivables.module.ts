import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ReceivablesRoutingModule } from './receivables-routing.module';
import { InvoicesComponent } from './invoices/invoices.component';

@NgModule({
  declarations: [InvoicesComponent],
  imports: [
    CommonModule,
    ReceivablesRoutingModule
  ]
})
export class ReceivablesModule { }
