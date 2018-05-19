import 'rxjs/add/operator/toPromise';
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ProductItem } from '../models/product-item';
import { PageData } from '../models/page-data';

@Injectable()
export class ProductItemsService {
    constructor(private http: HttpClient) { 
    }

    public async fetchItems(categoryId: number = null, subCategoryId: number = null, pageSize: number = null, page: number = null): Promise<PageData> {
        return await this.http.get<PageData>(`ProductItems/Get?categoryId=${categoryId}&subCategoryId=${subCategoryId}&pageSize=${pageSize}&page=${page}`).toPromise();
    }

    public async getItem(id: number): Promise<ProductItem> {
        return await this.http.get<ProductItem>(`ProductItems/${id}`).toPromise();
    }

    public async add(item: ProductItem): Promise<ProductItem> {
        return await this.http.post<ProductItem>(`ProductItems`, item).toPromise();
    }
}