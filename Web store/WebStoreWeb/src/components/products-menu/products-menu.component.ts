import { Component, OnInit } from '@angular/core';

import { HttpClient } from '@angular/common/http';
import { ProductItem } from '../../models/product-item';
import { ProductCategory } from '../../models/product-category';
import { ProductCategoriesService } from '../../services/product-categories.service';
import { ProductItemsService } from '../../services/product-items.service';
import { ProductSubCategory } from '../../models/product-sub-category';
import { AlertService } from '../../services/alert.service';

@Component({
    moduleId: module.id.toString(),
    selector: 'products-menu',
    templateUrl: 'products-menu.component.html',
    styleUrls: ['products-menu.component.css']
})
export class ProductsMenuComponent implements OnInit {
    private newProduct: ProductItem;
    private newCategory: ProductCategory;
    private newSubCategory: ProductSubCategory;

    private categories: ProductCategory[] = [];
    private subCategories: ProductSubCategory[] = [];

    constructor(
        private http: HttpClient,
        private categoriesService: ProductCategoriesService,
        private alert: AlertService,
        private itemsService: ProductItemsService) { 

        this.newProduct = new ProductItem();
        this.newCategory = new ProductCategory();
        this.newSubCategory = new ProductSubCategory();
    }

    private async fetchCategories() {
        this.categories = await this.categoriesService.categories;
    }
    
    private async fetchSubCategories() {
        this.subCategories = await this.categoriesService.subCategories;
    }
    
    async ngOnInit() {
        await this.fetchCategories();
        await this.fetchSubCategories();
    }

    async addNewProduct() {
        try {
            await this.itemsService.add(this.newProduct);
        } catch (e) {
            this.alert.info("Couldn't add new product");
        }
    }

    async addNewCategory() {
        try {
            await this.categoriesService.add(this.newCategory);
        } catch (e) {
            this.alert.info("Couldn't add new category");
        }
        
        await this.fetchCategories();
    }
    
    async addNewSubCategory() {
        try {
            await this.categoriesService.addSub(this.newSubCategory);
        } catch (e) {
            this.alert.info("Couldn't add new subcategory");
        }
        
        await this.fetchSubCategories();
    }
}