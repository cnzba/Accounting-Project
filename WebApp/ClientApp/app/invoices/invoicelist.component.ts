import { Component, OnInit } from '@angular/core';
import { InvoiceService } from "./invoice.service";
import { IInvoice, IInvoiceLine } from "./invoice";
import { AlertService } from "../alert/alert.service";
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-invoicelist',
    templateUrl: './invoicelist.component.html',
    styleUrls: ['./invoicelist.component.css']
})
export class InvoicelistComponent implements OnInit {
 
    InvoiceService: any;

    title: string = 'CBA Invoicing';
    invo: IInvoice[];
  

    errorMessage: string;

    // inject InvoiceService
    constructor(private route: ActivatedRoute,private invoiceService: InvoiceService, private alertService: AlertService) {
        this.invo = this.route.snapshot.data['invoices'];
    }
  // deleteFieldValue(index) {
       //  this.invo.splice(index, 1);
    //  }
   
  // getInvoices(): void {
     //  this.invoiceService.getInvoices().subscribe(invoices => {
         //  this.invo = invoices;
         //  this.alertService.success("");
     //  }, error => this.alertService.error(error));
 //  }

   ngOnInit(): void {
      // this.alertService.success("Getting invoices...");
     //  this.getInvoices();
   }


    // onSelect(inv: IInvoice): void {
    // this.selectedinvoi = inv;
    // }
}


