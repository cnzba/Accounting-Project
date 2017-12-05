import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Location } from '@angular/common';
import 'rxjs/add/operator/switchMap';
import { InvoiceService } from "./invoice.service";
import { IInvoice } from "./invoice";

@Component({
    selector: 'app-editinvoice',
    templateUrl: './editinvoice.component.html',
    styleUrls: ['./editinvoice.component.css']
})
export class EditinvoiceComponent implements OnInit {

    constructor(
        private invoiceService: InvoiceService,
        private route: ActivatedRoute,
        private location: Location
    ) { }
    private fieldArray: Array<any> = [];
    private selectedinvoi: any = {};
    addfield() {
        this.fieldArray.push(this.selectedinvoi)
        this.selectedinvoi = {};

    }
    deletefield(index) {
        this.fieldArray.splice(index, 1);

    }

    ngOnInit() {
    }

}
