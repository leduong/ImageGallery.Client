import { Injectable } from '@angular/core';
import { Http } from '@angular/http';

@Injectable()
export class UserManagementService {
    constructor(
        private http: Http,
    ) {

    }

    getUserInfo() {
        let url = 'https://user-management.informationcart.com/api/UserProfile';
        return this.http.get(url);
    }

    resetPassword(email) {
        let url = 'https://user-management.informationcart.com/api/Account';
        return this.http.post(url, email);
    }

    validatePassword(password) {
        let url = 'https://user-management.informationcart.com/api/Account/ValidatePassword';
        return this.http.post(url, password);
    }

    createPassword(password) {
        let url = 'https://user-management.informationcart.com/api/Account/CreatePassword';
        return this.http.post(url, password);
    }
}