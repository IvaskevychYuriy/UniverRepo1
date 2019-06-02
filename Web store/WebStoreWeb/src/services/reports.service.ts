
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { DatesRangeFilter } from '../models/dates-range-filter';
import { ProductItemReport } from '../models/product-item-report';
import { TotalReport } from '../models/total-report';

@Injectable()
export class ReportsService {
    constructor(private http: HttpClient) { 
    }

    public async productsReport(filter: DatesRangeFilter = null): Promise<ProductItemReport[]> {
        filter = filter || new DatesRangeFilter();
        return await this.http.get<ProductItemReport[]>(`reports/products?from=${filter.from}&to=${filter.to}`).toPromise();
    }
    
    public async totalReport(filter: DatesRangeFilter = null): Promise<TotalReport> {
        filter = filter || new DatesRangeFilter();
        return await this.http.get<TotalReport>(`reports/total?from=${filter.from}&to=${filter.to}`).toPromise();
    }
}