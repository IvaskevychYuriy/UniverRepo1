import { Component, OnInit } from '@angular/core';
import { TotalReport } from '../../models/total-report';
import { ProductItemReport } from '../../models/product-item-report';
import { ReportsService } from '../../services/reports.service';
import { DataSource } from '@angular/cdk/table';
import { CollectionViewer } from '@angular/cdk/collections';
import { Observable, of } from 'rxjs';

@Component({
  selector: 'reports',
  templateUrl: './reports.component.html',
  styleUrls: ['./reports.component.css']
})
export class ReportsComponent implements OnInit {
  private isLoading: boolean;
  private totalReport: TotalReport;
  private itemsReports: ProductItemReport[];
  private dataSource: ItemsReportsDataSource;
  private displayedColumns: string[];

  constructor(
    private reportsService: ReportsService
  ) { 
    this.displayedColumns = ["name", "sold", "cost", "income", "profit"];
  }

  async ngOnInit() {
    this.isLoading = true;
    this.itemsReports = await this.reportsService.productsReport();
    this.totalReport = await this.reportsService.totalReport();
    this.dataSource = new ItemsReportsDataSource(this.itemsReports);
    this.isLoading = false;
  }
}

class ItemsReportsDataSource extends DataSource<ProductItemReport> {
  private reports: ProductItemReport[];

  constructor(reports: ProductItemReport[]) {
      super();
      this.reports = reports;
  }

  connect(collectionViewer: CollectionViewer): Observable<ProductItemReport[]> {
      return of(this.reports);
  }

  disconnect(collectionViewer: CollectionViewer): void {
  }
}
