import { TestBed, inject } from '@angular/core/testing';

import { InvoicePaymentService } from './invoice-payment.service';

describe('InvoicePaymentService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [InvoicePaymentService]
    });
  });

  it('should be created', inject([InvoicePaymentService], (service: InvoicePaymentService) => {
    expect(service).toBeTruthy();
  }));
});
