import { Component, OnInit } from '@angular/core';
import { InvoiceService } from "./invoice.service";
import { IInvoice } from "./invoice";
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
    constructor(private route: ActivatedRoute, private alertService: AlertService) {
        this.invo = this.route.snapshot.data['invoices'];
    }
  
    ngOnInit(): void {}
  }

