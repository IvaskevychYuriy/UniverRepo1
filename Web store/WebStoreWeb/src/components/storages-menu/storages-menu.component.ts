import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { of } from 'rxjs';
import { ProductItemsService } from '../../services/product-items.service';
import { ProductItem } from '../../models/product-item';
import { StorageItem } from '../../models/storage-item';
import { StoragesService } from '../../services/storages.service';
import { Storage } from '../../models/storage';
import { DataSource } from '@angular/cdk/table';
import { CollectionViewer } from '@angular/cdk/collections';
import { AlertService } from '../../services/alert.service';
import { Drone } from '../../models/drone';
import { DronesAddModel } from '../../models/drones-add-model';
import { DroneStates } from '../../models/enumerations/drone-states';
import { AddressCoordinates } from '../../models/address-coordinates';
import { DronesService } from '../../services/drones.service';
import { StorageEditModel } from '../../models/storage-edit-model';

@Component({
  moduleId: module.id.toString(),
  selector: 'storages-menu',
  templateUrl: 'storages-menu.component.html',
  styleUrls: ['storages-menu.component.css']
})
export class StoragesMenuComponent implements OnInit {

  private dataSource: StorageItemsDataSource;
  private displayedColumns: string[];
  private dronesDataSource: DronesDataSource;
  private dronesDisplayedColumns: string[];

  private storages: Storage[] = [];
  private products: ProductItem[] = [];

  private initialMapCoords: AddressCoordinates;
  private selectedStorage: Storage;
  private editingStorage: StorageEditModel;

  private newStorage: Storage;
  private newStorageItem: StorageItem;
  private newDronesModel: DronesAddModel;

  constructor(
    private http: HttpClient,
    private alert: AlertService,
    private storagesService: StoragesService,
    private itemsService: ProductItemsService,
    private dronesService: DronesService) {
  
    this.displayedColumns = ["name", "price", "quantity", "actions"];
    this.dronesDisplayedColumns = ["id", "state", "arrivalTime", "actions"];

    this.initialMapCoords = {
      latitude: 51.5,
      longitude: 0
    };
    this.selectedStorage = new Storage();
    this.selectedStorage.id = 0;
    this.editingStorage = null;

    this.newStorage = new Storage();
    this.newStorageItem = new StorageItem();
    this.newDronesModel = new DronesAddModel();
  }

  async ngOnInit() {
    await this.fetchStorages();
    await this.fetchProducts();
  }

  async addNewStorage() {
    if (!this.newStorage.coordinates) {
      this.alert.info("Please, select storage location");
      return;
    }

    try {
      await this.storagesService.add(this.newStorage);
      this.alert.info("New storage added");
    } catch (e) {
      this.alert.info("Couldn't add new storage");
    }

    await this.fetchStorages();
  }

  async addNewStorageItem() {
    try {
      await this.storagesService.addItem(this.newStorageItem);
      this.alert.info("New item(s) added");
    } catch (e) {
      this.alert.info("Couldn't add new storage item");
    }

    await this.fetchProducts();
    await this.loadStorage(this.selectedStorage);
  }

  async addNewDrones() {
    try {
      await this.dronesService.add(this.newDronesModel);
      this.alert.info("New drone(s) added");
    } catch (e) {
      this.alert.info("Couldn't add new drone(s)");
    }

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
    this.newStorageItem.storageId = storage.id;
    this.newDronesModel.storageId = storage.id;
    this.dataSource = new StorageItemsDataSource(this.selectedStorage.items);
    this.dronesDataSource = new DronesDataSource(this.selectedStorage.drones);

    if (this.editingStorage) {
      this.editingStorage = this.mapToEditingStorageModel(this.selectedStorage);
    }
  }

  private droneState(drone: Drone): string {
    return DroneStates[drone.state];
  }

  private droneArrival(drone: Drone): string {
    const date = drone.arrivalTime;
    return date ? new Date(date).toLocaleString() : "";
  }
  
  private storageMapClicked(event: any, storage: Storage | StorageEditModel) {
    if (!storage) {
      return;
    }

    storage.coordinates = {
      latitude: event.coords.lat,
      longitude: event.coords.lng
    };
  }

  async deleteItems(element: StorageItem) {
    if (element.canDelete) {
      this.alert.info("Cannot delete item(s)");
      return;
    }

    try {
      await this.storagesService.deleteItems(this.selectedStorage.id, element.productId);
      this.alert.info("Item(s) successfully deleted");
    } catch (e) {
      this.alert.info("Couldn't delete item(s)");
    }

    await this.loadStorage(this.selectedStorage);
  }

  async deleteDrone(drone: Drone) {
    if (drone.state !== DroneStates.Available) {
      this.alert.info("Cannot delete non-available drone");
    }

    try {
      await this.dronesService.delete(drone.id);
      this.alert.info("Drone successfully deleted");
    } catch (e) {
      this.alert.info("Couldn't delete drone");
    }

    await this.loadStorage(this.selectedStorage);
  }

  async toggleEditStorage() {
    if (this.editingStorage) {
      this.editingStorage = null;
    } else {
      this.editingStorage = this.mapToEditingStorageModel(this.selectedStorage);
    }
  } 

  private mapToEditingStorageModel(storage: Storage): StorageEditModel {
    return {
      id: storage.id,
      name: storage.name,
      coordinates: storage.coordinates
    };
  }

  async editStorage() {
    if (!this.editingStorage) {
      return;
    }
    
    try {
      await this.storagesService.update(this.editingStorage);
      this.alert.info("Successfully updated selected storage");
    } catch (e) {
      this.alert.info("Couldn't edit selected storage");
    }

    await this.loadStorage(this.selectedStorage);
    await this.fetchStorages();
  }
}

class StorageItemsDataSource extends DataSource<StorageItem> {
  private storageItems: StorageItem[];

  constructor(storageItems: StorageItem[]) {
      super();
      this.storageItems = storageItems;
  }

  connect(collectionViewer: CollectionViewer): Observable<StorageItem[]> {
      return of(this.storageItems);
  }

  disconnect(collectionViewer: CollectionViewer): void {
  }
}

class DronesDataSource extends DataSource<Drone> {
  private drones: Drone[];

  constructor(drones: Drone[]) {
      super();
      this.drones = drones;
  }

  connect(collectionViewer: CollectionViewer): Observable<Drone[]> {
      return of(this.drones);
  }

  disconnect(collectionViewer: CollectionViewer): void {
  }
}