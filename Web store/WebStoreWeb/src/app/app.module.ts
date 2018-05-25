import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';

import { AppComponent } from './app.component';
import { routing } from './app.routing.module';
import { LoginComponent } from '../components/login/login.component';
import { AuthGuard } from '../guards/auth.guard';
import { AlertService } from '../services/alert.service';
import { AuthenticationService } from '../services/authentication.service';
import { HomeComponent } from '../components/home/home.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { HttpOptionsInterceptor } from '../services/http-options.interceptor';

import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MatButtonModule, MatToolbarModule, MatIconModule, MatGridListModule, MatMenuModule, MatListModule, MatInputModule, MatCardModule, MatPaginatorModule, MatSelectModule, MatOptionModule, MatTableModule, MatSnackBarModule } from '@angular/material';
import { NavbarComponent } from '../components/navbar/navbar.component';
import { ProductCellComponent } from '../components/product-grid/product-cell/product-cell.component';
import { ProductGridComponent } from '../components/product-grid/product-grid.component';
import { SidebarComponent } from '../components/sidebar/sidebar.component';
import { ProductCategoriesService } from '../services/product-categories.service';
import { ProductsMenuComponent } from '../components/products-menu/products-menu.component';
import { ProductItemsService } from '../services/product-items.service';
import { ShoppingCartService } from '../services/shopping-cart.service';
import { ShoppingCartComponent } from '../components/shopping-cart/shopping-cart.component';
import { OrderService } from '../services/order.service';
import { OrderHistoryComponent } from '../components/order-history/order-history.component';
import { StoragesMenuComponent } from '../components/storages-menu/storages-menu.component';
import { StoragesService } from '../services/storages.service';
import { ReportsComponent } from '../components/reports/reports.component';
import { ReportsService } from '../services/reports.service';
import { HangfireDashboardComponent } from '../components/hangfire-dashboard/hangfire-dashboard.component';

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
    MatSnackBarModule,
    routing
],
declarations: [
    AppComponent,
    HomeComponent,
    ProductsMenuComponent,
    LoginComponent,
    NavbarComponent,
    SidebarComponent,
    ProductCellComponent,
    ProductGridComponent,
    OrderHistoryComponent,
    ShoppingCartComponent,
    StoragesMenuComponent,
    ReportsComponent,
    HangfireDashboardComponent
],
providers: [
    AuthGuard,
    AuthenticationService,
    ProductCategoriesService,
    ProductItemsService,
    ShoppingCartService,
    OrderService,
    StoragesService,
    ReportsService,
    AlertService,
    { provide: HTTP_INTERCEPTORS, useClass: HttpOptionsInterceptor, multi: true }
],
  bootstrap: [AppComponent]
})
export class AppModule { }
