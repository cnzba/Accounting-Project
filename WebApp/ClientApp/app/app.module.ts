import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router';
import { NgForm } from '@angular/forms';
import 'rxjs/add/operator/map';

import { AppComponent } from './app.component';
import { HttpClientModule } from "@angular/common/http";
import { AlertComponent } from './alert/alert.component';
import { AlertService } from "./alert/alert.service";
import { LoginComponent } from './login/login.component';
import { LogintestComponent } from './logintest/logintest.component';
import { RegisterComponent } from './logintest/register.component';
import { AuthGuard } from "./login/auth.guard";
import { AuthenticationService } from "./login/authentication.service";
import { UserService } from "./users/user.service";
import { fakeBackendProvider } from "./login/mockauthentication-backend";
import { MockBackend } from "@angular/http/testing";
import { BaseRequestOptions, HttpModule } from "@angular/http";
import { InvoicelistComponent } from "./invoices/invoicelist.component";
import { InvoicedetailComponent } from "./invoices/invoicedetail.component";
import { InvoiceEditComponent } from "./invoices/invoice-edit.component";
import { InvoiceService } from "./invoices/invoice.service";


@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        LoginComponent,
        LogintestComponent,
        RegisterComponent,
        InvoicelistComponent,
        InvoicedetailComponent,
        InvoiceEditComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        FormsModule,
        HttpModule,
        RouterModule.forRoot([
            { path: 'login', component: LoginComponent },
            { path: 'invoices', component: InvoicelistComponent, canActivate: [AuthGuard] },
            { path: "invoices/:id", component: InvoicedetailComponent, canActivate: [AuthGuard] },
            { path: "invoice/edit/:id", component: InvoiceEditComponent },
            { path: "invoice/new", component: InvoiceEditComponent },
            { path: "invoices", component: InvoicelistComponent, canActivate: [AuthGuard] },
            { path: "invoices", component: InvoicelistComponent, canActivate: [AuthGuard] }
            // otherwise redirect to the invoice list
          //  { path: '**', redirectTo: 'invoices' }            
        ])
    ],
    providers: [
        InvoiceService,
        AuthGuard,
        AlertService,
        AuthenticationService,
        UserService,
        // providers used to create fake backend
        fakeBackendProvider,
        MockBackend,
        BaseRequestOptions
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }
