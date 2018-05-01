import { Component, OnInit, Pipe, PipeTransform, Injectable  } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { InvoiceService } from "./invoice.service";
import { IInvoice, IInvoiceLine } from "./invoice";
import { AlertService } from "../alert/alert.service";

import { PaginationComponent } from '../pagination/pagination.component';







@Component({
    selector: 'app-invoicelist',
    templateUrl: './invoicelist.component.html',
    styleUrls: ['./invoicelist.component.css'],

})


@Injectable()
export class InvoicelistComponent implements OnInit{
    
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
        this.invo = this.route.snapshot.data['invoices'];
    }
   onPageChange(offset) {
        this.offset = offset;
   }
    
    performFilter(filterBy: string): IInvoice[] {
        filterBy = filterBy.toLocaleLowerCase();
        return this.invo.filter((inv: IInvoice) =>
            inv.clientName.toLocaleLowerCase().indexOf(filterBy) !== -1);
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

   
    

               
   
    
    




