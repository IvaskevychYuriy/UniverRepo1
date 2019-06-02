
import { Injectable } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { DronesAddModel } from '../models/drones-add-model';

@Injectable()
export class DronesService {
    constructor(private http: HttpClient) { 
    }
    
    public async add(model: DronesAddModel) {
        await this.http.post(`Drones`, model).toPromise();
    }

    public async delete(id: number) {
        await this.http.delete(`Drones/${id}`).toPromise();
    }
}