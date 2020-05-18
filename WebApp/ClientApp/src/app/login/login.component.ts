import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from "@angular/router";
//import { AuthenticationService } from "./authentication.service";
import { AlertService } from "../common/alert/alert.service";
import { CallbackService } from '../common/callback.service';
import { Subscription } from 'rxjs';
import { SpinnerService } from "../common/spinner.service";
import { NgForm, FormBuilder } from '@angular/forms';
import { UserService } from '../users/user.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { ForgotPasswordComponent } from './forget-password/forgot-password.component';
//import { NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
    selector: 'app-login',
    templateUrl: './login.component.html',
    styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit, OnDestroy {
    model: any = {username:"",password:""};
    returnUrl: string;
    subscription: Subscription;
    modalRef: BsModalRef;
    email: string = "";
    isLoginFail: boolean;
    userLogin:FormGroup;    

    config = {
        backdrop: true,
        ignoreBackdropClick: true
    };

    constructor(
        private fb:FormBuilder,
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
        this.userLogin = this.fb.group({
            username: new FormControl('', [
                Validators.required,
                Validators.pattern("[A-Za-z0-9._%-]+@[A-Za-z0-9._%-]+\\.[a-z]{2,3}")]),
            password: new FormControl('', [
                Validators.required])
        }); 

    }

    OnSubmit({value,valid}, ev:Event) {
        ev.preventDefault();        
        this.spinnerService.showSpinner();
        this.model.username = value.username;
        this.model.password = value.password;
        this.userService.login(this.model).subscribe(
            (res:any) => {
                var token = JSON.parse(res._body).token;
                localStorage.setItem('token',token);
                this.router.navigateByUrl('/core');
                //localStorage.setItem('userName', )
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

    }

    inputChanged(event) {
        this.isLoginFail = false;
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }


    openModal() {

        this.modalRef = this.modalService.show(ForgotPasswordComponent, this.config);
        this.modalRef.content.onClose.subscribe(result => {
           
        })
    }


    get getUsername() {
        return this.userLogin.get('username')
    }

    get getPassword() {
        return this.userLogin.get('password')
    }
}
