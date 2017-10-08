import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { routing } from './app.routing.module';
import { AlertComponent } from '../directives/alert-directive/alert.component';
import { LoginComponent } from '../components/login/login.component';
import { AuthGuard } from '../guards/auth.guard';
import { AlertService } from '../services/alert.service';
import { AuthenticationService } from '../services/authentication.service';
import { HomeComponent } from '../components/home/home.component';
import { SecretComponent } from '../components/secret/secret.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HttpOptionsInterceptor } from '../services/http-options.interceptor';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatToolbarModule, MatIconModule } from '@angular/material';
import { NavbarComponent } from '../components/navbar/navbar.component';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatToolbarModule,
    MatIconModule,
    routing
],
declarations: [
    AppComponent,
    AlertComponent,
    HomeComponent,
    SecretComponent,
    LoginComponent,
    NavbarComponent
],
providers: [
    AuthGuard,
    AlertService,
    AuthenticationService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpOptionsInterceptor, multi: true }
],
  bootstrap: [AppComponent]
})
export class AppModule { }
