import { Component, OnInit } from '@angular/core';
import { ShoppingCartService } from '../../services/shopping-cart.service';
import { OrderService } from '../../services/order.service';
import { CartItem } from '../../models/cart-item';
import { Order } from '../../models/order';
import { AuthenticationService } from '../../services/authentication.service';
import { AlertService } from '../../services/alert.service';
import { Router } from '@angular/router';
import { DataSource, CollectionViewer } from '@angular/cdk/collections';
import { Observable } from 'rxjs';

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

    constructor(
        private cartService: ShoppingCartService,
        private orderService: OrderService,
        private authService: AuthenticationService,
        private alertService: AlertService,
        private router: Router) {
        this.displayedColumns = ["name", "quantity", "price"];
    }
    
    ngOnInit(): void {
        const order = new Order();
        order.cartItems = this.cartService.allItems;
        this.orderService.calculateOrderInfo(order)
            .then(result => 
            {
                this.order = result;
                this.dataSource = new OrderProductsDataSource(this.order);
            })
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

    private submitOrder(): void {
        if(!this.order || !this.order.cartItems || this.order.cartItems.length < 1) {
            return;
        }

        if(!this.authService.userProfile) {
            this.alertService.error("Please, login to buy");
            return;
        }

        this.orderService.createOrder(this.order)
            .then(result => {
                this.alertService.success(`Order #${result.id} successfully created`);
                this.cartService.clearCart();
                this.router.navigate(['/home']);
            })
            .catch(err => this.alertService.error(`An error has occured. Please try again later`));
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