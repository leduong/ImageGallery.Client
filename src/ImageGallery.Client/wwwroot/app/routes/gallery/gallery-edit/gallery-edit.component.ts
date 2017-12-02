import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Params } from "@angular/router";

import 'rxjs/add/operator/first';
import 'rxjs/add/operator/toPromise';

import { GalleryService } from '../../../gallery.service';
import { IEditImageViewModel } from '../../../shared/interfaces';


@Component({
    selector: 'app-gallery-edit',
    templateUrl: './gallery-edit.component.html',
    styleUrls: ['./gallery-edit.component.scss'],
    providers: [GalleryService]
})
export class GalleryEditComponent implements OnInit {

    editImageViewModel: IEditImageViewModel;

    constructor(private readonly galleryService: GalleryService, private activatedRoute: ActivatedRoute) { }

    async ngOnInit() {
        console.log(`[ngOnInit] app-gallery-edit`);

        const imageId = await this.getImageIdAsync();

        console.log(`Image id: ${imageId}`);

        this.getEditImageViewModel(imageId);
    }

    private async getImageIdAsync(): Promise<string> {
        const params = await this.activatedRoute.paramMap.first().toPromise();
        const imageId = params.get('id');
        return imageId;
    }

    getEditImageViewModel(imageId: string) {
        this.galleryService.getEditImageViewModel(imageId)
            .subscribe((response: IEditImageViewModel) => {
                this.editImageViewModel = response;
            },
            (err: any) => console.log(err),
            () => console.log('getEditImageViewModel() retrieved EditImageViewModel'));
    }
}
