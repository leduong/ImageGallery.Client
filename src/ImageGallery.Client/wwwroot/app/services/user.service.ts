import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

@Injectable()
export class UserManagementService {
    private apiEndpoint = '';

    constructor(
        private http: Http,
    ) {
        this.getConfig().subscribe(res => {
            this.apiEndpoint = res.json().clientConfiguration.apiUserManagementUri;
        });
    }

    getConfig() {
        return this.http.get('api/ClientAppSettings');
    }

    getUserInfo() {
        return this.http.get(`${this.apiEndpoint}/api/UserProfile`);
    }

    resetPassword(email) {
        return this.http.post(`${this.apiEndpoint}/api/Account`, email);
    }

    validatePassword(password) {
        return this.http.post(`${this.apiEndpoint}/api/Account/ValidatePassword`, password);
    }

    createPassword(password) {
        return this.http.post(`${this.apiEndpoint}/api/Account/CreatePassword`, password);
    }
}