import { Routes, RouterModule } from '@angular/router';
import { LoginComponent } from "./login/login.component";
import { LogintestComponent } from "./logintest/logintest.component";
import { RegisterComponent } from "./logintest/register.component";


const appRoutes: Routes = [
    // { path: '', component: HomeComponent, canActivate: [AuthGuard] },
    { path: 'logintest', component: LogintestComponent},
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);
