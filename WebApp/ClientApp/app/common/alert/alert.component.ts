import { Component, OnInit } from '@angular/core';
import { AlertService } from "./alert.service";
import { Subscription } from 'rxjs';
 

@Component({
    selector: 'alert',
    templateUrl: 'alert.component.html'
})
 
export class AlertComponent {
    message: any;
    subscription: Subscription;
 
    constructor(private alertService: AlertService) { }
 
    ngOnInit() {
        this.subscription = this.alertService.getMessage().subscribe(message => { this.message = message; });
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }
}
