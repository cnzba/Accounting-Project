import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  model: any = {};
  confirmPasswordMatchError: boolean = false;
  oldPasswordMatch: boolean = false;

  constructor(private router: Router) { }

  ngOnInit() {
  }

  onSubmit() {
    if (this.model.newPassword === this.model.oldPassword && this.model.oldPassword === this.model.newPassword) {
      this.oldPasswordMatch = true;
      this.confirmPasswordMatchError = false;
      return;
    } else {
      this.oldPasswordMatch = false;
    }
    if (this.model.newPassword === this.model.confirmNewPassword && this.model.confirmNewPassword === this.model.newPassword) {
      // call backend
      this.confirmPasswordMatchError = false;
    } else {
      this.confirmPasswordMatchError = true;
      return;
    }
    this.router.navigate(['/login']);
  }
}