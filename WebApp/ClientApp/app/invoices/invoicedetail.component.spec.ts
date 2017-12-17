import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { InvoiceService } from './invoice.service';
import { InvoicedetailComponent } from './invoicedetail.component';

xdescribe('InvoicedetailComponent', () => {
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

    xit('should create', () => {
        expect(component).toBeTruthy();
    });
});
