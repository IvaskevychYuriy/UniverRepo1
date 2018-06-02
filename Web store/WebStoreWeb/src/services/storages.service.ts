import 'rxjs/add/operator/toPromise';
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { Storage } from '../models/storage';
import { StorageItem } from '../models/storage-item';
import { StorageEditModel } from '../models/storage-edit-model';

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

    public async update(storage: StorageEditModel) {
        await this.http.put(`Storages`, storage).toPromise();
    }

    public async delete(id: number) {
        await this.http.delete(`Storages/${id}`).toPromise();
    }

    public async deleteItems(id: number, productId: number) {
        await this.http.delete(`Storages/${id}/items?productId=${productId}`).toPromise();
    }
}