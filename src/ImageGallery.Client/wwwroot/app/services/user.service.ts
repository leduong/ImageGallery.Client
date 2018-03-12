import { Injectable } from '@angular/core';
import { Response } from '@angular/http';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';

import { IUserProfileViewModel } from '../shared/interfaces';

@Injectable()
export class UserManagementService {

    private baseUrl: string = '/api/UserProfile';

    constructor(
        private httpClient: HttpClient,
    ) {

    }

    public getUserInfo() {
        return this.httpClient.get<IUserProfileViewModel>(this.baseUrl)
            .catch(this.handleError);
    }

    private handleError(error: any) {
        console.error('server error:', error);
        if (error instanceof Response) {
            let errMessage = '';
            try {
                errMessage = error.json().error;
            } catch (err) {
                errMessage = error.statusText;
            }
            return Observable.throw(errMessage);
            // Use the following instead if using lite-server
            //return Observable.throw(err.text() || 'backend server error');
        }
        return Observable.throw(error || 'ASP.NET Core server error');
    }
}