import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { InvoiceService } from './invoice.service';
import { InvoicelistComponent } from './invoicelist.component';
import { RouterTestingModule } from '@angular/router/testing';

describe('InvoicelistComponent', () => {
  let component: InvoicelistComponent;
  let fixture: ComponentFixture<InvoicelistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
        declarations: [InvoicelistComponent],
         providers: [InvoiceService],
        imports: [RouterTestingModule]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(InvoicelistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
