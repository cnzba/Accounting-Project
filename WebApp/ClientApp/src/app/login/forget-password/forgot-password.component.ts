import { Component, OnInit, TemplateRef } from '@angular/core';
import { ForgotPasswordService } from './forgot-password.service';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Subject } from 'rxjs';

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

    config = {
        backdrop: true,
        ignoreBackdropClick: true
    };

    public onClose: Subject<boolean>;

    constructor(
        private forgotPasswordService: ForgotPasswordService,
        public bsModalRef: BsModalRef,
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
                    this.modalRef = this.modalService.show(template, this.config);
                    this.emailSent = true;
                    this.loading = false;
                },
                error => {
                    // if (!isNullOrUndefined(error) && error.httpError.status == 400) {
                    let errorMessage = error.globalError;
                    this.model.title = "Error";
                    this.model.message = errorMessage;
                    this.modalRef = this.modalService.show(template, this.config);
                    //}
                    this.loading = false;
                });
    }

    cancel(e) {
        e.preventDefault();
        this.bsModalRef.hide()
    }

    cancelChildModal() {
        this.modalRef.hide();
        if (this.model.title != "Error")
            this.bsModalRef.hide()
    }
}
