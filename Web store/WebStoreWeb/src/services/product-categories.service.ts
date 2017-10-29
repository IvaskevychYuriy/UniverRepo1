import 'rxjs/add/operator/toPromise';
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ProductCategory } from '../models/product-category';

@Injectable()
export class ProductCategoriesService {
    constructor(private http: HttpClient) { 
    }

    get categories(): Promise<ProductCategory[]> {
        return this.http.get('ProductCategories')
            .toPromise()
            .then(data => data as ProductCategory[]);
    }

    public getCategory(id: number): Promise<ProductCategory> {
        return this.http.get(`ProductCategories/${id}`)
            .toPromise()
            .then(data => data as ProductCategory);
    }

    public add(category: ProductCategory): Promise<ProductCategory> {
        return this.http.post(`ProductCategories`, JSON.stringify(category))
            .toPromise()
            .then(data => data as ProductCategory);
    }
}