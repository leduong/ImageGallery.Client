import { Injectable } from '@angular/core';
import { Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';

import { IEditImageViewModel, IAddImageViewModel } from './shared/interfaces';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable()
export class GalleryService {

    private baseUrl: string = '/api/images';

    constructor(private httpClient: HttpClient) {
    }

    public getGalleryIndexViewModel(limit: number, page: number) {
        return new Promise((resolve, reject) => {
            this.httpClient.get(`${this.baseUrl}/list?limit=${limit}&page=${page}`, { observe: 'response' })
                .subscribe(res => {
                    resolve({
                        totalCount: res.headers.get('X-InlineCount'),
                        images: res.body
                    });
                });
        });
    }

    public getEditImageViewModel(id: string): Observable<IEditImageViewModel> {
        return this.httpClient.get<IEditImageViewModel>(`${this.baseUrl}/${id}`)
            .catch(this.handleError);
    }

    public postEditImageViewModel(model: IEditImageViewModel): Observable<Object> {
        let headers = new HttpHeaders({ 'Content-Type': 'application/json' });
        return this.httpClient.post(`${this.baseUrl}/edit`, JSON.stringify(model), { headers: headers })
            .catch(this.handleError);
    }

    public deleteImageViewModel(id: string): Observable<Object> {
        return this.httpClient.delete(`${this.baseUrl}/${id}`)
            .catch(this.handleError);
    }

    public postImageViewModel(model: IAddImageViewModel): Observable<Object> {
        let formData = new FormData();
        formData.append('Title', model.title);
        formData.append('Category', model.category);
        formData.append('File', model.file);
        return this.httpClient.post(`${this.baseUrl}/order`, formData)
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