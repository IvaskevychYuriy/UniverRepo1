import { Component, OnInit } from '@angular/core';

import { AlertService } from '../../services/alert.service';
import 'rxjs/add/operator/toPromise';
import { HttpClient } from '@angular/common/http';

@Component({
    moduleId: module.id.toString(),
    templateUrl: 'home.component.html'
})

export class HomeComponent implements OnInit {
    private value: string;

    constructor(
        private http: HttpClient,
        private alertService: AlertService) { }

    ngOnInit() {
        this.http.get('Test/1')
            .toPromise()
            .then(response => this.value = response as string)
            .catch(error => this.alertService.error(error));
    }
}