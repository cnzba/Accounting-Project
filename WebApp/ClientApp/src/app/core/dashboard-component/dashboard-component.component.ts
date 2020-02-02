import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

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
  issuedValue$ : Observable<number>;
  receiveDataSource =[
    {title:"Invoice Issued", count:11, value:this.issuedValue$,status:"issued"},
    {title:"Payment Received", count:12, value:this.issuedValue$ ,status:"paid"},
    {title:"Invoice Overdue", count:13, value:this.issuedValue$, status:"overdue"},
  
  ]

  payDataSource = [
    {title:"Expenditures", count:21, value:223},
    {title:"Payment made", count:22, value:2343},
    {title:"Payment Overdue", count:23, value:243},
  ]

  constructor(private route:Router, private http:HttpClient) { }

  ngOnInit() {
    this.issuedValue$ = this.http.get<number>("/api/invoice/totalbystatus/2");
    this.issuedValue$.subscribe(x => console.log(x));
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
