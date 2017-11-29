import 'rxjs/add/operator/toPromise';
import { Injectable } from "@angular/core";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { UselessTask } from '../models/useless-task';

@Injectable()
export class UselessTasksService {
    constructor(private http: HttpClient) { 
    }

    public getProgress(id: number, serverId: string): Promise<number> {
        return this.http.get(`Useless/Progress?id=${id}`, { headers: new HttpHeaders().set('Server-Identifier', serverId) })
            .toPromise()
            .then(data => data as number);
    }

    public get(id: number, serverId: string): Promise<UselessTask> {
        return this.http.get(`Useless/Get?id=${id}`, { headers: new HttpHeaders().set('Server-Identifier', serverId) })
            .toPromise()
            .then(data => data as UselessTask);
    }

    public add(workAmount: number): Promise<UselessTask> {
        return this.http.post(`Useless/New?workAmount=${workAmount}`, null)
            .toPromise()
            .then(data => data as UselessTask);
    }

    public cancel(id: number, serverId: string): Promise<Object> {
        return this.http.post(`Useless/Cancel?id=${id}`, null, { headers: new HttpHeaders().set('Server-Identifier', serverId) })
            .toPromise()
    }
}