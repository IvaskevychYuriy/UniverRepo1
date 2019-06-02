import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Order } from "../models/order";


@Injectable()
export class OrderService {
    constructor(private http: HttpClient){
    }

    public async calculateOrderInfo(order: Order): Promise<Order> {
        return await this.http.post<Order>('orders/calculate', order).toPromise();
    }

    public async getOrder(id: number): Promise<Order> {
        return await this.http.get<Order>(`orders/get/${id}`).toPromise();
    }

    public async getAllOrders(): Promise<Order[]> {
        return await this.http.get<Order[]>('orders').toPromise();
    }
    
    public async createOrder(order: Order): Promise<Order> {
        return await this.http.post<Order>('orders', order).toPromise();
    }
}