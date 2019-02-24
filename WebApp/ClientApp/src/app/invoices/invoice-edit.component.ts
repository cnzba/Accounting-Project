import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Location } from '@angular/common';

import { Observable } from 'rxjs';

import { InvoiceService } from "./invoice.service";
import { Invoice, IInvoice, IInvoiceLine, InvoiceLine } from "./invoice";
import { AlertService } from "../common/alert/alert.service";
import { ApiError } from "../common/error.service";
import { SpinnerService } from "../common/spinner.service";

// TODO
// client-side validation for due date
// remove a success alert message when the form is touched or dirtied
// check invoice (as opposed to DraftInvoice) validation and use attributes where possible
// validate line items (description not empty; amounts default to 0)

@Component({
    selector: 'app-invoice-edit',
    templateUrl: './invoice-edit.component.html',
    styleUrls: ['./invoice-edit.component.css']
})
export class InvoiceEditComponent implements OnInit {
    constructor(
        private invoiceServiceP: InvoiceService,
        private route: ActivatedRoute,
        private location: Location,
        private alertService: AlertService,
        private spinnerService: SpinnerService) { }

    invoiceService: InvoiceService = this.invoiceServiceP;
    // the copy of the invoice to reset to when the reset button is pushed
    private resetInvoice: IInvoice;

    // the model backing the form
    modifyInvoice: IInvoice = new Invoice();

    // model for any errors on the form
    formErrors: ApiError = new ApiError();

    private userAskedForAddress = false;
    private userAskedForContact = false;

    get requireAddress(): boolean {
        return this.modifyInvoice.grandTotal >= 1000;
    }

    get showAddress(): boolean {
        if (this.requireAddress) return true;
        else if (this.modifyInvoice.clientContact.length > 0) return true;
        else return this.userAskedForAddress;
    }

    get showContact(): boolean {
        if (this.modifyInvoice.clientContactPerson.length > 0) return true;
        else return this.userAskedForContact;
    }

    // button actions
    addAddress() {
        this.userAskedForAddress = true;
    }

    addContact() {
        this.userAskedForContact = true;
    }

    removeAddress() {
        this.userAskedForAddress = false;
        this.modifyInvoice.clientContact = "";
    }

    removeContact() {
        this.userAskedForContact = false;
        this.modifyInvoice.clientContactPerson = "";
    }

    // code for line items
    addLineItem() {
        this.modifyInvoice.invoiceLine.push(new InvoiceLine());
    }

    private deleteLineItem(i: number) {
        this.modifyInvoice.invoiceLine.splice(i, 1);
    }

    private moveItemUp(i: number) {
        if (i <= 0) return;
        let il = this.modifyInvoice.invoiceLine;

        [il[i - 1], il[i]] = [il[i], il[i - 1]];
    }

    private moveItemDown(i: number) {
        let il = this.modifyInvoice.invoiceLine;
        if (i >= (il.length - 1)) return;

        [il[i + 1], il[i]] = [il[i], il[i + 1]];
    }

    private deepCopyInvoice(invoice: IInvoice): IInvoice {
        return JSON.parse(JSON.stringify(invoice));
    }

    onSubmit() {
        this.spinnerService.showSpinner();
        this.formErrors = new ApiError();

        this.invoiceService.saveDraftInvoice(this.modifyInvoice)
            .subscribe(invoice => { 
                this.resetInvoice = this.deepCopyInvoice(invoice);
                this.modifyInvoice = invoice;
                this.spinnerService.hideSpinner()
                this.alertService.success("Invoice saved");
                console.log(invoice);
            },
            ((err: ApiError) => {
                this.formErrors = err;
                this.spinnerService.hideSpinner()
                if (err.globalError) this.alertService.error(err.globalError);
            }));
    }

    onReset() {
        this.alertService.clear();
        this.modifyInvoice = this.deepCopyInvoice(this.resetInvoice);
    }

    ngOnInit() {
        this.route.data.subscribe((data: { invoice: IInvoice }) => {
            this.modifyInvoice = data.invoice;
            this.resetInvoice = this.deepCopyInvoice(this.modifyInvoice);
        });
    }


}

