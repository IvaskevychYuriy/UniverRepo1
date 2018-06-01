import { CartItem } from "./cart-item";
import { OrderStates } from "./enumerations/order-states";
import { OrderHistory } from "./order-history";
import { AddressCoordinates } from "./address-coordinates";

export class Order {
    public id: number;
    public totalPrice: number;
    public coordinates: AddressCoordinates;

    public cartItems: CartItem[];
    public historyRecords: OrderHistory[];
}