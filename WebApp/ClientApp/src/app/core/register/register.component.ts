import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { UserRegisterService, } from "../services";
import { CBAUser } from '../domain/CBAUser';
import { Router } from '@angular/router';
import { CBAOrg } from '../domain/CBAOrg';
import { Country } from '../domain/Country';
import { Observable, observable } from 'rxjs';
import { flatMap,map} from "rxjs/operators";
import { UserValidators } from '../validators/user.validator';
import {  } from "@angular/material/core";
@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  selectedTab = 0;
  public displayInputGST = true;
  private regUser:CBAUser;
  private regOrg:CBAOrg;
  logoImgUrl ="/assets/images/logo_placeholder.png";
  selectedCountry = 'New Zealand';

  form:FormGroup;
  constructor(private fb:FormBuilder,
              private userRegService:UserRegisterService,
              private router: Router,
              private checkUserExistService:UserValidators){ 
      this.regUser = new CBAUser();
      this.regOrg = new CBAOrg();
    }

  ngOnInit() {
    this.form = this.fb.group({
      email: ['',
      Validators.compose([
        Validators.required,
        Validators.email,
        Validators.maxLength(50)
      ]),
      this.checkUserExistService.emailExistValidator()
      ],
      firstName:['',Validators.compose([
        Validators.required,   
        Validators.maxLength(50)   
      ])],
      lastName:['',Validators.compose([
        Validators.required, 
        Validators.maxLength(50)      
      ])],

      phoneNumberPrefix:['',Validators.compose([
        Validators.required, 
        Validators.pattern("[0-9]{3}")
      ])],

      phoneNumberBody:['',Validators.compose([
        Validators.required, 
        Validators.pattern("[0-9]{7,10}")
      ])],

      passwords:this.fb.group({
        password: ['',Validators.compose([
          Validators.required,
          Validators.minLength(8)        
        ])],
        confirmedPassword:['',Validators.compose([
          Validators.required,  
          Validators.minLength(8)      
        ])]
      },
      {validator: this.comparePasswords}
      ),
      
      orgName:['',Validators.compose([
        Validators.required, 
        Validators.maxLength(50)     
      ])],

      orgCode:['',Validators.compose([
        Validators.required,   
        Validators.pattern("[a-zA-Z]{4}")  
      ])],

      streetAddrL1:['',Validators.compose([
        Validators.required,      
      ])],

      streetAddrL2:['',Validators.compose([
              
      ])],

      city:['',Validators.compose([
        Validators.required,      
      ])],

      country:['',Validators.compose([
        Validators.required,      
      ])],

      orgPhoneNumberPrefix:['',Validators.compose([
        Validators.required, 
        Validators.pattern("[0-9]{3}")     
      ])],

      orgPhoneNumberBody:['',Validators.compose([
        Validators.required,   
        Validators.pattern("[0-9]{7,10}")   
      ])],

      logoUrl:['',Validators.compose([
      ])],

      charitiesNumber:['',Validators.compose([
        Validators.maxLength(20),      
      ])],

      gstNumber:['',Validators.compose([
        Validators.required,  
        Validators.maxLength(20)    
      ])],
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
    this.regUser.phoneNumber= value.phoneNumberPrefix+ "-"+value.phoneNumberBody;
    this.regUser.firstName= value.firstName;
    this.regUser.lastName = value.lastName;
    this.regUser.email = value.email;
    this.regUser.password = value.passwords.password;
    this.regOrg.OrgName= value.orgName;
    this.regOrg.orgCode= value.orgCode;
    this.regOrg.streetAddrL1 = value.streetAddrL1;
    this.regOrg.streetAddrL2 = value.streetAddrL2;
    this.regOrg.city = value.city;
    this.regOrg.country = this.selectedCountry;
    this.regOrg.phoneNumber = value.orgPhoneNumberPrefix + "-" + value.orgPhoneNumberBody;
    this.regOrg.logoUrl = value.logoUrl;
    this.regOrg.chritiesNumber = value.charitiesNumber;
    this.regOrg.gstNumber = value.gstNumber;
    this.userRegService.registerUser(this.regUser,this.regOrg).subscribe(
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

  nextTab(){
    this.selectedTab = 1;
  }

  prevTab(){
    this.selectedTab = 0;
  }

  onTabChange(index:number){
    this.selectedTab=index;
  }

  toggleGST(event){
    console.log(event);
    if (event.checked == true){
      this.displayInputGST=false;
    } else{
      this.displayInputGST=true;
    }
  }

  public uploadFinished =(event) =>{
    let url = event.dbPath;
    this.logoImgUrl= url;
    this.form.get('logoUrl').setValue(url);
  }

  // public createImgPath = () =>{
    
  //     return `https://localhost:44390/${this.logoImgUrl}`;
  // }

  
}
