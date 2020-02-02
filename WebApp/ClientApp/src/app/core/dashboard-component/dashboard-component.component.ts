import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { DashboardRecivableData } from '../domain';

@Component({
  selector: 'app-dashboard-component',
  templateUrl: './dashboard-component.component.html',
  styleUrls: ['./dashboard-component.component.css']
})
export class DashboardComponent implements OnInit {

  // displayColumns: string[] = ['title','count', 'value'];
  // receiveablesData = [
  //   {title:"Invoice Issued", count:11, value:12343},
  //   {title:"Payment Received", count:12, value:2343},
  //   {title:"Invoice Overdue", count:13, value:343},
  // ]


  displayedColumns: string[] = ['title', 'count', 'value'];
  //issuedValue$ : Observable<number>;
  receiveDataSource =[
    {title:"Invoice Issued", count:0, value:0,status:"issued"},
    {title:"Payment Received", count:0, value:0 ,status:"paid"},
    {title:"Invoice Overdue", count:0, value:0, status:"overdue"},
  
  ]

  payDataSource = [
    {title:"Expenditures", count:21, value:223},
    {title:"Payment made", count:22, value:2343},
    {title:"Payment Overdue", count:23, value:243},
  ]

  constructor(private route:Router, private http:HttpClient) { }

  ngOnInit() {

    /*此处重构，一个request获取所有Receivable的data：
    // Issued: count, value
    //Payble: count, value
    //Overdue:count, value
    {
      issued:{
        count:--
        value:--
      },
      ---
    }*/
    this.http.get<DashboardRecivableData>("/api/invoice/dashboarddata")
      .subscribe(data => {
        this.receiveDataSource[0].value = data.issuedValue,
        this.receiveDataSource[0].count = data.issuedCount,
        this.receiveDataSource[1].value = data.paidValue,
        this.receiveDataSource[1].count = data.paidCount,
        this.receiveDataSource[2].value = data.overdueValue,
        this.receiveDataSource[2].count = data.overdueCount
      });
  }

  //InvoiceStatus 
  //{ New = 0, Draft = 1, Issued = 2, Paid = 3, Cancelled = 4 , overdued =5}
  //TODO: overdued is not exist in the enum of the server, need to be redesigned
  onClickInvoiceRow(ev){
    console.log(ev);
    let status = this.receiveDataSource.filter( item => item.title==ev.title)[0].status;
    this.route.navigate(["invoices"],{queryParams:{status:status}});
  }

}
