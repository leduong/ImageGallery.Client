import { NgModule } from '@angular/core';
import { GalleryComponent } from './gallery/gallery.component';
import { GalleryEditComponent } from './gallery-edit/gallery-edit.component';
import { GalleryViewComponent } from './gallery-view/gallery-view.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { GalleryAddComponent } from './gallery-add/gallery-add.component';
import { AboutComponent } from './about/about.component';
import { KeysPipe } from '../../pipes/keys.pipe';
import { PaginationModule } from 'ngx-bootstrap';
import { NgxLoadingSpinnerModule } from 'ngx-loading-spinner-fork';

@NgModule({
    imports: [
        SharedModule,
        PaginationModule.forRoot(),
        NgxLoadingSpinnerModule.forRoot(),
    ],
    declarations: [GalleryComponent, GalleryEditComponent, GalleryViewComponent, GalleryAddComponent, AboutComponent, KeysPipe],
    exports: [
        RouterModule,
        GalleryComponent,
        GalleryEditComponent,
        GalleryViewComponent,
        GalleryAddComponent,
        AboutComponent,
        PaginationModule,
        NgxLoadingSpinnerModule,
    ]
})
export class GalleryModule { }
