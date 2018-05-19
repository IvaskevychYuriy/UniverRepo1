import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../guards/auth.guard';
import { LoginComponent } from '../components/login/login.component';
import { HomeComponent } from '../components/home/home.component';
import { AdminMenuComponent } from '../components/admin-menu/admin-menu.component';
import { ShoppingCartComponent } from '../components/shopping-cart/shopping-cart.component';
import { OrderHistoryComponent } from '../components/order-history/order-history.component';

const appRoutes: Routes = [
    { path: 'home', component: HomeComponent },
    { path: 'cart', component: ShoppingCartComponent },
    { path: 'orders', component: OrderHistoryComponent },
    { path: 'administration', component: AdminMenuComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent, data: {is_register: false} },
    { path: 'register', component: LoginComponent, data: {is_register: true} },

    // otherwise redirect to home
    { path: '', redirectTo: 'home', pathMatch: "full" },
    { path: '**', redirectTo: 'home' }
];

export const routing = RouterModule.forRoot(appRoutes);