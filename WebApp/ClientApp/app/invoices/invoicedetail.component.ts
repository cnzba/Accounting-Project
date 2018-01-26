import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Location } from '@angular/common';
import 'rxjs/add/operator/switchMap';
import { InvoiceService } from "./invoice.service";
import { IInvoice } from "./invoice";

@Component({
    selector: 'app-invoicedetail',
    templateUrl: './invoicedetail.component.html',
    styleUrls: ['./invoicedetail.component.css']
})
export class InvoicedetailComponent implements OnInit {
    title: string = 'CBA Invoicing';

    selectedinvoi: IInvoice;
    errorMessage: string;

    constructor(
        private invoiceService: InvoiceService,
        private route: ActivatedRoute,
        private location: Location
    ) { }
    ngOnInit(): void {
        this.route.paramMap
            .switchMap((params: ParamMap) => this.invoiceService.getInvoice(params.get('id')))
            .subscribe(invoices => {
                this.selectedinvoi = invoices;
            });

    }
    goBack(): void {
        this.location.back();
    }
}

