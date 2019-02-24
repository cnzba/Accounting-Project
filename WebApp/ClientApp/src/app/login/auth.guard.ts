import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthenticationService } from "./authentication.service";
import { Observable } from 'rxjs';
import { take, tap } from 'rxjs/operators';


@Injectable()
export class AuthGuard implements CanActivate {
    constructor(private router: Router, private authService: AuthenticationService) { }

    canActivate(
        next: ActivatedRouteSnapshot,
        state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

        return this.authService.isLoggedIn()
            .pipe(take(1), tap(
            (result: boolean) => {
                if (!result) {
                    console.log(`Guard activated for ${state.url}`);
                    this.router.navigate(['/login', { returnUrl: state.url }]);
                }
            }));
    };
    
}
