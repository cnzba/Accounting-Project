import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { LogintestComponent } from './logintest.component';

describe('LogintestComponent', () => {
  let component: LogintestComponent;
  let fixture: ComponentFixture<LogintestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LogintestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LogintestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
