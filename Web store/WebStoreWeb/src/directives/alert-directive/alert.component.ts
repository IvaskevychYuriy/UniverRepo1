import { Component, OnInit, OnDestroy } from '@angular/core';
import { AlertService } from '../../services/alert.service';
import { Subscription } from 'rxjs';

@Component({
    moduleId: module.id.toString(),
    selector: 'alert',
    templateUrl: 'alert.component.html'
})
export class AlertComponent implements OnInit, OnDestroy{
    private subscription: Subscription;
    message: any;

    constructor(private alertService: AlertService) { }

    ngOnInit(): void {
        this.subscription = this.alertService.getMessage().subscribe(message => { 
            this.message = message; 
            setTimeout(function () {
                this.message = null;
            }, 1000);
        });
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }
}