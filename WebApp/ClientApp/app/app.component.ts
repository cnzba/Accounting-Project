import { Component } from '@angular/core';
import { InvoiceService } from "./invoices/invoice.service";
import { IInvoice } from "./invoices/invoice";


//export class Invoice {
//    id: string;
//    date: string;
//    client: string;
//    amount: number;

//    due: string;
//    status: string;
//    GST: string;
//    charnum: string;
//    des: string;
//}

//const INVOICES: Invoice[] = [
//    {
//        id: ' INV-00050', date: '10/5/2016', client: 'Electrocal Commission c/o Glen Clarke', amount: 25, due: 'Paid', status: 'sent',
//        GST: '$-712-551', charnum: '21479', des: 'Fundraising Dinner'
//    },
//    {
//        id: ' INV-00051', date: '20/6/2016', client: 'John Smith', amount: 15, due: '15/6/2017', status: 'sent', GST: '96-345-234',
//        charnum: '234578', des: 'Donation'
//    }
//];

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [InvoiceService]
})
export class AppComponent {
    title: string = 'CBA Invoicing';
    invo :IInvoice[] = [];
    selectedinvoi: IInvoice;
    errorMessage: string;

    // inject InvoiceService
    constructor(private invoiceService: InvoiceService) {
    }

    // populate list of invoices on component initialisation
    ngOnInit(): void {
        this.invoiceService.getInvoices()
            .subscribe(
            invoices => {
                this.invo = invoices;
            },
            error => this.errorMessage = <any>error);
    }

    onSelect(inv: IInvoice): void {
        this.selectedinvoi = inv;
    }

}
