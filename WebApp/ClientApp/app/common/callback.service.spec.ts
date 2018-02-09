import { TestBed, inject } from '@angular/core/testing';

import { CallbackService } from './callback.service';

describe('CallbackService', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CallbackService]
    });
  });

  it('should be created', inject([CallbackService], (service: CallbackService) => {
    expect(service).toBeTruthy();
  }));
});
