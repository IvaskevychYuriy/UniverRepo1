import { Component, Input } from '@angular/core';
import { ProductItem } from '../../../models/product-item';
import { ShoppingCartService } from '../../../services/shopping-cart.service';
import { AlertService } from '../../../services/alert.service';

@Component({
    moduleId: module.id.toString(),
    selector: 'product-cell',
    templateUrl: 'product-cell.component.html',
    styleUrls: ['product-cell.component.css']
})
export class ProductCellComponent {
    @Input() product: ProductItem;
    public buyAmount: number = 1;

    constructor(
        private cartService: ShoppingCartService,
        private alert: AlertService) {
    }

    public addToCart() {
        this.cartService.addItem(this.product, this.buyAmount);
        this.alert.info("Item(s) added successfully")
    }
}