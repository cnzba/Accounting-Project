import { Component, OnInit, OnDestroy, HostListener } from '@angular/core';
import { IInvoice } from '../invoices/invoice';
import { InvoiceService } from '../invoices/invoice.service';
import { ActivatedRoute, ParamMap } from '@angular/router';
import { AlertService } from '../alert/alert.service';
import { Subscription } from 'rxjs';
import { CallbackService } from '../common/callback.service';
import { environment } from '../../environments/environment';
import { InvoicePaymentService } from './invoice-payment.service';

@Component({
  selector: 'app-invoice-payment',
  templateUrl: './invoice-payment.component.html',
  styleUrls: ['./invoice-payment.component.css']
})
export class InvoicePaymentComponent implements OnInit, OnDestroy {
    invoice: IInvoice;
    message: string = "Verifying invoice details";
    subscription: Subscription;
    amount: number;
    handler: any;
    disablePay: boolean = false;

    constructor(
        private invoiceService: InvoiceService,
        private route: ActivatedRoute,
        private alertService: AlertService,
        private callbackService: CallbackService,
        private paymentService: InvoicePaymentService
    ) {
        this.subscription = callbackService.paymentNavObs$.subscribe();
    }

    ngOnInit() {
        this.callbackService.paymentNav(true);
        this.route.paramMap
            .switchMap((params: ParamMap) => this.invoiceService.getInvoiceByPaymentId(params.get('id')))
            .subscribe(
            invoice => {
                this.invoice = invoice;
                this.callbackService.paymentNav(false);
                this.verifyInvoice();
            }, error => {
                this.callbackService.paymentNav(false);
                this.message = null;
                this.alertService.error("Invalid invoice number");
            });
        this.handler = (<any>window).StripeCheckout.configure({
            key: environment.stripeKey,
            locale: 'auto',
            token: (token: any) => this.pay(token)
        });
    }

    verifyInvoice(): void {
        if (!this.invoice) {
            this.message = null;
            this.alertService.error("Invalid invoice number");
        } else if (this.invoice.status === "Sent") {
            this.message = "Valid invoice";
            this.amount = this.invoice.grandTotal * 100;
        } else if (this.invoice.status === "Paid" || this.invoice.status === "Cancelled") {
            this.message = null;
            this.alertService.error("Invoice has already been paid");
        } else if (this.invoice.status === "Draft" || this.invoice.status === "New") {
            this.message = null;
            this.alertService.error("Invoice has not been sent");
        } else {
            this.message = null;
            this.alertService.error("Invalid invoice number");
        }
    }

    getToken() {
        this.handler.open({
            name: 'CNZBA',
            description: 'Invoice No - ' + this.invoice.invoiceNumber,
            zipCode: false,
            currency: 'nzd',
            amount: this.amount,
            panelLabel: "Pay {{amount}}",
            allowRememberMe: false
        });
    }

    pay(token: any) {
        let body = {
            TokenId: token.id,
            PaymentId: this.invoice.paymentId,
            TokenObj: JSON.stringify(token),
            Gateway: "stripe",
            Type: "card"
        };
        this.disablePay = true;
        console.log("token===", body);
        this.callbackService.paymentNav(true);
        this.message = "Processing payment....";
        this.paymentService.chargeCard(body)
                .subscribe(
                data => {
                    this.callbackService.paymentNav(false);
                    this.message = null;
                    console.log("pgresp===", data);
                    var response = JSON.parse(data);
                    if (response.status === "succeeded") {
                        this.alertService.success(response.message);
                    } else {
                        this.alertService.error(response.message);
                    }
                },
                error => {
                    this.disablePay = false;
                    this.message = null;
                    console.log("pgerr===", error);
                    this.alertService.error(error);
                    this.callbackService.paymentNav(false);
                });
    }

    @HostListener('window:popstate')
    onPopstate() {
        this.handler.close();
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }
}
