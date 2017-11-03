import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterModule } from '@angular/router'; 
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
import { routing } from "./app.routing";
import { fakeBackendProvider } from "./login/mockauthentication-backend";
import { MockBackend } from "@angular/http/testing";
import { BaseRequestOptions, HttpModule } from "@angular/http";


@NgModule({
    declarations: [
        AppComponent,
        AlertComponent,
        LoginComponent,
        LogintestComponent,
        RegisterComponent,
        InvoicelistComponent,
        InvoicedetailComponent
    ],
    imports: [
        BrowserModule,
        HttpClientModule,
        FormsModule,
        HttpModule,
        RouterModule.forRoot([
            {
                path: "",
                component: InvoicelistComponent
            },
            { path: "invoices/:id", component: InvoicedetailComponent },

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
