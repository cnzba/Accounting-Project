import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
//import { AuthenticationService } from "./authentication.service";
import { AlertService } from "../common/alert/alert.service";
import { CallbackService } from '../common/callback.service';
import { Subscription } from 'rxjs';
import { SpinnerService } from "../common/spinner.service";
import { NgForm } from '@angular/forms';
import { UserService } from '../users/user.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ForgotPasswordComponent } from './forgot-password.component';
//import { NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
    model: any = {};
    returnUrl: string;
    subscription: Subscription;
    modalRef: BsModalRef;
    email: string = "";
    isLoginFail: boolean;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        //private authenticationService: AuthenticationService,        
        private alertService: AlertService,
        private callbackService: CallbackService,
        private spinnerService: SpinnerService,
        private userService: UserService,
        private modalService: BsModalService) {
        this.subscription = callbackService.updateNavObs$.subscribe();
    }

    ngOnInit() {
        // reset login status
        //this.authenticationService.logout().subscribe();

        // get return url from route parameters or default to '/'
        if (this.route.snapshot.paramMap.has('returnUrl'))
            this.returnUrl = this.route.snapshot.paramMap.get('returnUrl');
        else this.returnUrl = '/';

        console.log(`LOGIN: After login will direct to ${this.returnUrl}`);
    }

    login() {
        this.spinnerService.showSpinner();
        this.userService.login(this.model).subscribe(
            (res:any) => {
                var token = JSON.parse(res._body).token;
                localStorage.setItem('token',token);
                this.router.navigateByUrl('/core');
            },
            err => {
                if(err.status == 400){
                    this.alertService.error("Incorrect username or password");                    
                    this.spinnerService.hideSpinner();
                }else{
                    console.log(err);
                }
            }


        );

        // this.authenticationService.login(this.model.username, this.model.password)
        //     .subscribe(
        //     (res: any) => {
        //         console.log(res.token);
        //         localStorage.setItem('token',res.token);
        //         this.router.navigateByUrl('/core');
        //         // this.isLoginFail = false;
        //         // if (data.forcePasswordChange) {
        //         //     localStorage.setItem("forcePasswordChange", "true");
        //         //     this.callbackService.updateNav(true);
        //         //     this.router.navigate(["change-password"]);
        //         // } else {
        //         //     this.router.navigate([this.returnUrl]);
        //         // }
        //     },
        //     error => {
        //         //this.alertService.error("User Not Found.");
        //         // this.isLoginFail = true;
        //         // this.spinnerService.hideSpinner();
        //         if (error.status == 400){
        //             this.alertService.error("Incorrect username or password");
        //         }else{
        //             console.log(error);
        //         }
        //     });
    }

    inputChanged(event) {
        this.isLoginFail = false;
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }


    openModal() {

        //let ngbModalOptions: NgbModalOptions = {
        //    backdrop: 'static',
        //    keyboard: false
        //};

        this.modalRef = this.modalService.show(ForgotPasswordComponent);
        this.modalRef.content.onClose.subscribe(result => {
            console.log('results', result);
        })
    }

}
