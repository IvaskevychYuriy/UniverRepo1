export interface ProductItemReport {
    id: number;
    name: string;

    soldCount: number;
    income: number;
    cost: number;

    profit: number;
    averageProfitPerItem: number;
}