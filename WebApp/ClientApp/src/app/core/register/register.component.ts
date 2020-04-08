import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl,Validators, FormBuilder } from '@angular/forms';
import { UserRegisterService } from "../services";
import { CBAUser } from '../domain/CBAUser';
import { Router } from '@angular/router';
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  private regUser:CBAUser;

  form:FormGroup;
  constructor(private fb:FormBuilder,
              private userRegService:UserRegisterService,
              private router: Router){ 
      this.regUser = new CBAUser();
    }

  ngOnInit() {
    this.form = this.fb.group({
      email: ['',Validators.compose([
        Validators.required,
        Validators.email,
      ])],
      firstName:['',Validators.compose([
        Validators.required,      
      ])],
      lastName:['',Validators.compose([
        Validators.required,  
     
      ])],

      phoneNumber:['',Validators.compose([
        Validators.required, 
     
      ])],

      passwords:this.fb.group({
        password: ['',Validators.compose([
          Validators.required,        
        ])],
        confirmedPassword:['',Validators.compose([
          Validators.required,  
          Validators.minLength(5)      
        ])]
      },
      {validator: this.comparePasswords}
      )      
    })
    
  }

  comparePasswords(fb:FormGroup){
    let confirmPswrdCtrl = fb.get('confirmedPassword');
    if (confirmPswrdCtrl.errors == null || 'passwordMismatch' in confirmPswrdCtrl.errors){
      if (fb.get('password').value != confirmPswrdCtrl.value){
        confirmPswrdCtrl.setErrors({passwordMismatch:true});
        console.log(confirmPswrdCtrl.errors);
      }        
      else
        confirmPswrdCtrl.setErrors(null);
    }
  }

  onSubmit({value, valid},ev: Event){
    ev.preventDefault();
    console.log(JSON.stringify(value));
    console.log(valid);
    this.regUser.phoneNumber= value.phoneNumber;
    this.regUser.firstName= value.firstName;
    this.regUser.lastName = value.lastName;
    this.regUser.email = value.email;
    this.regUser.password = value.passwords.password;

    this.userRegService.registerUser(this.regUser).subscribe(
      (res:any) =>{
        if (res.succeeded){
          //TODO: Tips the user that the regester succeed.
          this.router.navigate(['/login'])
        }else{
          //TODO: Tips the errors to user.
        }
      },
      err => {
        console.log(err);
      }
    );
  }
}
