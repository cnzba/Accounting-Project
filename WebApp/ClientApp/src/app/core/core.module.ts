import { NgModule } from '@angular/core';
import { SidebarComponent } from './sidebar';
import { SharedModule } from '../shared';
import { DashboardComponent } from './dashboard-component';
import {CoreRoutingModule} from './core-routing.module';
import { RegisterComponent } from "./register";
import { UserRegisterService } from './services';

@NgModule({
  imports: [
    SharedModule,
    CoreRoutingModule,

  ],
  exports:[
    SidebarComponent,
    DashboardComponent,
    RegisterComponent

  ],

  providers:[
    UserRegisterService,
  ],
  declarations: [
    SidebarComponent,
    DashboardComponent,
    RegisterComponent

  ]
})
export class CoreModule { }
