import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

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

  receiveDataSource =[
    {title:"Invoice Issued", count:11, value:12343},
    {title:"Payment Received", count:12, value:2343},
    {title:"Invoice Overdue", count:13, value:343},
  
  ]

  payDataSource = [
    {title:"Expenditures", count:21, value:223},
    {title:"Payment made", count:22, value:2343},
    {title:"Payment Overdue", count:23, value:243},
  ]

  constructor(private route:Router) { }

  ngOnInit() {
  }

  onClickInvoiceRow(ev){
    console.log(ev);
    this.route.navigate(["invoices"]);

  }

}
