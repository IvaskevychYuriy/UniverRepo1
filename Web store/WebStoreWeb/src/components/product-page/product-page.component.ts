import { Component, OnInit, OnDestroy } from '@angular/core';
import { ProductItemsService } from '../../services/product-items.service';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from '../../services/alert.service';
import { ProductItem } from '../../models/product-item';
import { Subscription } from 'rxjs';
import { ShoppingCartService } from '../../services/shopping-cart.service';

@Component({
  selector: 'product-page',
  templateUrl: './product-page.component.html',
  styleUrls: ['./product-page.component.css']
})
export class ProductPageComponent implements OnInit, OnDestroy {

  private subscription: Subscription;

  private product: ProductItem;
  private buyAmount: number;

  constructor(
    private route: ActivatedRoute,
    private alert: AlertService,
    private productsService: ProductItemsService,
    private cartService: ShoppingCartService) {

    this.buyAmount = 1;
  }

  async ngOnInit() {
    this.subscription = this.route.params.subscribe(async p => await this.loadProduct(p['id']));
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  async loadProduct(productId) {
    try {
      this.product = await this.productsService.getItem(productId);
    } catch (e) {
      this.alert.info("Failed to load product");
    }
  }

  addToCart() {
    this.cartService.addItem(this.product, this.buyAmount);
    this.alert.info("Item(s) added successfully")
  }
}
