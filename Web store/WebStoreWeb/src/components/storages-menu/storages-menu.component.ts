import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ProductItemsService } from '../../services/product-items.service';
import { ProductItem } from '../../models/product-item';
import { StorageItem } from '../../models/storage-item';
import { StoragesService } from '../../services/storages.service';
import { Storage } from '../../models/storage';
import { DataSource } from '@angular/cdk/table';
import { CollectionViewer } from '@angular/cdk/collections';

@Component({
  moduleId: module.id.toString(),
  selector: 'storages-menu',
  templateUrl: 'storages-menu.component.html',
  styleUrls: ['storages-menu.component.css']
})
export class StoragesMenuComponent implements OnInit {

  private dataSource: StorageItemsDataSource;
  private displayedColumns: string[];

  private storages: Storage[] = [];
  private products: ProductItem[] = [];

  private selectedStorage: Storage;
  private newStorage: Storage;
  private newStorageItem: StorageItem;

  constructor(
    private http: HttpClient,
    private storagesService: StoragesService,
    private itemsService: ProductItemsService) {
  
    this.displayedColumns = ["name", "price", "quantity"];

    this.selectedStorage = new Storage();
    this.selectedStorage.id = 0;

    this.newStorage = new Storage();
    this.newStorageItem = new StorageItem();
  }

  async ngOnInit() {
    await this.fetchStorages();
    await this.fetchProducts();
  }

  async addNewStorage() {
    try {
      await this.storagesService.add(this.newStorage);
    } catch (e) {
      // TODO: add a toast
    }

    await this.fetchStorages();
  }

  async addNewStorageItem() {
    try {
      await this.storagesService.addItem(this.newStorageItem);
    } catch (e) {
      // TODO: add a toast
    }

    await this.fetchProducts();
    await this.loadStorage(this.selectedStorage);
  }

  private async fetchStorages() {
    this.storages = await this.storagesService.storages();
  }

  private async fetchProducts() {
    const data = await this.itemsService.fetchItems();
    this.products = data.productItems;
  }

  private async loadStorage(storage: Storage) {
    this.selectedStorage = await this.storagesService.storage(storage.id);
    console.log(this.selectedStorage);
    this.newStorageItem.storageId = storage.id;
    this.dataSource = new StorageItemsDataSource(this.selectedStorage.items);
  }
}

class StorageItemsDataSource extends DataSource<StorageItem> {
  private storageItems: StorageItem[];

  constructor(storageItems: StorageItem[]) {
      super();
      this.storageItems = storageItems;
  }

  connect(collectionViewer: CollectionViewer): Observable<StorageItem[]> {
      return Observable.of(this.storageItems);
  }

  disconnect(collectionViewer: CollectionViewer): void {
  }
}