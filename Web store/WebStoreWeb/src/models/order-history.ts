import { OrderStates } from "./enumerations/order-states";

export class OrderHistory {
    public id: number;
    public state: OrderStates;
    public stateChangeDate: Date;
}