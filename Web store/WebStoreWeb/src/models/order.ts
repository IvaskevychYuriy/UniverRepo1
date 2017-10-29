import { CartItem } from "./cart-item";
import { ProductItem } from "./product-item";
import { OrderStates } from "./enumerations/order-states";

export class Order {
    id: number;
    cartItems: CartItem[];
    state: OrderStates;
    totalPrice: number;
}