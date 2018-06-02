import { Injectable } from '@angular/core';
import { Headers, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/toPromise';
import { LoginInfo } from '../models/login-info';
import { UserProfile } from '../models/user-profile';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class AuthenticationService {
    private _userProfile: UserProfile;

    get userProfile(): UserProfile {
        return this._userProfile;
    }
    
    constructor(private http: HttpClient) { 
        this._userProfile = JSON.parse(localStorage.getItem("currentUser"));
    }

    login(model: LoginInfo) {
        return this.http.post('account/login', JSON.stringify(model))
            .map((response) => {
                this._userProfile = response as UserProfile;
                localStorage.setItem('currentUser', JSON.stringify(this._userProfile));
            });
    }
    
    register(model: LoginInfo) {
        return this.http.post('account/register', JSON.stringify(model))
            .map((response) => {
                this._userProfile = response as UserProfile;
                localStorage.setItem('currentUser', JSON.stringify(this._userProfile));
            });
    }

    logout() {
        this._userProfile = null;
        localStorage.removeItem('currentUser');
        
        return this.http.post('account/logout', null)
            .toPromise();
    }
    
    isInRole(role: string): boolean {
        return this.userProfile 
            && this.userProfile.roleNames
            && this.userProfile.roleNames.indexOf(role) !== -1;
    }
}