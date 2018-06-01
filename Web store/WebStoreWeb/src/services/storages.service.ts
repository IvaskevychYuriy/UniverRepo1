import 'rxjs/add/operator/toPromise';
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Storage } from '../models/storage';
import { StorageItem } from '../models/storage-item';
import { DronesAddModel } from '../models/drones-add-model';

@Injectable()
export class StoragesService {
    constructor(private http: HttpClient) { 
    }

    public async storages(): Promise<Storage[]> {
        return await this.http.get<Storage[]>('Storages').toPromise();
    }
    
    public async storage(id: number): Promise<Storage> {
        return await this.http.get<Storage>(`Storages/${id}`).toPromise();
    }

    public async add(storage: Storage) {
        await this.http.post(`Storages`, storage).toPromise();
    }
    
    public async addItem(item: StorageItem) {
        await this.http.post(`Storages/item`, item).toPromise();
    }
    
    public async addDrones(model: DronesAddModel) {
        await this.http.post(`Storages/drones`, model).toPromise();
    }

    public async delete(id: number) {
        await this.http.delete(`Storages/${id}`).toPromise();
    }

    public async deleteItem(id: number) {
        await this.http.delete(`Storages/item/${id}`).toPromise();
    }
}