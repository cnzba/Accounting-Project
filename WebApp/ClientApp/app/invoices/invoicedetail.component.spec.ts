import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { InvoiceService } from './invoice.service';
import { InvoicedetailComponent } from './invoicedetail.component';

describe('InvoicedetailComponent', () => {
    let component: InvoicedetailComponent;
    let fixture: ComponentFixture<InvoicedetailComponent>;

    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [InvoicedetailComponent],
            providers: [InvoiceService]
        })
            .compileComponents();
    }));

    beforeEach(() => {
        fixture = TestBed.createComponent(InvoicedetailComponent);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
