import { Component, OnInit } from '@angular/core';
import { OrderService } from '../../services/order.service';
import { Order } from '../../models/order';
import { DataSource, CollectionViewer } from '@angular/cdk/collections';
import { Observable } from 'rxjs';

@Component({
    moduleId: module.id.toString(),
    selector: 'order-history',
    templateUrl: 'order-history.component.html',
    styleUrls: ['order-history.component.css']
})
export class OrderHistoryComponent implements OnInit {
    private orders: Order[];
    private dataSource: OrdersDataSource;
    private displayedColumns: string[];

    constructor(
        private orderService: OrderService
    ) { 
        this.displayedColumns = ["name", "state", "price"];
    }
    
    ngOnInit(): void {
        this.orderService.getAllOrders()
            .then(result => 
            {
                this.orders = result;
                this.dataSource = new OrdersDataSource(this.orders);
            })
    }

    get anyOrder(): boolean {
        return this.orders && this.orders.length > 0;
    }
}

class OrdersDataSource extends DataSource<Order> {
    private orders: Order[];

    constructor(orders: Order[]) {
        super();
        this.orders = orders;
    }

    connect(collectionViewer: CollectionViewer): Observable<Order[]> {
        return Observable.of(this.orders);
    }

    disconnect(collectionViewer: CollectionViewer): void {
    }
}