import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../guards/auth.guard';
import { LoginComponent } from '../components/login/login.component';
import { HomeComponent } from '../components/home/home.component';
import { ProductsMenuComponent } from '../components/products-menu/products-menu.component';
import { ShoppingCartComponent } from '../components/shopping-cart/shopping-cart.component';
import { OrderHistoryComponent } from '../components/order-history/order-history.component';
import { StoragesMenuComponent } from '../components/storages-menu/storages-menu.component';

const appRoutes: Routes = [
    { path: 'home', component: HomeComponent },
    { path: 'cart', component: ShoppingCartComponent },
    { path: 'orders', component: OrderHistoryComponent },
    { path: 'products', component: ProductsMenuComponent, canActivate: [AuthGuard] },
    { path: 'storages', component: StoragesMenuComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent, data: {is_register: false} },
    { path: 'register', component: LoginComponent, data: {is_register: true} },

    // otherwise redirect to home
    { path: '', redirectTo: 'home', pathMatch: 'full' },
    { path: '**', redirectTo: 'home' }
];

export const routing = RouterModule.forRoot(appRoutes);