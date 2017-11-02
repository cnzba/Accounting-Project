import { Component, OnInit } from '@angular/core';
import { InvoiceService } from "./invoice.service";
import { IInvoice } from "./invoice";

@Component({
  selector: 'app-invoicelist',
  templateUrl: './invoicelist.component.html',
  styleUrls: ['./invoicelist.component.css']
})
export class InvoicelistComponent implements OnInit {
        InvoiceService: any;

    title: string = 'CBA Invoicing';
    invo: IInvoice[];
    selectedinvoi: IInvoice;
    errorMessage: string;

    // inject InvoiceService
    constructor(private invoiceService: InvoiceService) {
    }
    getInvoices(): void {
        this.InvoiceService.getInvoices().then(invo => this.invo = invo);
    }

    ngOnInit(): void {
        this.getInvoices();

  
              error => this.errorMessage = <any>error;
      }

      onSelect(inv: IInvoice): void {
          this.selectedinvoi = inv;
      }
  }

