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
        //alert(`saved!!!`);

        console.log(this.modifyInvoice);
       // this.invoiceService.saveDraftInvoice(this.modifyInvoice).subscribe(invoices =>  console.log(invoices) );
    }
   
    ngOnInit() {
       // if (this.route.snapshot.url[1] == "edit")
            
        this.invoiceService.createNewInvoice().subscribe(
            (invoice: IInvoice) => this.modifyInvoice = invoice);

        console.log(this.route.snapshot);
        this.route.paramMap
            .switchMap((params: ParamMap) => this.invoiceService.getInvoice(params.get('id')))
           .subscribe(invoices => {
               this.modifyInvoice = invoices;
           });
       this.invoiceService.this.modifyInvoice(this.modifyInvoice);
    }
}

