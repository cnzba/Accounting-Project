import { Component } from '@angular/core';
import { InvoiceService } from "./invoices/invoice.service";
import { User } from "./users/user";
import { AuthenticationService } from "./login/authentication.service";

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
  providers: [InvoiceService]
})

export class AppComponent {
    title = 'CBA Invoicing';
    currentUser: User;
    constructor(private authenticationService: AuthenticationService) { }
    ngDoCheck() {
        this.currentUser = this.authenticationService.getCurrentUser();
    }
}
