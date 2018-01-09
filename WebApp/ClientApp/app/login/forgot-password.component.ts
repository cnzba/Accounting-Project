import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms/src/directives/ng_form';

@Component({
  selector: 'app-forgot-password',
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css']
})
export class ForgotPasswordComponent implements OnInit {
  model: any = {};
  emailSent: boolean = false;

  constructor() { }

  ngOnInit() {
  }

  onSubmit() {
    // call backend and send email
    this.emailSent = true;
  }
}