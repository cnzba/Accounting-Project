import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from "@angular/common/http";
import { AlertComponent } from './common/alert/alert.component';
import { AlertService } from "./common/alert/alert.service";
import { LoginComponent } from './login/login.component';
import { AuthGuard } from "./login/auth.guard";
import { AuthenticationService } from "./login/authentication.service";
import { UserService } from "./users/user.service";
import { HttpModule } from "@angular/http";
import { InvoicelistComponent } from "./invoices/invoicelist.component";
import { InvoicedetailComponent } from "./invoices/invoicedetail.component";
import { InvoiceEditComponent } from "./invoices/invoice-edit.component";
import { InvoiceService } from "./invoices/invoice.service";
import { InvoiceListResolver } from "./invoices/invoicelist-resolver.service";
import { ForgotPasswordComponent } from './login/forgot-password.component';
import { ChangePasswordComponent } from './login/change-password.component';
import { ForgotPasswordService } from './login/forgot-password.service';
import { ChangePasswordService } from './login/change-password.service';
import { InvoicePaymentComponent } from './payment/invoice-payment.component';
import { InvoicePaymentService } from './payment/invoice-payment.service';
import { PaginationComponent } from "./pagination/pagination.component";
import { HttpErrorInterceptor } from "./common/error.service";
import { PageNotFoundComponent } from './pagenotfound/pagenotfound.component';
import { InvoiceResolverService } from "./invoices/invoice-resolver.service";
import { SpinnerService } from "./common/spinner.service";
import { environment } from '../environments/environment';
import { ResetPasswordComponent } from './login/reset-password.component';

import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSortModule } from '@angular/material/sort';
import { ModalModule } from 'ngx-bootstrap';

import { InvoiceFilterPipe } from './pipes/invoice-filter.pipe';

import { TwoDigitDecimaNumberDirective } from './invoices/two-digit-decima-number.directive';
import { InputIntegerOnlyDirective } from './invoices/input-integer-only.directive';

import { DatePipe } from '@angular/common';
import { AppRoutingModule } from './app-routing.module';
import { SharedModule } from './shared';
import { CoreModule } from './core';
import { AuthInterceptor } from './login/auth.interceptor';

@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        LoginComponent,
        InvoicelistComponent,
        InvoicedetailComponent,
        InvoiceEditComponent,
        ForgotPasswordComponent,
        ChangePasswordComponent,
        ResetPasswordComponent,
        InvoicePaymentComponent,
        PaginationComponent,
        PageNotFoundComponent,
        InvoiceFilterPipe,
        TwoDigitDecimaNumberDirective,
        InputIntegerOnlyDirective
    ],
    imports: [
        BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
        BrowserAnimationsModule,
        MatSortModule,
        AngularFontAwesomeModule,
        HttpClientModule,
        FormsModule,
        ReactiveFormsModule,
        HttpModule,
        ModalModule.forRoot(),
        AppRoutingModule,
        SharedModule,
        CoreModule
    ],
    providers: [
        DatePipe,
        InvoiceService,
        InvoiceListResolver,
        InvoiceResolverService,
        AuthGuard,
        AlertService,
        AuthenticationService,
        UserService,
        ForgotPasswordService,
        ChangePasswordService,
        ResetPasswordComponent,
        InvoicePaymentService,
        SpinnerService,
        { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true },
        { provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
