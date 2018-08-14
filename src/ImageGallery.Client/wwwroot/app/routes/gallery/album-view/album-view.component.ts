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

    categories: string[] = ['Landscapes', 'Portraits', 'Animals'];

    constructor(private readonly galleryService: GalleryService,
        private activatedRoute: ActivatedRoute,
        public toastr: ToastrService, 
        vcr: ViewContainerRef) {
        //this.toastr.setRootViewContainerRef(vcr);
    }

    async ngOnInit() {
        console.log(`[ngOnInit] app-gallery-view`);

        const imageId = await this.getImageIdAsync();

        console.log(`Image id: ${imageId}`);

        this.getAlbumViewModel(imageId);
    }

    private async getImageIdAsync(): Promise<string> {
        const params = await this.activatedRoute.paramMap.first().toPromise();
        const imageId = params.get('id');
        console.log(imageId);
        return imageId;
    }

    private getAlbumViewModel(imageId: string) {
        this.galleryService.getAlbumViewModel(imageId)
            .subscribe((response: IAlbumViewModel) => {
                this.albumViewModel = response;
            },
            (err: any) => console.log(err),
            () => console.log('getImageViewModel() retrieved EditImageViewModel'));
    }
}
