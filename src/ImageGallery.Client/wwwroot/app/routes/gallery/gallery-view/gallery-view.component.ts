import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { ActivatedRoute, Params, Router } from "@angular/router";

import 'rxjs/add/operator/first';
import 'rxjs/add/operator/toPromise';

import { GalleryService } from '../../../gallery.service';
import { IImageViewModel } from '../../../shared/interfaces';
import { ToastrService } from 'ngx-toastr';


@Component({
    selector: 'app-gallery-view',
    templateUrl: './gallery-view.component.html',
    styleUrls: ['./gallery-view.component.scss'],
    providers: [GalleryService]
})
export class GalleryViewComponent implements OnInit {

    imageViewModel: IImageViewModel;

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

        this.getImageViewModel(imageId);
    }

    private async getImageIdAsync(): Promise<string> {
        const params = await this.activatedRoute.paramMap.first().toPromise();
        const imageId = params.get('id');
        console.log(imageId);
        return imageId;
    }

    private getImageViewModel(imageId: string) {
        this.galleryService.getImageViewModel(imageId)
            .subscribe((response: IImageViewModel) => {
                this.imageViewModel = response;
            },
            (err: any) => console.log(err),
            () => console.log('getImageViewModel() retrieved EditImageViewModel'));
    }
}
