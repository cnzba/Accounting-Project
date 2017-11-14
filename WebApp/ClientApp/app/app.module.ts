import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router'; 
import 'rxjs/add/operator/map';

import { AppComponent } from './app.component';
import { HttpClientModule } from "@angular/common/http";
import { InvoicelistComponent } from './invoices/invoicelist.component';
import { InvoicedetailComponent } from './invoices/invoicedetail.component';
import { EditinvoiceComponent } from './invoices/editinvoice.component';
import { InvoiceService } from "./invoices/invoice.service";

@NgModule({
  declarations: [
    AppComponent,
    InvoicelistComponent,
      InvoicedetailComponent,
    EditinvoiceComponent
    
  ],
  imports: [
      BrowserModule,
      FormsModule,
      HttpClientModule,
      RouterModule.forRoot([
          {
              path: "",
              component: InvoicelistComponent
          },
          { path: "invoices/:id", component: InvoicedetailComponent },
          {
              path: "editinv",
              component:EditinvoiceComponent
          }
          
      ])
  ],
  providers: [InvoiceService],
  bootstrap: [AppComponent]
})
export class AppModule { }
