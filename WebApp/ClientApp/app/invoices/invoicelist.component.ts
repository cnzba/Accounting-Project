import { Component, OnInit } from '@angular/core';
import { InvoiceService } from "./invoice.service";
import { IInvoice } from "./invoice";
import { AlertService } from "../alert/alert.service";

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
    constructor(private invoiceService: InvoiceService, private alertService: AlertService) {
    }
    getInvoices(): void {
        this.invoiceService.getInvoices().subscribe(invoices => {
            this.invo = invoices;
            this.alertService.success("");
        }, error => this.alertService.error(error));
    }

    ngOnInit(): void {
        this.alertService.success("Getting invoices...");
        this.getInvoices();
      }

     // onSelect(inv: IInvoice): void {
         // this.selectedinvoi = inv;
     // }
  }

