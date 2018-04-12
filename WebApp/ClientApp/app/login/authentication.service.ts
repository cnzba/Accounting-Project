import { Injectable } from '@angular/core';
import { IUser } from "../users/user";
import { Observable } from 'rxjs/Observable';
import { ReplaySubject } from 'rxjs/ReplaySubject';
import 'rxjs/add/observable/throw';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/do';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/delay';
import 'rxjs/add/operator/mergeMap';
import {
    HttpRequest,
    HttpHandler,
    HttpEvent,
    HttpInterceptor,
    HttpResponse,
    HttpErrorResponse,
    HttpParams,
    HttpClient,
    HttpBackend
} from "@angular/common/http";
import { ApiError } from "../common/error.service";
import { Router } from "@angular/router";
import { HttpHeaders } from "@angular/common/http";
import { HttpEventType } from "@angular/common/http";

// the helper service handles the browser side of things (storing login name in local storage)
class AuthenticationHelper {
    private userSubject = new ReplaySubject<IUser>(1);

    constructor() {
    }

    setCurrentUser(user: IUser) {
        this.userSubject.next(user);
        if (user != null) this.setStoredUser(user.email);
        else this.clearStoredUser();
    }

    getCurrentUser(): Observable<IUser> {
        return this.userSubject.asObservable();
    }

    clearCurrentUser() {
        this.userSubject.next(null);
        this.clearStoredUser();
    }

    isLoggedIn(): Observable<boolean> {
        return this.getCurrentUser()
            .map((data: IUser) => {
                if (data != null) return true;
                else return false;
            });
    }

    logout() {
        this.clearStoredUser();
        localStorage.removeItem('forcePasswordChange');
        this.userSubject.next(null);
    }

    clearStoredUser() {
        localStorage.removeItem('LoginId');
    }

    getStoredUser(): string {
        return localStorage.getItem('LoginId');
    }

    setStoredUser(user: string) {
        localStorage.setItem('LoginId', user)
    }
}

// authentication service uses HttpBackend instead of HttpClient so that it bypasses
// the global error handling provided by HttpErrorInterceptor
// from Angular 5.2.3 it will be possible to revert to using HttpClient
@Injectable()
export class AuthenticationService {
    private accountUrl = 'api/account';
    private getUserUrl = 'api/user/';
    private helper = new AuthenticationHelper();

    constructor(private backend: HttpBackend) {
        let login: string = this.helper.getStoredUser();

        if (login) {
            console.log(`AUTH: Refreshing user ${login}`);
            console.log(`AUTH: Retrieving user information for logged in user ${login}`);
            let req = new HttpRequest("GET", this.getUserUrl + login);
            this.backend.handle(req)
                .filter(event => event.type === HttpEventType.Response)
                .subscribe(
                (data: HttpResponse<IUser>) => this.helper.setCurrentUser(data.body),
                (err: HttpErrorResponse) => {
                    console.log("No user logged in.");
                    this.helper.setCurrentUser(null);
                });
        }
        else this.helper.setCurrentUser(null);
    }

    isLoggedIn(): Observable<boolean> {
        return this.helper.isLoggedIn();
    }

    getCurrentUser(): Observable<IUser> {
        return this.helper.getCurrentUser();
    }

    login(username: string, password: string) {
        let params: HttpParams = new HttpParams().set('username', username).set('password', password);
        let headers: HttpHeaders = new HttpHeaders().set('Accept', 'application/json')

        let post = new HttpRequest<string>("POST", this.accountUrl, "", { params: params, headers: headers });

        console.log(`Logging in with ${username} and ${password}`);
        return this.backend.handle(post)
            .filter(event => event.type === HttpEventType.Response)
            .flatMap((login: HttpResponse<string>) => {
                let get = new HttpRequest("GET", this.getUserUrl + login.body);
                console.log(`Retrieving info for ${login.body}`);
                return this.backend.handle(get).filter(event => event.type === HttpEventType.Response);
            })
            .map((data: HttpResponse<IUser>) => data.body)
            .do(user => this.helper.setCurrentUser(user));
    }

    localLogout(): void {
        this.helper.logout();
    }

    logout() {
        let get = new HttpRequest("GET", this.accountUrl);

        console.log(`AUTH: Logging out user`);

        return this.backend.handle(get)
            .filter(event => event.type === HttpEventType.Response)
            .do(data => {
                this.helper.logout();
            }).catch((err: HttpErrorResponse) => {
                // fix for Angular 4.4.6 for empty body
                if (err.status == 200) return "1";
                else throw err;
            });
    }

}

