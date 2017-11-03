import { Component } from '@angular/core';
import { InvoiceService } from "./invoices/invoice.service";
import { User } from "./users/user";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [InvoiceService]
})

export class AppComponent {
    title = 'CBA Invoicing';
    currentUser: User;

    ngDoCheck() {
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    }
}
