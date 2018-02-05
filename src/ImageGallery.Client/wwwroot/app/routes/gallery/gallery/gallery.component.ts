import { Component, OnInit, ViewContainerRef, ChangeDetectorRef } from '@angular/core';
import { GalleryService } from '../../../gallery.service';
import { IGalleryIndexViewModel } from '../../../shared/interfaces';
import { AuthService } from '../../../services/auth.service';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';


@Component({
    selector: 'app-gallery',
    templateUrl: './gallery.component.html',
    styleUrls: ['./gallery.component.scss'],
    providers: [GalleryService]
})
export class GalleryComponent implements OnInit {

    galleryIndexViewModel: IGalleryIndexViewModel;
    page = 1;
    limit = 15;
    perPage = [15, 30, 60, 90];
    totalItems = 10;

    constructor(
        private authService: AuthService,
        private galleryService: GalleryService,
        public toastr: ToastsManager, 
        vcr: ViewContainerRef,
        private spinnerService: Ng4LoadingSpinnerService,
        private changeDetectorRef: ChangeDetectorRef
    ) { 
        this.toastr.setRootViewContainerRef(vcr);
    }

    ngOnInit() {
        this.limit = localStorage.getItem('limit') ? parseInt(localStorage.getItem('limit')) : 15;
        this.page = localStorage.getItem('page') ? parseInt(localStorage.getItem('page')) : 1;

        this.authService.getIsAuthorized().subscribe(
            (isAuthorized: boolean) => {
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

    private getGalleryIndexViewModel(event?) {
        this.spinnerService.show();

        if (typeof event == 'string') {
            this.limit = parseInt(event);
            localStorage.setItem('limit', this.limit.toString());
        } else if (typeof event == 'object') {
            this.limit = event.itemsPerPage;
            this.page = event.page;
            localStorage.setItem('limit', this.limit.toString());
            localStorage.setItem('page', this.page.toString());
        }
        this.changeDetectorRef.detectChanges();

        this.galleryService.getGalleryIndexViewModel(this.limit, this.page)
            .then((response: any) => {
                this.galleryIndexViewModel = response.images;
                this.totalItems = response.totalCount;
                this.scrollToTop();
                this.spinnerService.hide();
            }).catch(err => {
                this.spinnerService.hide();
            });
    }

    private scrollToTop() {
        window.scrollTo(0, 0);
    }
}
