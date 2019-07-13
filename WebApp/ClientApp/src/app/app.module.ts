import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
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

import { AngularFontAwesomeModule } from 'angular-font-awesome';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatSortModule } from '@angular/material/sort';

import { InvoiceFilterPipe } from './pipes/invoice-filter.pipe';

import { TwoDigitDecimaNumberDirective } from './invoices/two-digit-decima-number.directive';
import { InputIntegerOnlyDirective } from './invoices/input-integer-only.directive';


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
        HttpModule,
        RouterModule.forRoot([
            { path: 'login', component: LoginComponent },
            {
                path: 'invoices', component: InvoicelistComponent, canActivate: [AuthGuard],
                resolve: { invoices: InvoiceListResolver }
            },
            { path: "invoices/:id", component: InvoicedetailComponent, canActivate: [AuthGuard] },
            {
                path: "invoices/edit/:id", component: InvoiceEditComponent, canActivate: [AuthGuard],
                resolve: { invoice: InvoiceResolverService }
            },
            {
                path: "invoice/new", component: InvoiceEditComponent,
                resolve: { invoice: InvoiceResolverService }
            },
            { path: "forgot-password", component: ForgotPasswordComponent },
            { path: "change-password", component: ChangePasswordComponent, canActivate: [AuthGuard] },
            { path: "pay/:id", component: InvoicePaymentComponent },
            // otherwise redirect to the invoice list
            { path: "", redirectTo: '/invoices', pathMatch: 'full' },
            { path: '**', component: PageNotFoundComponent }
        ], { enableTracing: false })
    ],
    providers: [
        InvoiceService,
        InvoiceListResolver,
        InvoiceResolverService,
        AuthGuard,
        AlertService,
        AuthenticationService,
        UserService,
        ForgotPasswordService,
        ChangePasswordService,
        InvoicePaymentService,
        SpinnerService,
        { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptor, multi: true }
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
