import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Location } from '@angular/common';
import { RouterModule, Router } from '@angular/router';
import 'rxjs/add/operator/switchMap';
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

    errorMessage: string;

    // inject InvoiceService
    constructor(
        private invoiceService: InvoiceService,
        private route: ActivatedRoute,
        private location: Location,
        private router: Router
    ) { }
    getInvoices(): void {
        this.invoiceService.getInvoices().subscribe(invoices => {
            this.invo = invoices;
        });
    }
   
    modify() {
        this.router.navigate(['invoices/edit/:id']);

    }
    
    ngOnInit(): void {
        this.getInvoices();


        error => this.errorMessage = <any>error;


        // onSelect(inv: IInvoice): void {
        // this.selectedinvoi = inv;
        // }
    }

}
