import { DroneStates } from "./enumerations/drone-states";

export interface Drone {
    id: number;
    state: DroneStates;
    arrivalTime?: Date;
    maxWeight: number;
    cartItemId: number;
}