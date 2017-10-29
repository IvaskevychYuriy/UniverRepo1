import { ProductItem } from "../models/product-item";
import { CartItem } from "../models/cart-item";
import { Injectable } from "@angular/core";

@Injectable()
export class ShoppingCartService {
    private storageItemName: string = "cart-data";

    public addItem(product: ProductItem, quantity: number) {
        if (quantity <= 0) {
            return;
        }

        let items = this.loadItemsData() || [];
        const item = items.find(it => it.product.id === product.id);
        if (item) {
            item.quantity += quantity;
        } else {
            items.push(new CartItem(product.id, quantity));
        }

        this.saveItemsData(items);
    }

    public removeItem(productId: number) {
        let items = this.loadItemsData() || [];
        const index = items.findIndex(it => it.product.id === productId);
        if (index !== -1) {
            items.splice(index, 1);
            this.saveItemsData(items);
        }
    }

    get allItems(): CartItem[] {
        return this.loadItemsData();
    }

    public clearCart(): void {
        localStorage.removeItem(this.storageItemName);
    }

    private loadItemsData(): CartItem[] {
        const data = localStorage.getItem(this.storageItemName);
        if (!data || data.length < 1) {
            return [];
        }

        const result = JSON.parse(data) as CartItem[];
        return result ? result : [];
    }

    private saveItemsData(items: CartItem[]): void {
        localStorage.setItem(this.storageItemName, JSON.stringify(items));
    }
}