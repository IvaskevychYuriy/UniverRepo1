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
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HttpOptionsInterceptor } from '../services/http-options.interceptor';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatToolbarModule, MatIconModule, MatGridListModule, MatMenuModule, MatListModule, MatInputModule, MatCardModule, MatPaginatorModule, MatSelectModule, MatOptionModule, MatTableModule } from '@angular/material';
import { NavbarComponent } from '../components/navbar/navbar.component';
import { ProductCellComponent } from '../components/product-grid/product-cell/product-cell.component';
import { ProductGridComponent } from '../components/product-grid/product-grid.component';
import { SidebarComponent } from '../components/sidebar/sidebar.component';
import { ProductCategoriesService } from '../services/product-categories.service';
import { AdminMenuComponent } from '../components/admin-menu/admin-menu.component';
import { ProductItemsService } from '../services/product-items.service';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { ShoppingCartComponent } from '../components/shopping-cart/shopping-cart.component';
import { OrderService } from '../services/order.service';
import { OrderHistoryComponent } from '../components/order-history/order-history.component';
import { UselessComponent } from '../components/useless/useless.component';
import { UselessTasksService } from '../services/useless-tasks.service';

@NgModule({
  imports: [
    BrowserModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatToolbarModule,
    MatListModule,
    MatIconModule,
    MatInputModule,
    MatSelectModule,
    MatOptionModule,
    MatCardModule,
    MatGridListModule,
    MatPaginatorModule,
    MatTableModule,
    MatMenuModule,
    routing
],
declarations: [
    AppComponent,
    AlertComponent,
    HomeComponent,
    AdminMenuComponent,
    LoginComponent,
    NavbarComponent,
    SidebarComponent,
    ProductCellComponent,
    ProductGridComponent,
    OrderHistoryComponent,
    ShoppingCartComponent,
    UselessComponent
],
providers: [
    AuthGuard,
    AlertService,
    AuthenticationService,
    ProductCategoriesService,
    ProductItemsService,
    ShoppingCartService,
    OrderService,
    UselessTasksService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpOptionsInterceptor, multi: true }
],
  bootstrap: [AppComponent]
})
export class AppModule { }
