import { Component, Input, EventEmitter, Output } from '@angular/core';
import { ProductItem } from '../../../models/product-item';
import { ShoppingCartService } from '../../../services/shopping-cart.service';
import { AlertService } from '../../../services/alert.service';
import { AuthenticationService } from '../../../services/authentication.service';

@Component({
    moduleId: module.id.toString(),
    selector: 'product-cell',
    templateUrl: 'product-cell.component.html',
    styleUrls: ['product-cell.component.css']
})
export class ProductCellComponent {
    @Input() product: ProductItem;
    @Output() itemDeleted = new EventEmitter<ProductItem>();
    
    public buyAmount: number = 1;

    constructor(
        private cartService: ShoppingCartService,
        private authService: AuthenticationService,
        private alert: AlertService) {
    }

    addToCart() {
        this.cartService.addItem(this.product, this.buyAmount);
        this.alert.info("Item(s) added successfully")
    }

    async deleteProduct(product: ProductItem) {
        this.itemDeleted.emit(product);
    }
}