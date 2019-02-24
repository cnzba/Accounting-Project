import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
import { AuthenticationService } from "./authentication.service";
import { AlertService } from "../common/alert/alert.service";
import { CallbackService } from '../common/callback.service';
import { Subscription } from 'rxjs';
import { SpinnerService } from "../common/spinner.service";

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
    model: any = {};
    returnUrl: string;
    subscription: Subscription;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private alertService: AlertService,
        private callbackService: CallbackService,
        private spinnerService: SpinnerService) {
        this.subscription = callbackService.updateNavObs$.subscribe();
    }

    ngOnInit() {
        // reset login status
        this.authenticationService.logout().subscribe();

        // get return url from route parameters or default to '/'
        if (this.route.snapshot.paramMap.has('returnUrl'))
            this.returnUrl = this.route.snapshot.paramMap.get('returnUrl');
        else this.returnUrl = '/';

        console.log(`LOGIN: After login will direct to ${this.returnUrl}`);
    }

    login() {
        this.spinnerService.showSpinner();
        this.authenticationService.login(this.model.username, this.model.password)
            .subscribe(
            data => {
                if (data.forcePasswordChange) {
                    localStorage.setItem("forcePasswordChange", "true");
                    this.callbackService.updateNav(true);
                    this.router.navigate(["change-password"]);
                } else {
                    this.router.navigate([this.returnUrl]);
                }
            },
            error => {
                this.alertService.error("Login failed.");
                this.spinnerService.hideSpinner();
            });
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }
}