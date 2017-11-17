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
        this.getGalleryIndexViewModel()
    }

    getGalleryIndexViewModel() {
        this.galleryService.getGalleryIndexViewModel()
            .subscribe((response: IGalleryIndexViewModel) => {
                this.galleryIndexViewModel = response;
            },
            (err: any) => console.log(err),
            () => console.log('getGalleryIndexViewModel() retrieved GalleryIndexViewModel'));
    }
}
