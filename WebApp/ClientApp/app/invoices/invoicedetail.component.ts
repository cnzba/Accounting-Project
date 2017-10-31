import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, ParamMap } from "@angular/router";
import { Location } from '@angular/common';
import 'rxjs/add/operator/switchMap';
import { InvoiceService } from "./invoice.service";
export class IInvoice {
    id: number;
    date: string;
    client: string;
    amount: number;

    due: string;
    status: string;
    GST: string;
    charnum: string;
    des: string;



}


const INVOICES: IInvoice[] = [
    {
        id: 50, date: '10/5/2016', client: 'Electrocal Commission c/o Glen Clarke', amount: 25, due: 'Paid', status: 'sent',
        GST: '$-712-551', charnum: '21479', des: 'Fundraising Dinner'
    },
    {
        id: 51, date: '20/6/2016', client: 'John Smith', amount: 15, due: '15/6/2017', status: 'sent', GST: '96-345-234',
        charnum: '234578', des: 'Donation'
    }


];

@Component({
  selector: 'app-invoicedetail',
  templateUrl: './invoicedetail.component.html',
  styleUrls: ['./invoicedetail.component.css']
})
export class InvoicedetailComponent implements OnInit {

    invoices = new IInvoice();

    constructor(
        private invoiceService: InvoiceService,
        private route: ActivatedRoute,
        private location: Location
    ) { }
    ngOnInit(): void {
        this.route.paramMap
            .switchMap((params: ParamMap) => this.invoiceService.getInvoice(+params.get('id')))
            .subscribe(invoices => this.invoices = invoices);
    }
    goBack(): void {
        this.location.back();
    }
}
