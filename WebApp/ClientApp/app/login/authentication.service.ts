import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpErrorResponse } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'
import { User } from "../users/user";

@Injectable()
export class AuthenticationService {
    constructor(private http: HttpClient) { }

    isLoggedIn(): boolean {
        if (localStorage.getItem('currentUser')) {
            // logged in so return true
            return true;
        }
        else return false;
    }

    getCurrentUser(): User {
        if (!this.isLoggedIn()) return null;
        else return { id: 0, firstName: '', lastName: 'guest', username: 'guest', password: 'guest' };
    }

    login(username: string, password: string) {
        const url: string = '/api/Account';
        let params: HttpParams = new HttpParams().set('username', username).set('password', password);

        return this.http.post<string>(url, "", { params: params })
            .map(data => {
                localStorage.setItem('currentUser', "loggedin");
                return "1"; // dummy data; doesn't matter
            })
            .catch((err: HttpErrorResponse) => {
                if (err.status == 200) {
                    localStorage.setItem('currentUser', "loggedin")
                    return "1"; // dummy data; doesn't matter
                }
                else throw err;
            });
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
    }
}
