import { Component, ViewChild, ViewChildren } from '@angular/core';

import { AuthenticationService } from '../../services/authentication.service';
import { Router } from '@angular/router';
import { MatMenu, MatMenuTrigger } from '@angular/material';

@Component({
    moduleId: module.id.toString(),
    selector: 'navbar-toolbar',
    templateUrl: 'navbar.component.html',
    styleUrls: ['navbar.component.css']
})
export class NavbarComponent {
    @ViewChildren(MatMenuTrigger) triggers: MatMenuTrigger[];
    
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

    openProductsMenu() {
        this.closeMenus();
        this.router.navigate(['/products']);
    }
    
    openStoragesMenu() {
        this.closeMenus();
        this.router.navigate(['/storages']);
    }

    openShoppingCart() {
        this.router.navigate(['/cart']);
    }

    openOrderHistory() {
        this.closeMenus();
        this.router.navigate(['/orders']);
    }

    private closeMenus() {
        this.triggers.forEach(t => t.closeMenu());
    }
}