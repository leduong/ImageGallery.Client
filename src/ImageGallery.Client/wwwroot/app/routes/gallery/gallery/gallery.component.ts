import { Component, OnInit } from '@angular/core';
import { GalleryService } from '../../../gallery.service';
import { IGalleryIndexViewModel } from '../../../shared/interfaces';


@Component({
    selector: 'app-gallery',
    templateUrl: './gallery.component.html',
    styleUrls: ['./gallery.component.scss'],
    providers: [GalleryService]
})
export class GalleryComponent implements OnInit {

    galleryIndexViewModel: IGalleryIndexViewModel;

    constructor(private galleryService: GalleryService) { }

    ngOnInit() {
        console.log(`[ngOnInit] app-gallery`);

        this.getGalleryIndexViewModel();
    }

    public deleteImage(imageId: string) {
        console.log(`[deleteImage] app-gallery-edit`);

        this.galleryService.deleteImageViewModel(imageId)
            .subscribe((response) => { },
            (err: any) => console.log(err),
            () => console.log('deleteImageViewModel() delete ImageViewModel'));;
    }

    private getGalleryIndexViewModel() {
        this.galleryService.getGalleryIndexViewModel()
            .subscribe((response: IGalleryIndexViewModel) => {
                this.galleryIndexViewModel = response;
            },
            (err: any) => console.log(err),
            () => console.log('getGalleryIndexViewModel() retrieved GalleryIndexViewModel'));
    }
}
