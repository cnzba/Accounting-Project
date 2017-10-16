import { Component } from '@angular/core';
import { InvoiceService } from "./invoices/invoice.service";
import { IInvoice } from "./invoices/invoice";

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
