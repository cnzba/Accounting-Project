import { Component, OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { ChangePasswordService } from './change-password.service';
import { AlertService } from '../alert/alert.service';
import { Subscription } from 'rxjs';
import { CallbackService } from '../common/callback.service';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnDestroy {
    model: any = {};
    confirmPasswordMatchError: boolean = false;
    oldPasswordMatch: boolean = false;
    loading = false;
    subscription: Subscription;

    constructor(private router: Router,
        private changePasswordService: ChangePasswordService,
        private alertService: AlertService,
        private callbackService: CallbackService) {
        this.subscription = callbackService.updateNavObs$.subscribe();
    }

    onSubmit() {
        if (this.model.newPassword === this.model.oldPassword && this.model.oldPassword === this.model.newPassword) {
            this.oldPasswordMatch = true;
            this.confirmPasswordMatchError = false;
            return;
        } else {
            this.oldPasswordMatch = false;
        }
        if (this.model.newPassword === this.model.confirmNewPassword && this.model.confirmNewPassword === this.model.newPassword) {
            this.confirmPasswordMatchError = false;
            this.loading = true;
            this.changePasswordService.changePassword(this.model.oldPassword, this.model.newPassword)
                .subscribe(
                data => {
                    this.loading = false;
                    this.alertService.success(data);
                    let forcePasswordChange: string = localStorage.getItem("forcePasswordChange");
                    if (forcePasswordChange) localStorage.removeItem("forcePasswordChange");
                    this.callbackService.updateNav(false);
                    this.router.navigate(["invoices"]);
                },
                error => {
                    this.alertService.error(error);
                    this.loading = false;
                });
        } else {
            this.confirmPasswordMatchError = true;
            return;
        }
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }
}
