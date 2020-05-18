import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { DashboardReceivableData } from '../domain';

@Injectable({
    providedIn:'root'
})



export class CoreService {
    constructor (private http:HttpClient){}
    getDashboardReiceivableData(){
        //Intercept the header in login/auth.interceptor.ts
        return this.http.get<DashboardReceivableData>("/api/invoice/dashboarddata");
    }
    
}
