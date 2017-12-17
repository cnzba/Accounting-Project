import { TestBed, async } from '@angular/core/testing';
import { AppComponent } from './app.component';
import { AlertComponent } from './alert/alert.component';
import { AlertService } from './alert/alert.service';
import { RouterTestingModule } from '@angular/router/testing';

xdescribe('AppComponent', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [AppComponent, AlertComponent],
            imports: [RouterTestingModule],
            providers: [AlertService]
        }).compileComponents();
    }));
    xit('should create the app', async(() => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app).toBeTruthy();
    }));
    xit(`should have as title 'CBA Invoicing'`, async(() => {
        const fixture = TestBed.createComponent(AppComponent);
        const app = fixture.debugElement.componentInstance;
        expect(app.title).toEqual('CBA Invoicing');
    }));
    xit('should render title in a h1 tag', async(() => {
        const fixture = TestBed.createComponent(AppComponent);
        fixture.detectChanges();
        const compiled = fixture.debugElement.nativeElement;
        expect(compiled.querySelector('h1').textContent).toContain('CBA Invoicing');
    }));
});
