import { Component, OnInit} from '@angular/core';
import { FormGroup, Validators, FormBuilder, ValidatorFn, AbstractControl, ValidationErrors } from '@angular/forms';
import { UserRegisterService, } from "../services";
import { CBAUser } from '../domain/CBAUser';
import { Router } from '@angular/router';
import { CBAOrg } from '../domain/CBAOrg';
import { UserValidators } from '../validators/user.validator';
import { AlertService } from 'src/app/common/alert/alert.service';
import { debounceTime, filter, subscribeOn } from 'rxjs/operators';
import { SpinnerService } from "../../common/spinner.service";

@Component({
    selector: 'app-register',
    templateUrl: './register.component.html',
    styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit{
  selectedTab = 0;
  public displayInputGST = true;
  private _regUser:CBAUser;
  private _regOrg:CBAOrg;
  logoImgUrl ="/assets/images/logo_placeholder.png";
  selectedCountry = 'New Zealand';
  form:FormGroup;

  constructor(private fb:FormBuilder,
              private userRegService:UserRegisterService,
              private router: Router,
              private checkUserExistService:UserValidators,
              private alertService:AlertService,
              private spinnerService:SpinnerService){ 
      this._regUser = new CBAUser();
      this._regOrg = new CBAOrg();
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
            firstName: ['', Validators.compose([
                Validators.required,
                Validators.maxLength(50)
            ])],
            lastName: ['', Validators.compose([
                Validators.required,
                Validators.maxLength(50)
            ])],

      phoneNumberPrefix:['',Validators.compose([
        Validators.required,         
        this.fixLength(3)
      ])],

      phoneNumberBody:['',Validators.compose([
        Validators.required, 
        Validators.minLength(7),
        Validators.maxLength(10)
      ])],

            passwords: this.fb.group({
                password: ['', Validators.compose([
                    Validators.required,
                    Validators.minLength(8)
                ])],
                confirmedPassword: ['', Validators.compose([
                    Validators.required,
                    Validators.minLength(8)
                ])]
            },
                { validator: this.comparePasswords }
            ),

            orgName: ['', Validators.compose([
                Validators.required,
                Validators.maxLength(50)
            ])],

      orgCode:['',Validators.compose([
        Validators.required, 
        this.fixLength(4),           
      ])],

            streetAddrL1: ['', Validators.compose([
                Validators.required,
            ])],

      streetAddrL2:['',Validators.compose([              
      ])],

            city: ['', Validators.compose([
                Validators.required,
            ])],

            country: ['', Validators.compose([
                Validators.required,
            ])],

      orgPhoneNumberPrefix:['',Validators.compose([
        Validators.required, 
        this.fixLength(3)     
      ])],

      orgPhoneNumberBody:['',Validators.compose([
        Validators.required,   
        Validators.minLength(7),
        Validators.maxLength(10)   
      ])],

            logoUrl: ['', Validators.compose([
            ])],

            charitiesNumber: ['', Validators.compose([
                Validators.maxLength(20),
            ])],

      inputGST:this.fb.group({
        haveGST:[false ],
        gstNumber:[""]
        },
        {validator:this.validateGST}
      )
    })    
  }
  fixLength(length:number): ValidatorFn {    
    return (control: AbstractControl):ValidationErrors | null =>{            
      var num = control.value;
      return num.length != length ? {"fixLength":true, "requiredLength":length} : null;      
    }
  }
  
  validateGST(fb:FormGroup){
    let haveGSTCtrl = fb.get('haveGST');
    let gstNumberCtrl = fb.get('gstNumber');
    if (haveGSTCtrl.value && gstNumberCtrl.value==""){
      gstNumberCtrl.setErrors({required:true});
    }else{
      gstNumberCtrl.setErrors(null);
    }
  }

  comparePasswords(fb:FormGroup){
    const confirmPswrdCtrl = fb.get('confirmedPassword');
    if (!confirmPswrdCtrl) return;
    const confirmPwd$ = confirmPswrdCtrl.valueChanges.pipe(
      debounceTime(300),
      filter( v => confirmPswrdCtrl.valid)
    )
    confirmPwd$.subscribe( confirmPwd =>{
      if (fb.get('password').value != confirmPwd){
        confirmPswrdCtrl.setErrors({passwordMismatch:true});
      }        
      else
        confirmPswrdCtrl.setErrors(null);
    })
  }

  onSubmit({value, valid},ev: Event){
    ev.preventDefault();
    this.spinnerService.showSpinner();
    this._regUser.phoneNumber= value.phoneNumberPrefix+ "-"+value.phoneNumberBody;
    this._regUser.firstName= value.firstName;
    this._regUser.lastName = value.lastName;
    this._regUser.email = value.email;
    this._regUser.password = value.passwords.password;
    this._regOrg.OrgName= value.orgName;
    this._regOrg.orgCode= value.orgCode;
    this._regOrg.streetAddrL1 = value.streetAddrL1;
    this._regOrg.streetAddrL2 = value.streetAddrL2;
    this._regOrg.city = value.city;
    this._regOrg.country = this.selectedCountry;
    this._regOrg.phoneNumber = value.orgPhoneNumberPrefix + "-" + value.orgPhoneNumberBody;
    this._regOrg.logoUrl = value.logoUrl;
    this._regOrg.chritiesNumber = value.charitiesNumber;
    this._regOrg.gstNumber = value.gstNumber;
    
    this.userRegService.registerUser(this._regUser,this._regOrg).subscribe(
      (res:any) =>{
        if (res == "succeed"){
          this.router.navigate(['/login']);
          this.alertService.success("Register succeed and a confirmation email has been sent to you.");
        }else{
          console.log(res);
          this.alertService.error("Register failed.");
          this.spinnerService.hideSpinner();
        }
      },
      err => {
        console.log(err);
        this.spinnerService.hideSpinner();
      }
    );
  }

    nextTab() {
        this.selectedTab = 1;
    }


    onTabChange(index: number) {
        this.selectedTab = index;
    }

  toggleGST(event){
    if (event.checked == true){
      this.displayInputGST=false;
    } else{
      this.displayInputGST=true;
    }
  }

  public uploadLogoFinished =(event) =>{
    let url = event.dbPath;
    this.logoImgUrl= url;
    this.form.get('logoUrl').setValue(url);
  }



}
