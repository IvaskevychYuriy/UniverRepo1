import { Component } from '@angular/core';

import { AuthenticationService } from '../../services/authentication.service';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id.toString(),
    selector: 'navbar-toolbar',
    templateUrl: 'navbar.component.html',
    styleUrls: ['navbar.component.css']
})
export class NavbarComponent {

    constructor(
        private authService: AuthenticationService,
        private router: Router
    ) { }

    get userName(){
        if (this.authService.userProfile) {
            return this.authService.userProfile.userName;
        }

        return null;
    }

    logout() {
        this.authService.logout();
    }

    login() {
        this.router.navigate(['/login']);
    }
    
    register() {
        this.router.navigate(['/register']);
    }
}