import { InvoiceService } from './invoice.service';
import { IInvoice } from "./invoice";
import { TestBed, async, inject } from '@angular/core/testing';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpRequest } from "@angular/common/http";
import { HttpParams } from "@angular/common/http";

describe('Invoice service (list of invoices)', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                InvoiceService
            ],
            imports: [
                HttpClientModule,
                HttpClientTestingModule
            ]
        });
    });

    afterEach(inject([HttpTestingController], (backend: HttpTestingController) => {
        backend.verify();
    }));

    const dummyInvoices = `[{"subTotal":150.0,"grandTotal":150.0,"id":1,"invoiceNumber":"112234","date":"2017-11-21T00:00:00","issueeOrganization":"OrgOne","issueeCareOf":"John Doe","gstnumber":123123,"charitiesNumber":546546,"clientContact":"297 Moorhouse Ave\\r\\nSydenham\\r\\nChristchurch 8011","gst":0.0,"dueDate":"2017-11-28T00:00:00","statusId":1,"status":null,"invoiceLine":[{"id":2,"invoiceId":1,"description":"helped with website","amount":50.0},{"id":3,"invoiceId":1,"description":"Marketing","amount":100.0}]},{"subTotal":510.0,"grandTotal":510.0,"id":2,"invoiceNumber":"32323","date":"1991-10-18T00:00:00","issueeOrganization":"OrgTwo","issueeCareOf":"Jane Doe","gstnumber":45,"charitiesNumber":29,"clientContact":"530/546A Memorial Ave\\r\\nChristchurch Airport\\r\\nChristchurch 8053","gst":0.0,"dueDate":"2017-10-25T00:00:00","statusId":2,"status":null,"invoiceLine":[{"id":4,"invoiceId":2,"description":"Fundraising","amount":10.0},{"id":5,"invoiceId":2,"description":"Legal Advice","amount":500.0}]}]`;

    it("sub+gst=total",
        // the async function wrapper - as distinct from async keyword -
        // serialises the observer returned by getInvoices
        async(inject([InvoiceService, HttpClient, HttpTestingController], (invoiceService: InvoiceService, http: HttpClient, backend: HttpTestingController) => {
            invoiceService.getInvoices().subscribe(
                (invoiceList: IInvoice[]) => {
                    expect(invoiceList[0].grandTotal).toBe(invoiceList[0].subTotal+invoiceList[0].gst);
                });

            const req = backend.expectOne('api/invoice');
            let invoices: IInvoice[] = JSON.parse(dummyInvoices);
            req.flush(invoices);
        })));

    it("Should have 2 invoices",
        // the async function wrapper - as distinct from async keyword -
        // serialises the observer returned by getInvoices
        async(inject([InvoiceService, HttpClient, HttpTestingController], (invoiceService: InvoiceService, http: HttpClient, backend: HttpTestingController) => {
            expect(invoiceService).toBeDefined();

            invoiceService.getInvoices().subscribe(
                (invoiceList: IInvoice[]) => {
                    expect(invoiceList.length).toBe(2);
                    expect(invoiceList[0].id).toBe(1);
                    expect(invoiceList[0].invoiceLine.length).toBe(2);
                    expect(invoiceList[1].id).toBe(2);
                });

            const req = backend.expectOne('api/invoice');
            let invoices: IInvoice[] = JSON.parse(dummyInvoices);
            req.flush(invoices);
        })))

    
});



