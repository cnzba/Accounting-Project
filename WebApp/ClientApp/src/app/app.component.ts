import { Component } from '@angular/core';
import { InvoiceService } from "./invoices/invoice.service";
import { IUser } from "./users/user";
import { AuthenticationService } from "./login/authentication.service";
import { AlertService } from "./common/alert/alert.service";

import { Router, Event, NavigationStart, NavigationEnd, NavigationError, NavigationCancel } from '@angular/router';
import { CallbackService } from './common/callback.service';
import { SpinnerService } from "./common/spinner.service";
import { MatSidenavModule } from '@angular/material';

@Component({
    selector: 'app-root',
    templateUrl: './app.component.html',
    styleUrls: ['./app.component.css'],
    providers: [InvoiceService, CallbackService]
})

export class AppComponent {
    title = 'CBA Invoicing';
    loading: boolean = true;
    forcePasswordChange: boolean = false;

    currentUser: IUser;
    showUser: boolean;

    constructor(private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private router: Router,
        private callbackService: CallbackService,
        private spinnerService: SpinnerService) {

        spinnerService.getSpinner().subscribe(show => {
            this.loading = show;
        });

        router.events.subscribe((routerEvent: Event) => this.checkRouterEvent(routerEvent));

        callbackService.updateNavObs$.subscribe(fpc => {
            this.forcePasswordChange = fpc;
        });
        callbackService.paymentNavObs$.subscribe(show => {
            if (this.showUser)
                this.showUser = false;
            if (this.currentUser)
                this.currentUser = null;
            this.loading = show;
        });
    }

    ngOnInit() {
        this.alertService.success("Loading ...");
        this.authenticationService.getCurrentUser().subscribe(
            (user: IUser) => {
                this.currentUser = user;
                this.showUser = true;
            });
        this.forcePasswordChange = localStorage.getItem("forcePasswordChange") === "true";

    }

    onLogout(){
        console.log("logout click");
    }

    checkRouterEvent(routerEvent: Event): void {
        if (routerEvent instanceof NavigationStart) {
            this.spinnerService.showSpinner();
        }

        if (routerEvent instanceof NavigationEnd ||
            routerEvent instanceof NavigationCancel ||
            routerEvent instanceof NavigationError) {
            this.spinnerService.hideSpinner();
        }
    }
}
