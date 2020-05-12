import { AbstractControl, AsyncValidatorFn } from "@angular/forms";
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { CheckUserExistService } from "../services";
import { map, debounceTime } from "rxjs/operators";
import { Observable } from "rxjs";

@Injectable({
    providedIn:'root'
})

export class UserValidators{
    
    constructor(private http:HttpClient, 
        private checkUserExistService:CheckUserExistService){}

    emailExistValidator():AsyncValidatorFn{
        return (control:AbstractControl):Observable<{[key:string]:any} | null> =>{
            return this.checkUserExistService.checkUserExist(control.value).pipe(
                map (res => {
                    if (res==="Exist") return {'userExist':true};
                    return null;
                }),
                debounceTime(300)
            )
                
        }
    }    
}