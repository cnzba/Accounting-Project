import { TestBed, async, inject } from '@angular/core/testing';

import { AuthGuard } from './auth.guard';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from "@angular/router";
import { AuthenticationService } from "./authentication.service";

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/of';

class MockAuthService {
    isLoggedIn(): Observable<boolean> {
        return Observable.of(false);
    }
}

describe('AuthGuard', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [AuthGuard,
                { provide: AuthenticationService, useClass: MockAuthService }
            ],
            imports: [RouterTestingModule]
        });
    });

    it('should redirect non-logged in user', async(inject([AuthGuard, Router],
        (guard: AuthGuard, router: Router) => {
            // add a spy
            spyOn(router, 'navigate');

            expect(guard).toBeDefined();

            var obs = (guard.canActivate(<any>{}, <any>{}) as Observable<boolean>);
            obs.subscribe(result => {
                expect(result).toEqual(false);
            });
            expect(router.navigate).toHaveBeenCalled();
        })));

    it('should accept a logged-in user', async(inject([AuthGuard, Router, AuthenticationService],
        (guard: AuthGuard, router: Router, service: AuthenticationService) => {
        // add a spy
        spyOn(router, 'navigate');
        spyOn(service, 'isLoggedIn').and.returnValue(Observable.of(true));

        expect(guard).toBeDefined();
        var obs = (guard.canActivate(<any>{}, <any>{}) as Observable<boolean>);
        obs.subscribe(result => {
            expect(result).toEqual(true);
        });
        expect(router.navigate).not.toHaveBeenCalled();

    })));
});
