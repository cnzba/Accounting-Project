import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Location } from '@angular/common';
import 'rxjs/add/operator/switchMap';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import { InvoiceService } from "./invoice.service";
import { IInvoice } from "./invoice";
import { AlertService } from "../alert/alert.service";

@Component({
    selector: 'app-invoice-edit',
    templateUrl: './invoice-edit.component.html',
    styleUrls: ['./invoice-edit.component.css']
})
export class InvoiceEditComponent implements OnInit {
    constructor(
        private invoiceService: InvoiceService,
        private route: ActivatedRoute,
        private location: Location,
        private alertService: AlertService) { }

    private modifyInvoice: IInvoice;
    private resetInvoice: IInvoice;

    // private modifyInvoice: any[] = [];

    //< private newAttribute: any = {};

    // addFieldValue() {
    // this.modifyInvoice.push(this.newAttribute)
    //   this.newAttribute = {};
    // }

    // deleteFieldValue(index) {
    //      this.modifyInvoice.splice(index, 1);
    //  }
    // reset(input: HTMLInputElement) {
    // input.value = '';
    // }
    submitted = false;

    onSubmit() {
        this.submitted = true;
        console.log(this.modifyInvoice);
        this.invoiceService.saveDraftInvoice(this.modifyInvoice)
            .subscribe(invoice => {
                this.resetInvoice = this.deepCopyInvoice(invoice);
                this.alertService.success("Invoice saved");
                console.log(invoice);
            },
            ((err: string) => this.alertService.error(err)));
    }

    private deepCopyInvoice(invoice: IInvoice) : IInvoice {
        return JSON.parse(JSON.stringify(invoice));
    }

    onReset() {
        this.alertService.clear();
        this.modifyInvoice = this.deepCopyInvoice(this.resetInvoice);
    }

    ngOnInit() {
        // select between creating a new invoice or modifying existing invoice depending on the url (route)
        this.route.paramMap.switchMap((params: ParamMap) => {
            let id = params.get('id');
            let data: Observable<IInvoice>;

            if (id == null) data = this.invoiceService.createNewInvoice();
            else data = this.invoiceService.getInvoice(id);

            return data;
        }).subscribe((invoice: IInvoice) => {
            this.modifyInvoice = invoice;
            this.resetInvoice = this.deepCopyInvoice(this.modifyInvoice);
        });
    }
}

