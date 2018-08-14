import { Injectable } from '@angular/core';
import { Response } from '@angular/http';

import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/catch';

import { IEditImageViewModel, IAddImageViewModel, IAlbumViewModel, } from './shared/interfaces';
import { HttpClient, HttpHeaders } from '@angular/common/http';

import { OAuthService } from 'angular-oauth2-oidc';

@Injectable()
export class GalleryService {

  private baseUrl: string = '/api/images';
  private albumUrl: string = 'https://imagegallery-api.informationcart.com/api/albums';

  constructor(private httpClient: HttpClient, private oauthService: OAuthService) {
  }

  public getGalleryIndexViewModel(limit: number, page: number) {
    var self = this;
    return new Promise((resolve, reject) => {
      this.httpClient.get(`${this.baseUrl}/list?limit=${limit}&page=${page}`, { observe: 'response', headers: self.generateBearerHeaaders() })
        .subscribe(res => {
          resolve({
            totalCount: res.headers.get('X-InlineCount'),
            images: res.body
          });
        }, error => {
          reject(error);
        });
    });
  }

  public getAlbumIndexViewModel(limit: number, page: number) {
    var self = this;
    console.log(self.generateBearerHeaaders());
    return new Promise((resolve, reject) => {
      this.httpClient.get(`${this.albumUrl}/${limit}/${page}`, { observe: 'response', headers: self.generateBearerHeaaders() })
        .subscribe(res => {
          resolve({
            totalCount: res.headers.get('X-InlineCount'),
            images: res.body
          });
        }, error => {
          reject(error);
        });
    });
  }

  public getAlbumViewModel(id: string): Observable<IAlbumViewModel> {
    var headers = this.generateBearerHeaaders();
    headers.append("Content-Type", "application/json");
    
    return this.httpClient.get<IAlbumViewModel>(`${this.albumUrl}/${id}`, { headers: this.generateBearerHeaaders() })
      .catch(this.handleError);
  }

  public getEditImageViewModel(id: string): Observable<IEditImageViewModel> {
    var self = this;
    return this.httpClient.get<IEditImageViewModel>(`${this.baseUrl}/${id}`, { headers: self.generateBearerHeaaders() })
      .catch(this.handleError);
  }

  public postEditImageViewModel(model: IEditImageViewModel): Observable<Object> {
    /*
    var headers = this.generateBearerHeaaders();
    headers.append("Content-Type", "application/json");
    console.log("buuuuuu!");

    return this.httpClient.post(`${this.baseUrl}/edit`, JSON.stringify(model), { headers: headers })
      .catch(this.handleError);
    */
    var headers = this.generateBearerHeaaders();
    headers.append("Content-Type", "application/json");

    return this.httpClient.post(`${this.baseUrl}/edit`, model, { headers: headers });
  }

  public deleteImageViewModel(id: string): Observable<Object> {
    var self = this;
    return this.httpClient.delete(`${this.baseUrl}/${id}`, { headers: self.generateBearerHeaaders() })
      .catch(this.handleError);
  }

  public postImageViewModel(model: IAddImageViewModel): Observable<Object> {
    let formData = new FormData();
    formData.append('Title', model.title);
    formData.append('Category', model.category);
    formData.append('File', model.file);

    var options = { headers: this.generateBearerHeaaders() }

    return this.httpClient.post(`${this.baseUrl}/order`, formData, options)
      .catch(this.handleError);
  }

  private generateBearerHeaaders(): HttpHeaders {
    return new HttpHeaders({
      "Authorization": "Bearer " + this.oauthService.getAccessToken()
    });
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
