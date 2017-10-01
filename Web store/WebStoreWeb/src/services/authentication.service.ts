import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map'
import { LoginInfo } from '../models/login-info';
import { UserProfile } from '../models/user-profile';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AuthenticationService {
    private _userProfile: UserProfile;

    get userProfile(): UserProfile {
        return this._userProfile;
    }
    
    constructor(private http: HttpClient) { }

    login(model: LoginInfo) {
        return this.http.post('account/login', JSON.stringify(model))
            .map((response) => {
                this._userProfile = response as UserProfile;
            });
    }
    
    register(model: LoginInfo) {
        return this.http.post('account/register', JSON.stringify(model))
            .map((response) => {
                this._userProfile = response as UserProfile;
            });
    }

    logout() {
        return this.http.post('account/logout', null)
            .map((response) => {
                this._userProfile = null;
            });
    }
}