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
}