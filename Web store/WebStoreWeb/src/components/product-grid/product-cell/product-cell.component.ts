import { Component, Input } from '@angular/core';
import { ProductItem } from '../../../models/product-item';

@Component({
    moduleId: module.id.toString(),
    selector: 'product-cell',
    templateUrl: 'product-cell.component.html',
    styleUrls: ['product-cell.component.css']
})
export class ProductCellComponent {
    @Input() product: ProductItem;

    public buyAmount: number = 1;
}