import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { InvoiceService } from "./invoice.service";
import { IInvoice, IInvoiceLine } from "./invoice";
import { AlertService } from "../common/alert/alert.service";

import { PaginationComponent } from '../pagination/pagination.component';







@Component({
    selector: 'app-invoicelist',
    templateUrl: './invoicelist.component.html',
    styleUrls: ['./invoicelist.component.css'],

})
export class InvoicelistComponent implements OnInit{
    
    // offset is the index of an invoice we want to view and is used to compute the page to show; offset = 3 for example means display the page containing the 4th invoice in the list

    // offset needs to be initialized
    offset: number = 0;
   // page: number = 1;
    limit: number = 1;
    title: string = 'CBA Invoicing';
    invo: IInvoice;

    errorMessage: string;
  
    // inject InvoiceService
    constructor(private route: ActivatedRoute, private invoiceService: InvoiceService, private alertService: AlertService, private http: Http) {
        this.invo = this.route.snapshot.data['invoices'];
    }
    onPageChange(offset) {
        this.offset = offset;
    }

    
   
    ngOnInit(): void {
       
    }

   
    

               
   
    
    }



