import 'rxjs/add/operator/toPromise';
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { ProductCategory } from '../models/product-category';
import { ProductSubCategory } from '../models/product-sub-category';

@Injectable()
export class ProductCategoriesService {
    constructor(private http: HttpClient) { 
    }

    get categories(): Promise<ProductCategory[]> {
        return this.http.get<ProductCategory[]>('ProductCategories').toPromise();
    }
    
    get subCategories(): Promise<ProductSubCategory[]> {
        return this.http.get<ProductSubCategory[]>('ProductCategories/sub').toPromise();
    }

    public async getCategory(id: number): Promise<ProductCategory> {
        return await this.http.get<ProductCategory>(`ProductCategories/${id}`).toPromise();
    }
    
    public async getSubCategory(id: number): Promise<ProductSubCategory> {
        return await this.http.get<ProductSubCategory>(`ProductCategories/sub/${id}`).toPromise();
    }

    public async add(category: ProductCategory) {
        await this.http.post(`ProductCategories`, category).toPromise();
    }

    public async addSub(subCategory: ProductSubCategory) {
        await this.http.post(`ProductCategories/sub`, subCategory).toPromise();
    }
}