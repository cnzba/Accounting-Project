/* tslint:disable:no-unused-variable */
import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { GstInputComponent } from './gst-input.component';

describe('GstInputComponent', () => {
  let component: GstInputComponent;
  let fixture: ComponentFixture<GstInputComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ GstInputComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GstInputComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
