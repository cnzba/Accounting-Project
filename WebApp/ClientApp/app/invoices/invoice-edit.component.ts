import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Location } from '@angular/common';
import 'rxjs/add/operator/switchMap';
import { InvoiceService } from "./invoice.service";
import { IInvoice } from "./invoice";




@Component({
    selector: 'app-invoice-edit',
    templateUrl: './invoice-edit.component.html',
    styleUrls: ['./invoice-edit.component.css']
})
export class InvoiceEditComponent implements OnInit {
    constructor(
        private invoiceService: InvoiceService,
        private route: ActivatedRoute,
        private location: Location
    ) { }

    private modifyInvoice: IInvoice;
    private selectedinvoiinvoiceLine: Array<any> = [];
    private newAttribute: any = {};

    addFieldValue() {
        this.selectedinvoiinvoiceLine.push(this.newAttribute)
        this.newAttribute = {};
    }

    deleteFieldValue(index) {
        this.selectedinvoiinvoiceLine.splice(index, 1);
    }
    reset(input: HTMLInputElement) {
        input.value = '';
    }
    submitted = false;

    onSubmit() {
        this.submitted = true;
        alert(`saved!!!`);
    }
    modify(): void {
        this.route.paramMap
            .switchMap((params: ParamMap) => this.invoiceService.getInvoice(params.get('id')))
            .subscribe(invoices => {
                this.modifyInvoice = invoices;
            });
        this.invoiceService.modifyInvoice(this.modifyInvoice);
    }
    create(): void {

        this.invoiceService.createInvoice(this.modifyInvoice);

    }


    ngOnInit() {





    }
}

