import { StorageItem } from "./storage-item";
import { Drone } from "./drone";
import { AddressCoordinates } from "./address-coordinates";

export class Storage {
    public id: number;
    public name: string;
    public coordinates: AddressCoordinates;
    
    public items: StorageItem[];
    public drones: Drone[];
}