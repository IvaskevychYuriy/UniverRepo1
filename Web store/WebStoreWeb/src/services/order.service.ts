import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { CartItem } from "../models/cart-item";
import { Order } from "../models/order";
import 'rxjs/add/operator/toPromise';

@Injectable()
export class OrderService {
    constructor(private http: HttpClient){
    }

    public calculateOrderInfo(order: Order): Promise<Order> {
        return this.http.post<Order>('orders/calculate', JSON.stringify(order))
            .toPromise();
    }

    public getOrder(id: number): Promise<Order> {
        return this.http.get<Order>(`orders/get/${id}`)
            .toPromise();
    }

    public getAllOrders(): Promise<Order[]> {
        return this.http.get<Order[]>('orders')
            .toPromise();
    }
    
    public createOrder(order: Order): Promise<Order> {
        return this.http.post<Order>('orders', JSON.stringify(order))
            .toPromise();
    }
}