import { Component, OnInit, TemplateRef } from '@angular/core';
import { NgForm } from '@angular/forms/src/directives/ng_form';
import { ForgotPasswordService } from './forgot-password.service';
import { AlertService } from '../common/alert/alert.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';
import { isNullOrUndefined } from 'util';


declare var $: any;

@Component({
    selector: 'app-forgot-password',
    templateUrl: './forgot-password.component.html',
    styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
    model: any = {};
    emailSent: boolean = false;
    loading: boolean = false;
    public id: number;
    modalRef: BsModalRef;

    public onClose: Subject<boolean>;

    constructor(
        private forgotPasswordService: ForgotPasswordService,
        public bsModalRef: BsModalRef,
        private alertService: AlertService,
        private modalService: BsModalService) {
    }

    ngOnInit() {
        this.onClose = new Subject();
    }


    //template: TemplateRef<successTemplate>
    onSubmit(template: TemplateRef<'successTemplate'>) {
        this.loading = true;
        this.forgotPasswordService.sendEmail(this.model.email)
            .subscribe(
                data => {

                    this.model.message = data;//"An email has been sent with instruction to reset your password";
                    this.model.title = "Password Reset Email Sent";
                    this.modalRef = this.modalService.show(template);
                    this.loading = false;
                },
                error => {
                    alert("error12");
                    if (!isNullOrUndefined(error)) {
                        if (error.status == 400) {
                            alert("inside");
                            this.model.title = "Error";
                            this.model.message = error.message;
                            this.modalRef = this.modalService.show(template);
                        }
                    }
                    this.loading = false;
                });
    }
}
