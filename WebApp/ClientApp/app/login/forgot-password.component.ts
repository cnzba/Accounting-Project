import { Component } from '@angular/core';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { ForgotPasswordService } from './forgot-password.service';
import { AlertService } from '../alert/alert.service';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent {
    model: any = {};
    emailSent: boolean = false;
    loading: boolean = false;

    constructor(
        private forgotPasswordService: ForgotPasswordService,
        private alertService: AlertService) { }

    onSubmit() {
        this.loading = true;
        this.forgotPasswordService.sendEmail(this.model.email)
            .subscribe(
            data => {
                this.loading = false;
                this.alertService.success(data);
                this.emailSent = true;
            },
            error => {
                this.alertService.error(error);
                this.loading = false;
            });
    }
}
