import { Component, Input, OnInit, OnDestroy } from '@angular/core';
import { ProductItem } from '../../models/product-item';
import { ActivatedRoute, Router } from '@angular/router';
import { ProductItemsService } from '../../services/product-items.service';
import { AlertService } from '../../services/alert.service';
import { Subscription } from 'rxjs';
import { PageData } from '../../models/page-data';

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
    private subscription: Subscription

    constructor (
        private route: ActivatedRoute,
        private router: Router,
        private productsService: ProductItemsService,
        private notifier: AlertService) {
        this.pageData = new PageData();
    }
    
    ngOnInit(): void {
        this.subscription = this.route.queryParams.subscribe(params => this.reloadGrid(params));
    }
    
    ngOnDestroy(): void {
        this.subscription.unsubscribe();
    }

    get anyItemsOnPage() {
        return this.pageData && this.pageData.productItems && this.pageData.productItems.length;
    }

    private reloadGrid(queryParams) {
        this.categoryId = queryParams["categoryId"];
        this.currentPage = Math.max(queryParams["page"] || 1, 1);

        this.productsService.fetchItems(this.categoryId, this.itemsPerPage, this.currentPage)
            .then(data => this.pageData = data)
            .catch(err => this.notifier.error("Could not fetch items, please try again later"));
    }
}