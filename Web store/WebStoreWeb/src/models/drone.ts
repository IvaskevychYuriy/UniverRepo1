import { DroneStates } from "./enumerations/drone-states";

export interface Drone {
    id: number;
    state: DroneStates;
    arrivalTime?: Date;
    cartItemId: number;
}