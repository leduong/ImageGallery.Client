﻿import { Injectable } from '@angular/core';
import { Http, Headers, Response, RequestOptions } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';

import { IGalleryIndexViewModel, IEditImageViewModel } from './shared/interfaces';
import { HttpClient } from '@angular/common/http';
import { HttpHeaders } from '@angular/common/http';

@Injectable()
export class GalleryService {

    baseUrl: string = '/api/images';

    constructor(private http: HttpClient) {
    }

    public getGalleryIndexViewModel(): Observable<IGalleryIndexViewModel> {
        return this.http.get<IGalleryIndexViewModel>(this.baseUrl)
            .catch(this.handleError);
    }

    public getEditImageViewModel(id: string): Observable<IEditImageViewModel> {
        return this.http.get<IEditImageViewModel>(`${this.baseUrl}/${id}`)
            .catch(this.handleError);
    }

    public postEditImageViewModel(model: IEditImageViewModel): Observable<Object> {
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        return this.http.post(this.baseUrl, JSON.stringify(model), { headers: headers })
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