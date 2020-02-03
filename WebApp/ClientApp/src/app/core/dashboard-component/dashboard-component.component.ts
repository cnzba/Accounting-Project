import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { DashboardRecivableData } from '../domain';
import { CoreService } from '../services';

@Component({
  selector: 'app-dashboard-component',
  templateUrl: './dashboard-component.component.html',
  styleUrls: ['./dashboard-component.component.css']
})
export class DashboardComponent implements OnInit {

  //Data for the matirial tables.
  displayedColumns: string[] = ['title', 'count', 'value'];
  //issuedValue$ : Observable<number>;
  receiveDataSource =[
    {title:"Invoice Issued", count:0, value:0,status:"issued"},
    {title:"Payment Received", count:0, value:0 ,status:"paid"},
    {title:"Invoice Overdue", count:0, value:0, status:"overdue"},
  
  ]

  //This is mock data, will be replaced when complete the payment module.
  payDataSource = [
    {title:"Expenditures", count:21, value:223},
    {title:"Payment made", count:22, value:2343},
    {title:"Payment Overdue", count:23, value:243},
  ]

  constructor(private route:Router, 
    private http:HttpClient,
    private coreService:CoreService
    ) { }

  ngOnInit() {
    
    //Retrive data needed for receivable part.
    //this.http.get<DashboardRecivableData>("/api/invoice/dashboarddata")
      this.coreService.getDashboardReiceivableData()
      .subscribe(data => {

        //Invoice Issued
        this.receiveDataSource[0].value = data.issuedValue,
        this.receiveDataSource[0].count = data.issuedCount,
        //Payment Received
        this.receiveDataSource[1].value = data.paidValue,
        this.receiveDataSource[1].count = data.paidCount,
        //Invoice Overdue
        this.receiveDataSource[2].value = data.overdueValue,
        this.receiveDataSource[2].count = data.overdueCount
      });
  }

  //InvoiceStatus 
  //{ New = 0, Draft = 1, Issued = 2, Paid = 3, Cancelled = 4 , overdued =5}
  onClickInvoiceRow(ev){
    console.log(ev);
    let status = this.receiveDataSource.filter( item => item.title==ev.title)[0].status;
    this.route.navigate(["invoices"],{queryParams:{status:status}});
  }

}
