import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from "rxjs";

@Injectable({
    providedIn:"root"
})
export class CheckUserExistService {
    constructor(private http:HttpClient){}
    checkUserExist(email:string):Observable<string>{
        return this.http.get<string>(`/api/user/checkuserexist/${email}`);
    }
}
