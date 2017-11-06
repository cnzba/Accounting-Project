import { TestBed, async, inject } from '@angular/core/testing';

import { AuthGuard } from './auth.guard';
import { RouterTestingModule } from '@angular/router/testing';
import { Router } from "@angular/router";
import { AuthenticationService } from "./authentication.service";

class MockAuthService {
    isLoggedIn(): boolean {
        return false;
    }
}
    
describe('AuthGuard', () => {
    beforeEach(() => {
        TestBed.configureTestingModule({
            providers: [AuthGuard,
                { provide: AuthenticationService, useClass: MockAuthService}
            ],
            imports: [RouterTestingModule]
        });
    });

    it('should redirect non-logged in user', inject([AuthGuard, Router], (guard: AuthGuard, router: Router) => {
        // add a spy
        spyOn(router, 'navigate');

        expect(guard).toBeDefined();
        expect(guard.canActivate(<any>{}, <any>{})).toEqual(false);
        expect(router.navigate).toHaveBeenCalled();
    }));

    xit('should accept a logged-in user', inject([AuthGuard, Router], (guard: AuthGuard, router: Router) => {
        // add a spy
        spyOn(router, 'navigate');

        expect(guard).toBeDefined();
        expect(guard.canActivate(<any>{}, <any>{})).toEqual(true);
        expect(router.navigate).not.toHaveBeenCalled();
        
    }));
});
