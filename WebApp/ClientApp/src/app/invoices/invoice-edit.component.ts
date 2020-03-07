import { Component, OnInit, TemplateRef, SimpleChange } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Location } from '@angular/common';
import {BsModalService, BsModalRef} from 'ngx-bootstrap/modal'

import { Observable } from 'rxjs';

import { InvoiceService } from "./invoice.service";
import { Invoice, IInvoice, IInvoiceLine, InvoiceLine } from "./invoice";
import { AlertService } from "../common/alert/alert.service";
import { ApiError } from "../common/error.service";
import { SpinnerService } from "../common/spinner.service";

import { Router } from '@angular/router';

//import { AuthenticationService } from "../login/authentication.service";

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
        private router: Router,
        private invoiceServiceP: InvoiceService,
        private route: ActivatedRoute,
        private location: Location,
        private alertService: AlertService,
        private spinnerService: SpinnerService,
        private modalService: BsModalService) { }

    invoiceService: InvoiceService = this.invoiceServiceP;
    

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
        else if (this.modifyInvoice.clientContact != null && this.modifyInvoice.clientContact.length > 0) return true;
        else return this.userAskedForAddress;
    }

    get showContact(): boolean {
        if (this.modifyInvoice.clientContactPerson != null && this.modifyInvoice.clientContactPerson.length > 0) return true;
        else return this.userAskedForContact;
    }

    // determine whether to show the finalise and send button
    get showFinalise(): boolean {
        return this.modifyInvoice.status == "Draft";
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

    private updateInvoiceLineAmountByUnitPrice(unitPrice: number, i: number) {
        this.modifyInvoice.invoiceLine[i].unitPrice = unitPrice;
        this.modifyInvoice.invoiceLine[i].amount = parseFloat((this.modifyInvoice.invoiceLine[i].quantity * unitPrice).toFixed(2));
    }

    private updateInvoiceLineAmountByQuantity(quantity: number, i: number) {
        this.modifyInvoice.invoiceLine[i].quantity = quantity;
        this.modifyInvoice.invoiceLine[i].amount = parseFloat((this.modifyInvoice.invoiceLine[i].unitPrice * quantity).toFixed(2));
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

    // replace the invoice used by the form with the provided invoice
    private updateInvoiceModel(invoice: IInvoice) {
        this.modifyInvoice = invoice;
    }

    async onSaveDraft() {
        this.spinnerService.showSpinner();
        this.formErrors = new ApiError();

        this.modifyInvoice.loginId = localStorage.getItem('LoginId');

        try {
            let invoice: IInvoice = await this.invoiceService.saveDraftInvoice(this.modifyInvoice).toPromise();

            this.updateInvoiceModel(invoice);
            this.spinnerService.hideSpinner()
            this.alertService.success("Invoice saved");
            console.log(invoice);
        }
        catch (error) {
            console.log(`There was an error finalising the invoice ${JSON.stringify(error)}`);
            let err: ApiError = error;

            this.formErrors = err;
            this.spinnerService.hideSpinner()
            if (err.globalError) this.alertService.error(err.globalError);
        }
    }

    async onFinalise() {
        console.log(`Finalising invoice ${this.modifyInvoice.invoiceNumber}`);
        this.spinnerService.showSpinner();
        this.formErrors = new ApiError();

        this.modifyInvoice.loginId = localStorage.getItem('LoginId');

        try {
            let invoiceResult = await this.invoiceService.saveDraftInvoice(this.modifyInvoice).toPromise();
            invoiceResult = await this.invoiceService.finaliseInvoice(invoiceResult.invoiceNumber).toPromise();

            this.updateInvoiceModel(invoiceResult);
            this.spinnerService.hideSpinner()
            this.router.navigate(["invoices"]);
            this.alertService.success(`Invoice ${invoiceResult.invoiceNumber} finalised and sent`);
        }
        catch (error) {
            console.log(`There was an error finalising the invoice ${JSON.stringify(error)}`);
            let err: ApiError = error;

            this.formErrors = err;
            this.spinnerService.hideSpinner()
            if (err.globalError) this.alertService.error(err.globalError);
        }
        finally{
            this.modalRef.hide();
        }
    }

    onShowFinalise(template: TemplateRef<any>){
        this.modalRef = this.modalService.show(template,{class:'modal-md'});
    }

    onCancelFinalise(): void{
        this.modalRef.hide();
    }


    /**Methods for modal */
    modalRef: BsModalRef;
    orgInvoice: string; //Keep the content of the invoice when initiate the component                        

    onCancel(template: TemplateRef<any>) {
        if (JSON.stringify(this.modifyInvoice) === this.orgInvoice) {
            console.log("no change");
            this.router.navigate(["invoices"]);
        } else {
            this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
        }
    };
    
    confirm(): void {
        this.router.navigate(["invoices"]);
        this.modalRef.hide();
    }

    decline(): void {
        this.modalRef.hide();
    }    

    
    ngOnInit() {
        this.route.data.subscribe((data: { invoice: IInvoice }) => {            
            this.orgInvoice = JSON.stringify(data.invoice);
            this.modifyInvoice = data.invoice;
        });
    }
    
}

