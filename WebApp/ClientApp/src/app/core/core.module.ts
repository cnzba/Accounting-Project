import { NgModule } from '@angular/core';
import { SidebarComponent } from './sidebar';
import { SharedModule } from '../shared';
import { DashboardComponent } from './dashboard-component';
import {CoreRoutingModule} from './core-routing.module'

@NgModule({
  imports: [
    SharedModule,
    CoreRoutingModule,

  ],
  exports:[
    SidebarComponent,
    DashboardComponent,

  ],
  declarations: [
    SidebarComponent,
    DashboardComponent,

  ]
})
export class CoreModule { }
