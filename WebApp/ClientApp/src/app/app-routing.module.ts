import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { InvoicelistComponent } from './invoices/invoicelist.component';
import { AuthGuard } from './login/auth.guard';
import { InvoiceListResolver } from './invoices/invoicelist-resolver.service';
import { InvoicedetailComponent } from './invoices/invoicedetail.component';
import { InvoiceEditComponent } from './invoices/invoice-edit.component';
import { InvoiceResolverService } from './invoices/invoice-resolver.service';
import { ChangePasswordComponent } from './login/change-password.component';
import { ForgotPasswordComponent } from './login/forgot-password.component';
import { InvoicePaymentComponent } from './payment/invoice-payment.component';
import { PageNotFoundComponent } from './pagenotfound/pagenotfound.component';
import { ResetPasswordComponent } from './login/reset-password.component';

const routes: Routes = [
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
    { path: "reset-password", component: ResetPasswordComponent },
    { path: "pay/:id", component: InvoicePaymentComponent },
    //{ path: 'core', redirectTo:'core', pathMatch:'full'},
    // otherwise redirect to the invoice list
    { path: '', redirectTo:'core', pathMatch:'full' },
    //{ path: '**', component: PageNotFoundComponent }
];

@NgModule({
    imports: [RouterModule.forRoot(routes)],
    exports: [RouterModule]
})
export class AppRoutingModule {}

