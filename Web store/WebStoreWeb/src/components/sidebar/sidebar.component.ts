import { Component, OnInit } from '@angular/core';

import { Router } from '@angular/router';
import { ProductCategoriesService } from '../../services/product-categories.service';
import { ProductCategory } from '../../models/product-category';

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
    
    ngOnInit(): void {
        this.categoriesService.categories
            .then(data => this.categories = data);
    }

    private selectCategory(category: ProductCategory): void {
        if (!category || !category.id) {
            return;
        }

        this.router.navigate(['/home'], { queryParams: { categoryId: category.id } });
    }
}