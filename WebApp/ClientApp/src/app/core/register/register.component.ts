import { Component, OnInit, OnDestroy } from '@angular/core';
import { FormGroup, FormControl,Validators, FormBuilder } from '@angular/forms';
import { UserRegisterService } from "../services";
import { CBAUser } from '../domain/CBAUser';
import { Router } from '@angular/router';
import { CBAOrg } from '../domain/CBAOrg';
import { Country } from '../domain/Country';
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
  countries : Country[] = 
  [
    {name: 'New Zealand', code: 'NZ'}, 
    {name:'Australia', code: 'AU'}, 
    {name: 'Austria', code: 'AT'}, 
    {name: 'Azerbaijan', code: 'AZ'}, 
    {name: 'Bahamas', code: 'BS'}, 
    {name: 'Bahrain', code: 'BH'}, 
    {name: 'Bangladesh', code: 'BD'}, 
    {name: 'Barbados', code: 'BB'}, 
    {name: 'Belarus', code: 'BY'}, 
    {name: 'Belgium', code: 'BE'}, 
    {name: 'Belize', code: 'BZ'}, 
    {name: 'Benin', code: 'BJ'},
  ];
  private selectedCountry:string;

  form:FormGroup;
  constructor(private fb:FormBuilder,
              private userRegService:UserRegisterService,
              private router: Router){ 
      this.regUser = new CBAUser();
      this.regOrg = new CBAOrg();
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
      ),
      
      orgName:['',Validators.compose([
        Validators.required,      
      ])],

      orgCode:['',Validators.compose([
        Validators.required,      
      ])],

      streetAddrL1:['',Validators.compose([
        Validators.required,      
      ])],

      streetAddrL2:['',Validators.compose([
        Validators.required,      
      ])],

      city:['',Validators.compose([
        Validators.required,      
      ])],

      country:['',Validators.compose([
        Validators.required,      
      ])],

      orgPhoneNumber:['',Validators.compose([
        Validators.required,      
      ])],

      //TODO: Upload and display logo
      logoUrl:['',Validators.compose([
        //Validators.required,      
      ])],

      charitiesNumber:['',Validators.compose([
        Validators.required,      
      ])],

      gstNumber:['',Validators.compose([
        Validators.required,      
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
    this.regUser.phoneNumber= value.phoneNumber;
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
    this.regOrg.phoneNumber = value.orgPhoneNumber;
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
}
