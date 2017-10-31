import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router'; 

import { AppComponent } from './app.component';
import { HttpClientModule } from "@angular/common/http";
import { InvoicelistComponent } from './invoices/invoicelist.component';
import { InvoicedetailComponent } from './invoices/invoicedetail.component';


@NgModule({
  declarations: [
    AppComponent,
    InvoicelistComponent,
    InvoicedetailComponent,
    
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
          { path: "details/:id", component: InvoicedetailComponent },

          
      ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
