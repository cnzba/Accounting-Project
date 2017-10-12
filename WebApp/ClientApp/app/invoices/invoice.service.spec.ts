import { InvoiceService } from './invoice.service';
import { TestBed, inject, async } from '@angular/core/testing';
import { HttpClientModule } from "@angular/common/http";

describe('Invoice service (list of invoices)', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [
                InvoiceService
            ],
            imports: [
                HttpClientModule
            ]
        });
    });

    it("Should have 3 invoices",
        async(inject([InvoiceService], (invoiceService) => {
            expect(invoiceService).toBeDefined();

            invoiceService.getInvoices().subscribe(
                (invoiceList) => {
                    expect(invoiceList.length).toBe(3);
                    expect(invoiceList[0].invoiceId).toBe(1);
                });
        })));
});
