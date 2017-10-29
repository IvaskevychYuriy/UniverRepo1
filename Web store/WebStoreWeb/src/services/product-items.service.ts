import 'rxjs/add/operator/toPromise';
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ProductItem } from '../models/product-item';
import { PageData } from '../models/page-data';

@Injectable()
export class ProductItemsService {
    constructor(private http: HttpClient) { 
    }

    public fetchItems(categoryId: number = null, pageSize: number = null, page: number = null): Promise<PageData> {
        return this.http.get(`ProductItems/Get?categoryId=${categoryId}&pageSize=${pageSize}&page=${page}`)
            .toPromise()
            .then(data => data as PageData);
    }

    public getItem(id: number): Promise<ProductItem> {
        return this.http.get(`ProductItems/${id}`)
            .toPromise()
            .then(data => data as ProductItem);
    }

    public add(item: ProductItem): Promise<ProductItem> {
        return this.http.post(`ProductItems`, JSON.stringify(item))
            .toPromise()
            .then(data => data as ProductItem);
    }
}