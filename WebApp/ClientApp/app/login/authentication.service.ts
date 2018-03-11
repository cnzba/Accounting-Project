import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
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


@Injectable()
export class AuthenticationService {
    private accountUrl = 'api/account';
    private getUserUrl = 'api/user/';
    private userSubject = new ReplaySubject<IUser>(1);

    constructor(private http: HttpClient) {
        let login: string = localStorage.getItem('LoginId');

        if (login) this.http.get<IUser>(this.getUserUrl + login)
            .catch((err: HttpErrorResponse, caught: Observable<IUser>) => {
                if (err.status == 404) {
                    localStorage.removeItem('LoginId');
                    return Observable.of(null);
                }
                else throw err;
            })
            .subscribe(data => this.userSubject.next(data));
        else this.userSubject.next(null);
    }

    isLoggedIn(): Observable<boolean> {
        return this.getCurrentUser()
            .map((data: IUser) => {
                if (data != null) return true;
                else return false;
            });
    }

    getCurrentUser(): Observable<IUser> {
        return this.userSubject.asObservable();
    }

    login(username: string, password: string) {
        let params: HttpParams = new HttpParams().set('username', username).set('password', password);

        return this.http.post<string>(this.accountUrl, "", { params: params })
            .do(data => localStorage.setItem('LoginId', data))
            .flatMap(login => this.http.get<IUser>(this.getUserUrl + login))
            .do(data => this.userSubject.next(data));
    }

    logout() {
        return this.http.get(this.accountUrl)
            // fix for Angular 4.4.6
            .catch((err: HttpErrorResponse) => {
                if (err.status == 200) {
                    return "1"; // dummy data; doesn't matter
                }
                else throw err;
            })
            .do(data => {
                //localStorage.removeItem('currentUser');
                localStorage.removeItem('LoginId');
                localStorage.removeItem('forcePasswordChange');
                this.userSubject.next(null);
            });
    }
}
