import { Component, OnInit, NgModule } from '@angular/core';
import { Router } from '@angular/router';
import { MenuItem}from 'primeng/api'

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {

    constructor( private _router:Router) { }
    items: MenuItem[];
    breadcrumbItems: MenuItem[];
    onMenuInvoices() {
        
        this._router.navigate(['/receivables']);
    }
   
    ngOnInit() {
        this.breadcrumbItems = [
            { label: 'Categories' },
            { label: 'Sports' },
            { label: 'Football' },
        ];
        this.items = [
            {
                label: 'Admin',
                icon: 'pi pi-pw pi-users',
                items: [
                    {
                        label: 'Organisations',
                        icon: 'pi pi-minus'
                         
                    },
                    { label: 'Users', icon: 'pi pi-minus'},
                    //{ separator: true },
                    
                ]
            },
            {
                label: 'Receivables',
                icon: 'pi pi-fw pi-dollar',
                items: [
                    { label: 'Clients', icon: 'pi pi-fw pi-minus' },
                    {
                        label: '**Invoices'
                        , icon: 'pi pi-fw pi-minus'
                        , command: () => this.onMenuInvoices()
                    }
                ]
            },
            {
                label: 'Payables',
                icon: 'pi pi-fw pi-money-bill',
                items: [
                    {
                        label: 'Suppliers',
                        icon: 'pi pi-fw pi-minus'
                    },
                    {
                        label: 'Payments',
                        icon: 'pi pi-fw pi-minus',
                        
                    }
                ]
            },
            {
                label: 'Help',
                icon: 'pi pi-fw pi-question',
                items: [
                    {
                        label: 'Manual',
                        icon: 'pi pi-minus'                        
                    },
                    {
                        label: 'About',
                        icon: 'pi pi-minus'
                    }
                ]
            },
            { label: 'Quit', icon: 'pi pi-times' }
        ];
        

  }

}
