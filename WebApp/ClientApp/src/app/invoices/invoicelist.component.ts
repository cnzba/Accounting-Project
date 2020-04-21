import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { InvoiceService } from "./invoice.service";
import { IInvoice, IInvoiceLine } from "./invoice";
import { AlertService } from "../common/alert/alert.service";

import { PaginationComponent } from '../pagination/pagination.component';

import { Sort } from '@angular/material';
import { InvoiceFilterPipe } from '../pipes/invoice-filter.pipe';
import { saveAs } from 'file-saver';
import { error } from 'protractor';
import { stat } from 'fs';
import { datepickerAnimation } from 'ngx-bootstrap/datepicker/datepicker-animations';


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
        let status = this.route.snapshot.queryParams["status"];
//        this.route.queryParams.subscribe(param =>{console.log(param["status"])})
        this.invo = this.route.snapshot.data['invoices'];        
        let allInvoices = this.invo.slice();
        if (status && status == "overdue"){
            this.sortedData = allInvoices.filter(
                invoice => { 
                    let due = new Date(invoice.dateDue);
                    return due.getTime()< Date.now() 
                        && invoice.status.toLowerCase() != "paid"
                        && +invoice.dateCreated.toString().slice(0,4) == (new Date).getFullYear();
                }
            )
        }else if (status){
            this.sortedData = allInvoices.filter(
                    invoice => invoice.status.toLowerCase() == status.toLowerCase()
                            && +invoice.dateCreated.toString().slice(0,4) == (new Date).getFullYear());
        }else{
            this.sortedData = allInvoices;            
        }
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
    
    deleteFieldValue() {

        // get invoice number to delete
        const invoiceNumber = this.getinvoiceNum();

        // Only Draft invoices can be deleted. otherwise exit.
        let inv: IInvoice[] = this.invo.filter((el) => el.invoiceNumber === invoiceNumber);
        if (inv.length > 0 && inv[0].status !== 'Draft') {
            this.alertService.error("You can not delete a none draft invoice.");
            return;
        }

        // Delete the invoice
        this.invoiceService.deleteInvoice(invoiceNumber).subscribe(
            data => {
                // Remove deleted invoice from list of invo
                this.invo = this.invo.filter(inv => inv.invoiceNumber !== invoiceNumber);
                // Reset pagination offset
                this.offset = 0;
                // Show delete confirmation on page
                this.alertService.success(invoiceNumber + " has successfully been deleted!");
                // find invoice index to remove from presentation list
                let delIndex = this.sortedData.findIndex((el) => el.invoiceNumber === invoiceNumber);
                // Remove deleted invoice from presentation list
                if (delIndex >= 0) {
                    this.sortedData.splice(delIndex, 1);
                }
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

    getPdfInvoice(invoiceNumber: string) {
        this.invoiceService.getPdfInvoice(invoiceNumber)
            .subscribe(
                success => {
                    saveAs(success, invoiceNumber + '.pdf');
                },
                error => {
                    alert('Server error while downloading file.');
                }
        );
    }
   
    ngOnInit(): void {
       
    }
}

function compare(a: number | string | Date, b: number | string | Date, isAsc: boolean) {
    return (a < b ? -1 : 1) * (isAsc ? 1 : -1);
}
