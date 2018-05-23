import { Component, OnInit, Pipe, PipeTransform, Injectable  } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { InvoiceService } from "./invoice.service";
import { IInvoice, IInvoiceLine } from "./invoice";
import { AlertService } from "../alert/alert.service";

import { PaginationComponent } from '../pagination/pagination.component';
import { DecimalPipe } from '@angular/common/src/pipes/number_pipe';







@Component({
    selector: 'app-invoicelist',
    templateUrl: './invoicelist.component.html',
    styleUrls: ['./invoicelist.component.css'],

})



export class InvoicelistComponent implements OnInit {
    invoiceNumber: string;
    selected:any;
    //selectedData:any;
    filtered: any;
   
    status = ['Select Status', 'All', 'Unpaid and sent', 'Unpaid with due date', 'Paid', 'Open', 'Overdue'];
   
    // offset is the index of an invoice we want to view and is used to compute the page to show; offset = 3 for example means display the page containing the 4th invoice in the list

    // offset needs to be initialized
    offset: number = 0;
    // page: number = 1;
    limit: number = 1;
    title: string = 'CBA Invoicing';
    // invo: IInvoice;

    errorMessage: string;

    _listFilter: string;
    get listFilter(): string {
        return this._listFilter;
    }
    set listFilter(value: string) {
        this._listFilter = value;
        this.filteredInvoice = this.listFilter ? this.performFilter(this.listFilter) : this.invo;
    }
    filteredInvoice: IInvoice[];
    invo: IInvoice[] = [];


    // inject InvoiceService
    constructor(private route: ActivatedRoute, private _invoiceService: InvoiceService, private alertService: AlertService, private http: Http) {
       // this.selected = this.stat;
        this.invo = this.route.snapshot.data['invoices'];
       
    }
    onPageChange(offset) {
        this.offset = offset;
    }
    onOptionsSelected() {
       // let value = event.target.value;
      //  this.selected = value;
        console.log(this.selected);
        this.filtered = this.filteredInvoice.filter(t => t.status == this.selected);
        
    }

    
    

    performFilter(filterBy: any): IInvoice[] {
       filterBy = filterBy.toLocaleLowerCase();
        return this.invo.filter((inv: IInvoice) =>
            (inv.clientName.toLocaleLowerCase().indexOf(filterBy) !== -1) || (inv.invoiceNumber.toLocaleLowerCase().indexOf(filterBy) !== -1)
            || (inv.status.toLocaleLowerCase().indexOf(filterBy) !== -1));
           

    
        
    }
   
 
   

    ngOnInit(): void {
        this._invoiceService.getInvoices()
            .subscribe(invo => {
                this.invo = invo;
                this.filteredInvoice = this.invo;
            },
            error => this.errorMessage = <any>error);
    }
       
    }

   
    

               
   
    
    




