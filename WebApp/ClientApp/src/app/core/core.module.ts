import { NgModule } from '@angular/core';
import { SidebarComponent } from './sidebar';
import { SharedModule } from '../shared';
import { DashboardComponent } from './dashboard-component';
import {CoreRoutingModule} from './core-routing.module';
import { RegisterComponent } from "./register";
import { UserRegisterService, CheckUserExistService } from './services';
import { RegErrTranslate } from '../pipes/regsiterInputErr.pipe';

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
    CheckUserExistService
  ],
  declarations: [
    SidebarComponent,
    DashboardComponent,
    RegisterComponent,
    RegErrTranslate

  ]
})
export class CoreModule { }
