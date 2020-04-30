import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetPasswordService } from './reset-password.service';
import { AlertService } from '../common/alert/alert.service';
import { Subscription } from 'rxjs';
import { isNullOrUndefined } from 'util';
import { NgForm } from '@angular/forms';

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
    constructor(
        private resetPasswordService: ResetPasswordService,
        private alertService: AlertService,
        private route: ActivatedRoute,
        private router: Router) {

    }

    ngOnInit() {

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


    onSubmit() {

        //if (this.model.newPassword != this.model.confirmNewPassword) {
        //    this.confirmPasswordMatchError = true;
        //    return;
        //}

        this.resetPasswordService.changePassword(this.model.newPassword, this.model.confirmNewPassword, this.id, this.token)
            .subscribe(
                data => {
                    alert("success + " + data);
                    this.alertService.success("Password has been reset successfully..!!");
                    this.router.navigate(['login']);
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
        //form.resetForm();
        form.reset();
    }
}
