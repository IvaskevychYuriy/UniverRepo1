import { ProductSubCategory } from "./product-sub-category";

export class ProductCategory {
    public id: number;
    public name : string;

    public subCategories: ProductSubCategory[];
}