import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { DashboardRecivableData } from '../domain';

@Injectable({
    providedIn:'root'
})
export class CoreService {
    constructor (private http:HttpClient){}
    getDashboardReiceivableData(){
       return this.http.get<DashboardRecivableData>("/api/invoice/dashboarddata");
    }
    
}