import { Component, OnInit } from '@angular/core';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { OrderService } from '../../services/order.service';
import { CartItem } from '../../models/cart-item';
import { Order } from '../../models/order';
import { AuthenticationService } from '../../services/authentication.service';
import { Router } from '@angular/router';
import { DataSource, CollectionViewer } from '@angular/cdk/collections';
import { Observable } from 'rxjs';
import { AlertService } from '../../services/alert.service';
import { AddressCoordinates } from '../../models/address-coordinates';

@Component({
    moduleId: module.id.toString(),
    selector: 'shopping-cart',
    templateUrl: 'shopping-cart.component.html',
    styleUrls: ['shopping-cart.component.css']
})
export class ShoppingCartComponent implements OnInit{
    private order: Order;
    private dataSource: OrderProductsDataSource;
    private displayedColumns: string[];

    private destinationCoordinates: AddressCoordinates;
    private initialMapCoords: AddressCoordinates;

    constructor(
        private cartService: ShoppingCartService,
        private orderService: OrderService,
        private authService: AuthenticationService,
        private alert: AlertService,
        private router: Router) {

        this.displayedColumns = ["name", "quantity", "price"];
        this.initialMapCoords = {
            latitude: 51.5,
            longitude: 0
        };
    }
    
    async ngOnInit() {
        const order = new Order();
        order.cartItems = this.cartService.allItems;

        try {
            this.order = await this.orderService.calculateOrderInfo(order);
        } catch (e) {
            this.cartService.clearCart();
        }
        
        this.dataSource = new OrderProductsDataSource(this.order);
    }

    get nothingAdded(): boolean {
        return !this.order || !this.order.cartItems || this.order.cartItems.length < 1
    }

    private clearCart() {
        this.cartService.clearCart();
        this.order = null;
    }
    
    private removeCartItem(id: number): void {
        this.cartService.removeItem(id);
        const index = this.order.cartItems.findIndex(o => o.product.id === id);
        if (index === -1) {
            return;
        }

        this.order.totalPrice -= this.order.cartItems[index].product.price;
        this.order.cartItems.splice(index, 1);
    }

    private async submitOrder() {
        if(!this.order || !this.order.cartItems || this.order.cartItems.length < 1) {
            return;
        }

        if(!this.authService.userProfile) {
            this.alert.info("Please, login to buy");
            return;
        }

        if(!this.destinationCoordinates) {
            this.alert.info("Please, select a destination address");
            return;
        }

        try {
            this.order.coordinates = this.destinationCoordinates;
            const result = await this.orderService.createOrder(this.order);
            this.alert.info(`Order successfully created`);
            this.cartService.clearCart();
            this.router.navigate(['/home']);
        } catch (e) {
            this.alert.info(`An error has occured. Please try again later`);
        }
    }

    private mapClicked(event: any) {
        this.destinationCoordinates = {
            latitude: event.coords.lat,
            longitude: event.coords.lng
        };
    }
}

class OrderProductsDataSource extends DataSource<CartItem> {
    private order: Order;

    constructor(order: Order) {
        super();
        this.order = order;
    }

    connect(collectionViewer: CollectionViewer): Observable<CartItem[]> {
        return Observable.of(this.order.cartItems);
    }

    disconnect(collectionViewer: CollectionViewer): void {
    }
}