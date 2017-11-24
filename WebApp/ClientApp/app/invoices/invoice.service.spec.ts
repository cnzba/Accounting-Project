import { InvoiceService } from './invoice.service';
import { IInvoice } from "./invoice";
import { TestBed, async, inject } from '@angular/core/testing';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpRequest } from "@angular/common/http";
import { HttpParams } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/of';

describe('Invoice service (list of invoices)', () => {
    var service : InvoiceService;
    var http;
    var invoice : IInvoice;

    beforeEach(() => {
        invoice = JSON.parse('{ "invoiceNumber": "INV-00052", "issueeOrganization": "Transtellar", "issueeCareOf": null, "clientContact": "52 Solmine Ave\\r\\nRiccarton\\r\\nChristchurch 8025", "dateDue": "2017-09-30T00:00:00", "status": "Paid", "dateCreated": "2017-09-09T00:00:00", "gstNumber": "96-712-561", "charitiesNumber": "CC20097", "gstRate": 0.15, "invoiceLine": [{ "itemOrder": 0, "description": "Fundraising Dinner", "amount": 21.74 }, { "itemOrder": 0, "description": "Bookkeeping 2 hours @21.74 per hour", "amount": 43.48 }], "subTotal": 65.22, "grandTotal": 75.0030 }');

        var res = Observable.of(invoice);
        
        http = jasmine.createSpyObj('http', {
            'get': res,
            'put': res,
            'post': res
        });

        service = new InvoiceService(http);
    });

    it("getInvoice should provide the invoice number to HttpClient.get", async(() => {
        service.getInvoice(invoice.invoiceNumber).subscribe();
        expect(http.get.calls.count()).toEqual(1);
        expect(http.get.calls.argsFor(0).toString().search(invoice.invoiceNumber) != -1).toEqual(true);
    }));

    it("updateInvoice should provide the invoice number to HttpClient.put", async(() => {
        service.updateInvoice(invoice).subscribe();
        expect(http.put.calls.count()).toEqual(1);
        expect((http.put.calls.argsFor(0).toString()).search(invoice.invoiceNumber) != -1).toEqual(true);
    }));

});



