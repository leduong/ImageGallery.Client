import { Component, OnInit, ViewContainerRef } from '@angular/core';
import { GalleryService } from '../../../gallery.service';
import { IGalleryIndexViewModel } from '../../../shared/interfaces';
import { AuthService } from '../../../services/auth.service';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';


@Component({
    selector: 'app-gallery',
    templateUrl: './gallery.component.html',
    styleUrls: ['./gallery.component.scss'],
    providers: [GalleryService]
})
export class GalleryComponent implements OnInit {

    galleryIndexViewModel: IGalleryIndexViewModel;

    constructor(
        private authService: AuthService,
        private galleryService: GalleryService,
        public toastr: ToastsManager, 
        vcr: ViewContainerRef
    ) { 
        this.toastr.setRootViewContainerRef(vcr);
    }

    ngOnInit() {
        console.log(`[ngOnInit] app-gallery`);

        this.authService.getIsAuthorized().subscribe(
            (isAuthorized: boolean) => {
                console.log(`[AuthService] -> [getIsAuthorized] raised with ${isAuthorized}`);

                if (isAuthorized)
                    this.getGalleryIndexViewModel();
            });
    }

    ngAfterViewInit() {
        if (localStorage.getItem('isEdited') == 'yes') {
            this.toastr.success('Image has been edited successfully!', 'Success!', {showCloseButton: true});
            localStorage.removeItem('isEdited');
        } else if (localStorage.getItem('isAdded') == 'yes') {
            this.toastr.success('Image has been added successfully!', 'Success!', {showCloseButton: true});
            localStorage.removeItem('isAdded');
        }
    }

    public deleteImage(imageId: string) {
        console.log(`[deleteImage] app-gallery-edit`);

        this.galleryService.deleteImageViewModel(imageId)
            .subscribe((response) => { },
            (err: any) => {
                this.toastr.error('Failed to delete image!', 'Oops!', {showCloseButton: true});
                console.log(err);
            },
            () => {
                this.toastr.success('Image has been deleted successfully!', 'Success!', {showCloseButton: true});
                this.galleryIndexViewModel.images = this.galleryIndexViewModel.images.filter(x => x.id != imageId);
            });
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
