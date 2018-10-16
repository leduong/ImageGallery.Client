import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { ActivatedRoute, Params, Router } from "@angular/router";

import 'rxjs/add/operator/first';
import 'rxjs/add/operator/toPromise';

import { GalleryService } from '../../../gallery.service';
import { IAlbumViewModel } from '../../../shared/interfaces';
import { ToastrService } from 'ngx-toastr';


@Component({
    selector: 'app-album-view',
    templateUrl: './album-view.component.html',
    styleUrls: ['./album-view.component.scss'],
    providers: [GalleryService]
})
export class AlbumViewComponent implements OnInit {

    albumViewModel: IAlbumViewModel;

    pagination = {
        page: 1,
        limit: 15,
        totalItems: 10
    };
    perPage = [15, 30, 60, 90];
    title = '';

    categories: string[] = ['Landscapes', 'Portraits', 'Animals'];

    constructor(private readonly galleryService: GalleryService,
        private activatedRoute: ActivatedRoute,
        public toastr: ToastrService, 
        vcr: ViewContainerRef) {
        //this.toastr.setRootViewContainerRef(vcr);
    }

    async ngOnInit() {
        this.title = localStorage.getItem('album-title');
        const imageId = await this.getImageIdAsync();
        this.getAlbumViewModel(imageId);
    }

    private async getImageIdAsync(): Promise<string> {
        const params = await this.activatedRoute.paramMap.first().toPromise();
        const imageId = params.get('id');
        return imageId;
    }

    private getAlbumViewModel(imageId: string) {
        this.galleryService.getAlbumViewModel(imageId, this.pagination.limit, this.pagination.page)
            .subscribe((response: IAlbumViewModel) => {
                this.albumViewModel = response;
            },
            (err: any) => console.log(err));
    }
}
