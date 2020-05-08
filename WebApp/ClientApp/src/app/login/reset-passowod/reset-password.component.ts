import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetPasswordService } from './reset-password.service';
import { AlertService } from '../../common/alert/alert.service';
import { Subscription } from 'rxjs';
import { isNullOrUndefined } from 'util';
import { NgForm } from '@angular/forms';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';

@Component({
    selector: 'app-reset-password',
    templateUrl: './reset-password.component.html',
    styleUrls: ['./reset-password.component.css']
})
export class ResetPasswordComponent implements OnInit {
    model: any = {};
    confirmPasswordMatchError: boolean = false;
    oldPasswordMatch: boolean = false;
    loading = false;
    subscription: Subscription;
    id: number = 0;
    token: string;
    isError: boolean = false;
    emailSent: boolean = false;

    resetPasswordForm: FormGroup;

    constructor(
        private resetPasswordService: ResetPasswordService,
        private alertService: AlertService,
        private route: ActivatedRoute, private formBuilder: FormBuilder) {

    }

    ngOnInit() {

        this.resetPasswordForm = this.formBuilder.group({

            passwords: this.formBuilder.group({
                newPassword: ['', Validators.compose([
                    Validators.required,
                    Validators.minLength(8),
                    Validators.pattern(".*[0-9].*")
                ])],
                confirmPassword: ['', Validators.compose([
                    Validators.required,
                    Validators.minLength(8),
                    Validators.pattern(".*[0-9].*")
                ])]
            },
                { validator: this.comparePasswords }
            ),
        });

        //Retrieve params from URL
        this.id = this.route.snapshot.queryParams['id'];
        this.token = this.route.snapshot.queryParams['token'];

        this.resetPasswordService.verifyToken(this.id, this.token)
            .subscribe(
                data => {
                    this.isError = false;
                },
                error => {

                    this.isError = true;
                    if (!isNullOrUndefined(error) && error.httpError.status != 400) {

                        this.alertService.error(error.globalError);
                    }
                });
    }

    comparePasswords(fb: FormGroup) {
        let confirmPswrdCtrl = fb.get('confirmPassword');
        if (confirmPswrdCtrl != null && (confirmPswrdCtrl.errors == null || 'passwordMismatch' in confirmPswrdCtrl.errors)) {
            if (fb.get('newPassword').value != confirmPswrdCtrl.value) {
                confirmPswrdCtrl.setErrors({ passwordMismatch: true });
            }
            else {
                confirmPswrdCtrl.setErrors(null);
            }
        }


    }
    onSubmit({ value, valid }) {

        this.resetPasswordService.changePassword(value.passwords.newPassword, value.passwords.confirmPassword, this.id, this.token)
            .subscribe(
                data => {
                    //this.alertService.success("Password has been reset successfully..!!");
                    this.model.isSuccess = true;
                    this.loading = false;
                },
                error => {
                    //if (!isNullOrUndefined(error) && error.httpError.status == 400) {
                    this.alertService.error(error.globalError);
                    // }
                    this.loading = false;
                });
    }

    reset(form: NgForm) {
        form.reset();
    }
}
