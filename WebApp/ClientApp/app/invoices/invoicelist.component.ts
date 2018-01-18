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
   route: any;
    InvoiceService: any;

    title: string = 'CBA Invoicing';
    invo: IInvoice[];
  

    errorMessage: string;

    // inject InvoiceService
    constructor(private invoiceService: InvoiceService, private alertService: AlertService) {
        this.invo = this.route.snapshot.data['invoices'];
    }
   deleteFieldValue(index) {
         this.invo.splice(index, 1);
      }
   
    ngOnInit(): void {
        
    }

    // onSelect(inv: IInvoice): void {
    // this.selectedinvoi = inv;
    // }
}


