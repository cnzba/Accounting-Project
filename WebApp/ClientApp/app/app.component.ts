import { Component } from '@angular/core';
import { InvoiceService } from "./invoices/invoice.service";
import { IUser } from "./users/user";
import { AuthenticationService } from "./login/authentication.service";
import { AlertService } from "./alert/alert.service";

import { Router, Event, NavigationStart, NavigationEnd, NavigationError, NavigationCancel } from '@angular/router';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    providers: [InvoiceService]
})

export class AppComponent {
    title = 'CBA Invoicing';
    loading: boolean = true;

    currentUser: IUser;
    showUser: boolean;

    constructor(private authenticationService: AuthenticationService,
        private alertService: AlertService, private router: Router) {
        router.events.subscribe((routerEvent: Event) => this.checkRouterEvent(routerEvent));
    }

    ngOnInit() {
        this.alertService.success("Loading ...");
        this.authenticationService.getCurrentUser().subscribe(
            (user: IUser) => {
                this.currentUser = user;
                this.showUser = true;
            });
    }

    checkRouterEvent(routerEvent: Event): void {
        if (routerEvent instanceof NavigationStart) {
            this.loading = true;
        }

        if (routerEvent instanceof NavigationEnd ||
            routerEvent instanceof NavigationCancel ||
            routerEvent instanceof NavigationError) {
            this.loading = false;
        }
    }
}
