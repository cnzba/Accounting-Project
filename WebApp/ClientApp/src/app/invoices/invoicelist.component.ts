import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { InvoiceService } from "./invoice.service";
import { IInvoice, IInvoiceLine } from "./invoice";
import { AlertService } from "../common/alert/alert.service";

import { PaginationComponent } from '../pagination/pagination.component';

import { Sort } from '@angular/material';
import { InvoiceFilterPipe } from '../pipes/invoice-filter.pipe';


@Component({
    selector: 'app-invoicelist',    
    templateUrl: './invoicelist.component.html',
    styleUrls: ['./invoicelist.component.css'],
    

})
export class InvoicelistComponent implements OnInit {
    

    // offset is the index of an invoice we want to view and is used to compute the page to show; offset = 3 for example means display the page containing the 4th invoice in the list

    // offset needs to be initialized
    offset: number = 0;
    // page: number = 1;
    limit: number = 20;
    title: string = 'CBA Invoicing';
    invo: IInvoice[];
    sortedData: IInvoice[];

    errorMessage: string;

    public searchString: string;

    // inject InvoiceService
    constructor(private route: ActivatedRoute, private invoiceService: InvoiceService, private alertService: AlertService, private http: Http) {
        this.invo = this.route.snapshot.data['invoices'];
        this.sortedData = this.invo.slice();
    }
    onPageChange(offset) {
        this.offset = offset;
       
    }
    
    private _invoiceNumber: string;
    getinvoiceNum():string{
       return this._invoiceNumber;
           } 
    setinvoiceNum(currentInvoiceNum:string){
         this._invoiceNumber = currentInvoiceNum;
       }
    
    deleteFieldValue(invoiceNumber: string){
            var delete_value = this.getinvoiceNum();
            invoiceNumber = delete_value;
            this.invoiceService.deleteInvoice(invoiceNumber).subscribe(
               data => {
               //window.location.reload();
               this.invo = this.invo.filter(inv=> inv.invoiceNumber !== invoiceNumber);
               this.offset=0;
               this.alertService.success(invoiceNumber +" has successfully been deleted!");
                   //this.invo = this.route.snapshot.data['invoices'];
               let delIndex = 0;
               for (let i = 0; i < this.sortedData.length; i++) {
                   if (this.sortedData[i].invoiceNumber == invoiceNumber)
                       delIndex = i;
               };
               this.sortedData.splice(delIndex, 1);
            },
              err=> {
                this.alertService.error("Error: the delete has failed")
            });

    }
       

    sortData(sort: Sort) {
        const data = this.invo.slice();
        if (!sort.active || sort.direction === '') {
            this.sortedData = data;
            return;
        }

        this.sortedData = data.sort((a, b) => {
            const isAsc = sort.direction === 'asc';
            const isDesc = sort.direction === 'desc';
            switch (sort.active) {
                case 'invoiceNumber': return compare(a.invoiceNumber, b.invoiceNumber, isAsc);
                case 'clientName': return compare(a.clientName.toLowerCase(), b.clientName.toLowerCase(), isDesc);
                case 'grandTotal': return compare(a.grandTotal, b.grandTotal, isAsc);
                case 'status': return compare(a.status, b.status, isDesc);
                case 'dateCreated': return compare(a.dateCreated, b.dateCreated, isAsc);
                case 'dateDue': return compare(a.dateDue, b.dateDue, isAsc);
                default: return 0;
            }
        });
    }
   
    ngOnInit(): void {
       
    }
}

function compare(a: number | string | Date, b: number | string | Date, isAsc: boolean) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
