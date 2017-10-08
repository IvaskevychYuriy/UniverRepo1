import { Component, OnInit } from '@angular/core';

import { AlertService } from '../../services/alert.service';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';

@Component({
    moduleId: module.id.toString(),
    templateUrl: 'secret.component.html'
})
export class SecretComponent implements OnInit {
    private values: string[];

    constructor(
        private http: HttpClient,
        private alertService: AlertService) { }

    ngOnInit() {
        this.http.get('Test')
            .toPromise()
            .then(response => this.values = response as string[])
            .catch(error => this.alertService.error(error));
    }
}