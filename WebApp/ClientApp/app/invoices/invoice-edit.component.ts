import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Location } from '@angular/common';
import 'rxjs/add/operator/switchMap';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';
import { InvoiceService } from "./invoice.service";
import { Invoice, IInvoice, IInvoiceLine, InvoiceLine } from "./invoice";
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

    // the copy of the invoice to reset to when the reset button is pushed
    private resetInvoice: IInvoice;

    // the model backing the form
    private modifyInvoice: IInvoice = new Invoice();

    private userAskedForAddress = false;
    private userAskedForContact = false;

    get requireAddress() : boolean {
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
    private addLineItem() {
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
        this.invoiceService.saveDraftInvoice(this.modifyInvoice)
            .subscribe(invoice => {
                this.resetInvoice = this.deepCopyInvoice(invoice);
                this.modifyInvoice = invoice;
                this.alertService.success("Invoice saved");
                console.log(invoice);
            },
            ((err: string) => this.alertService.error(err)));
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

