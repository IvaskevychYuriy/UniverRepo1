import { ProductItem } from "./product-item";

export class CartItem {
    constructor(id: number, quantity: number) {
        this.product = new ProductItem();
        this.product.id = id;
        this.quantity = quantity;
    }

    public product: ProductItem;
    public quantity: number;
}