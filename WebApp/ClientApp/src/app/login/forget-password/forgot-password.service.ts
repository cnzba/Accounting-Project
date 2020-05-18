import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class ForgotPasswordService {
    private url: string = "api/ForgotPassword";

    constructor(private http: HttpClient) { }

    sendEmail(email: string) {
        return this.http.post(this.url, { "email": email });
    }
}
