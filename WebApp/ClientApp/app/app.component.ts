import { Component } from '@angular/core';
import { InvoiceService } from "./invoices/invoice.service";


@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [InvoiceService]
})
export class AppComponent {
    title = 'CBA Invoicing';
}
