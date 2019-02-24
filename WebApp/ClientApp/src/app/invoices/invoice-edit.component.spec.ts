import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { InvoiceEditComponent } from './invoice-edit.component';

xdescribe('InvoiceEditComponent', () => {
  let component: InvoiceEditComponent;
  let fixture: ComponentFixture<InvoiceEditComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ InvoiceEditComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvoiceEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  xit('should create', () => {
    expect(component).toBeTruthy();
  });
});
