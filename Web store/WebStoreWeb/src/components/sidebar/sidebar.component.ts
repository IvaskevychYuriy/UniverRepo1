import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';
import { ProductCategoriesService } from '../../services/product-categories.service';
import { ProductCategory } from '../../models/product-category';
import { ProductSubCategory } from '../../models/product-sub-category';

@Component({
    moduleId: module.id.toString(),
    selector: 'sidebar',
    templateUrl: 'sidebar.component.html',
    styleUrls: ['sidebar.component.css']
})
export class SidebarComponent implements OnInit{
    public categories: ProductCategory[] = [];

    constructor(
        private router: Router,
        private categoriesService: ProductCategoriesService
    ) { }
    
    async ngOnInit() {
        this.categories = await this.categoriesService.categories;
    }

    private selectCategory(category: ProductCategory): void {
        if (!category || !category.id) {
            return;
        }

        this.router.navigate(['/home'], { queryParams: { categoryId: category.id } });
    }
    
    private selectSubCategory(subCategory: ProductSubCategory): void {
        if (!subCategory || !subCategory.id) {
            return;
        }

        this.router.navigate(['/home'], { queryParams: { subCategoryId: subCategory.id } });
    }
}