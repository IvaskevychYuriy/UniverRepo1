import { Routes, RouterModule } from '@angular/router';

import { AuthGuard } from '../guards/auth.guard';
import { LoginComponent } from '../components/login/login.component';
import { HomeComponent } from '../components/home/home.component';
import { SecretComponent } from '../components/secret/secret.component';

const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    { path: 'secret', component: SecretComponent, canActivate: [AuthGuard] },
    { path: 'login', component: LoginComponent, data: {is_register: false} },
    { path: 'register', component: LoginComponent, data: {is_register: true} },

    // otherwise redirect to home
    { path: '**', redirectTo: '' }
];

export const routing = RouterModule.forRoot(appRoutes);