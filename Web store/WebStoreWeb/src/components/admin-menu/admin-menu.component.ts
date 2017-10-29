import { Component, OnInit } from '@angular/core';

import 'rxjs/add/operator/toPromise';
import { AlertService } from '../../services/alert.service';
import { HttpClient } from '@angular/common/http';
import { ProductItem } from '../../models/product-item';
import { ProductCategory } from '../../models/product-category';
import { ProductCategoriesService } from '../../services/product-categories.service';
import { ProductItemsService } from '../../services/product-items.service';

@Component({
    moduleId: module.id.toString(),
    selector: 'admin-menu',
    templateUrl: 'admin-menu.component.html',
    styleUrls: ['admin-menu.component.css']
})
export class AdminMenuComponent implements OnInit {
    private newProduct: ProductItem;
    private newCategory: ProductCategory;
    private categories: ProductCategory[] = [];

    constructor(
        private http: HttpClient,
        private alertService: AlertService,
        private categoriesService: ProductCategoriesService,
        private itemsService: ProductItemsService) { 
        this.newProduct = new ProductItem();
        this.newCategory = new ProductCategory();
    }

    private fetchCategories(): void {
        this.categoriesService.categories
            .then(data => this.categories = data);
    }
    
    ngOnInit(): void {
        this.fetchCategories();
    }

    addNewProduct() {
        console.log(JSON.stringify(this.newProduct));
        this.itemsService.add(this.newProduct)
            .then(data => console.log(data))
            .catch(err => console.log(err));
    }

    addNewCategory() {
        this.categoriesService.add(this.newCategory)
            .then(data => this.fetchCategories())
            .catch(err => this.fetchCategories());
    }
}