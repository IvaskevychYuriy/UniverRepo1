import { CartItem } from "./cart-item";
import { OrderStates } from "./enumerations/order-states";
import { OrderHistory } from "./order-history";

export class Order {
    public id: number;
    public totalPrice: number;

    public cartItems: CartItem[];
    public historyRecords: OrderHistory[];
}