import { AuthenticationService } from './authentication.service';
import { IUser } from "../users/user";

import { async, inject } from '@angular/core/testing';

import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { HttpRequest, HttpParams } from "@angular/common/http";

import { Observable } from "rxjs/Observable";
import 'rxjs/add/observable/of';

describe('Authentication service', () => {
    let service: AuthenticationService;
    let http;
    let user: IUser = {
        "login": "guest",
        "name": "guest",
        "active": true,
        "forcePasswordChange": false
    };

    beforeEach(() => {
        http = jasmine.createSpyObj('http', ['get', 'put', 'post']);
        localStorage.removeItem('LoginId');
        service = new AuthenticationService(http);
    });

    it("should establish authentication using id in local storage", async(() => {
        localStorage.setItem('LoginId', 'foo');
        http.get.and.returnValue(Observable.of(user));

        let tmpService = new AuthenticationService(http);

        tmpService.getCurrentUser().subscribe(data => expect(data).toEqual(user));
        expect(http.get.calls.count()).toEqual(1);
    }));

    it("logout should clear local storage", async(() => {
        http.get.and.returnValue(Observable.of("1"));
        service.logout().subscribe();

        expect(localStorage.getItem('LoginId') == null);
        expect(http.get.calls.count()).toEqual(1);
    }));

    it("login should return user info", async(() => {
        http.post.and.returnValue(Observable.of("fooId"));
        http.get.and.returnValue(Observable.of(user));

        service.login("foo", "bar").subscribe(data => expect(data).toEqual(user));

        expect(http.post.calls.count()).toEqual(1);
        expect(http.get.calls.count()).toEqual(1);
    }));

    it("login should send username/password", async(() => {
        http.post.and.returnValue(Observable.of("fooId"));
        http.get.and.returnValue(Observable.of(user));

        service.login("foo", "bar").subscribe();

        expect(http.post.calls.count()).toEqual(1);

        let params: HttpParams = http.post.calls.argsFor(0)[2].params;

        expect(params.get('username')).toEqual('foo');
        expect(params.get('password')).toEqual('bar');
    }));

    it("login should set user id in local storage", async(() => {
        http.post.and.returnValue(Observable.of("fooId"));
        http.get.and.returnValue(Observable.of(user));

        service.login("foo", "bar").subscribe();

        expect(http.post.calls.count()).toEqual(1);
        expect(localStorage.getItem('LoginId')).toEqual('fooId');
    }));

});
