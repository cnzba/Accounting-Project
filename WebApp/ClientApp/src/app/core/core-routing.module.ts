import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { DashboardComponent } from './dashboard-component';
import { AuthGuard } from '../login/auth.guard';
import { PageNotFoundComponent } from '../pagenotfound/pagenotfound.component';


const routes: Routes = [
    { path: 'core', component: DashboardComponent, canActivate:[AuthGuard] },
    { path: '**', component: PageNotFoundComponent }

    //{ path: 'path/:routeParam', component: MyComponent },
    //{ path: 'staticPath', component: ... },
    //{ path: '**', component: ... },
    //{ path: 'oldPath', redirectTo: '/staticPath' },
    //{ path: ..., component: ..., data: { message: 'Custom' }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class CoreRoutingModule {}
