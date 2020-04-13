import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CBAUser } from '../domain/CBAUser';
import { tap } from "rxjs/operators";
import { CBAOrg } from '../domain/CBAOrg';

@Injectable({
    providedIn:'root'
})

export class UserRegisterService{
    private userRegUrl= 'api/user';
    
    constructor(private http:HttpClient) {}

    registerUser(user:CBAUser, org:CBAOrg){
        var body ={
            Email:user.email,
            FirstName: user.firstName,
            LastName: user.lastName,
            Password:user.password,
            PhoneNumber:user.phoneNumber,
            OrgName: org.OrgName,
            OrgCode:org.orgCode,
            StreetAddrL1: org.streetAddrL1,            
            StreetAddrL2: org.streetAddrL2,
            City: org.city,
            Country: org.country,
            OrgPhoneNumber: org.phoneNumber,
            LogoURL: org.logoUrl,
            CharitiesNumber: org.chritiesNumber,
            GSTNumber: org.gstNumber            
        }
        console.log('Post(send): ' +JSON.stringify(user));
        return this.http.post<CBAUser>(this.userRegUrl,body)
            .pipe(tap(data => console.log('Post(send): ' +JSON.stringify(user))))
    }
}