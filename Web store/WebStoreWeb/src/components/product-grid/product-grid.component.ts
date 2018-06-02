import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { ProductItem } from '../../models/product-item';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductItemsService } from '../../services/product-items.service';
import { Subscription } from 'rxjs';
import { PageData } from '../../models/page-data';
import { AlertService } from '../../services/alert.service';

@Component({
    moduleId: module.id.toString(),
    selector: 'product-grid',
    templateUrl: 'product-grid.component.html',
    styleUrls: ['product-grid.component.css']
})
export class ProductGridComponent implements OnInit, OnDestroy {
    private pageData: PageData;
    private itemsPerPage: number = 12;
    private currentPage: number = 1;
    private categoryId: number;
    private subCategoryId: number;
    private subscription: Subscription

    constructor (
        private route: ActivatedRoute,
        private router: Router,
        private alert: AlertService,
        private productsService: ProductItemsService) {

        this.pageData = new PageData();
    }
    
    ngOnInit(): void {
        this.subscription = this.route.queryParams.subscribe(async (params) => await this.reloadGrid(params));
    }
    
    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }

    get anyItemsOnPage() {
        return this.pageData && this.pageData.productItems && this.pageData.productItems.length;
    }

    private async reloadGrid(queryParams) {
        this.categoryId = queryParams["categoryId"];
        this.subCategoryId = queryParams["subCategoryId"];
        this.currentPage = Math.max(queryParams["page"] || 1, 1);

        try {
            await this.reloadData();
        } catch (e) {
            this.alert.info("Could not fetch items, please try again later");
        }
    }

    private async reloadData() {
        this.pageData = await this.productsService.fetchItems(this.categoryId, this.subCategoryId, this.itemsPerPage, this.currentPage)
    }

    async deleteProduct(product: ProductItem) {
        if (!product || !product.id || product.availableCount < 1) {
            return;
        }

        try {
            await this.productsService.delete(product.id);
            this.alert.info("Product successfully deleted");
        } catch (e) {
            this.alert.info("Couldn't delete product");
        }
        
        await this.reloadData();
    }
}