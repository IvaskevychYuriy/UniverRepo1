import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

import { AuthenticationService } from '../../services/authentication.service';
import { LoginInfo } from '../../models/login-info';
import { AlertService } from '../../services/alert.service';
import { Subscription } from 'rxjs';

@Component({
    moduleId: module.id.toString(),
    templateUrl: 'login.component.html'
})

export class LoginComponent implements OnInit, OnDestroy {
    private model: LoginInfo;
    private loading: boolean = false;
    private returnUrl: string;

    private isRegister: boolean = false;
    private subscription: Subscription;

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private authenticationService: AuthenticationService,
        private alertService: AlertService) { 
            this.model = new LoginInfo();
        }

    ngOnInit() {
        this.subscription = this.route.data
            .subscribe(data => this.isRegister = data.is_register);

        // get return url from route parameters or default to '/'
        this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';

        if (this.authenticationService.userProfile) {
            this.router.navigate([this.returnUrl]);
        }
    }

    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }

    login() {
        this.loading = true;
        this.authenticationService.login(this.model)
            .subscribe(
                data => {
                    this.router.navigate([this.returnUrl]);
                },
                error => {
                    this.alertService.error(error);
                    this.loading = false;
                });
    }

    register() {
        this.loading = true;
        this.authenticationService.register(this.model)
            .subscribe(
                data => {
                    this.alertService.success('Registration successful', true);
                    this.router.navigate(['/login']);
                },
                error => {
                    this.alertService.error(error);
                    this.loading = false;
                });
    }
}