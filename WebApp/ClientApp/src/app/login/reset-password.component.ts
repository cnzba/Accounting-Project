import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ResetPasswordService } from './reset-password.service';
import { AlertService } from '../common/alert/alert.service';
import { Subscription } from 'rxjs';
import { isNullOrUndefined } from 'util';

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
                },
                error => {
                    alert("error");
                    if (!isNullOrUndefined(error)) {
                        if (error.statuscode == 400) {
                                                    
                            //this.modalRef = this.modalService.show(template);
                            this.alertService.error(error.message);
                        }
                    }
                });
    }


    onSubmit() {

        this.resetPasswordService.changePassword(this.model.newPassword, this.model.confirmNewPassword, this.id, this.token)
            .subscribe(
                data => {
                    alert("success + " + data);
                    this.alertService.success("Password updated ");

                    this.router.navigate(['login']);
                    this.loading = false;
                },
                error => {
                    if (error.status == 400) {
                        alert("error");
                    }
                    this.loading = false;
                });
    }

    reset() {
        this.model.f.reset();
        //this.formGroup.clearValidators();
    }
}
