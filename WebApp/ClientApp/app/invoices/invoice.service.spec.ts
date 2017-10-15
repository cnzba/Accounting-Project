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

    it("Should have 2 invoices",
        async(inject([InvoiceService], (invoiceService) => {
            expect(invoiceService).toBeDefined();

            invoiceService.getInvoices().subscribe(
                (invoiceList) => {
                    expect(invoiceList.length).toBe(2);
                    expect(invoiceList[0].id).toBe(1);
                    expect(invoiceList[1].id).toBe(2);
                });
        })));
});
