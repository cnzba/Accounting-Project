import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CBAUser } from '../domain/CBAUser';
import { tap } from "rxjs/operators";

@Injectable({
    providedIn:'root'
})

export class UserRegisterService{
    private userRegUrl= 'api/user';
    
    constructor(private http:HttpClient) {}

    registerUser(user:CBAUser){
        var body ={
            Email:user.email,
            FirstName: user.firstName,
            LastName: user.lastName,
            Password:user.password,
            PhoneNumber:user.phoneNumber
        }
        console.log('Post(send): ' +JSON.stringify(user));
        return this.http.post<CBAUser>(this.userRegUrl,body)
            .pipe(tap(data => console.log('Post(send): ' +JSON.stringify(user))))
    }
}