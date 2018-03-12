import { NgModule } from '@angular/core';
import { GalleryComponent } from './gallery/gallery.component';
import { GalleryEditComponent } from './gallery-edit/gallery-edit.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { GalleryAddComponent } from './gallery-add/gallery-add.component';
import { AboutComponent } from './about/about.component';
import { KeysPipe } from '../../pipes/keys.pipe';
import { PaginationModule } from 'ngx-bootstrap';
import { Ng4LoadingSpinnerModule } from 'ng4-loading-spinner';
import { FileSelectDirective, FileDropDirective, FileUploader, FileUploadModule } from 'ng2-file-upload/ng2-file-upload';

@NgModule({
    imports: [
        SharedModule,
        PaginationModule.forRoot(),
        Ng4LoadingSpinnerModule.forRoot(),
        FileUploadModule,
    ],
    declarations: [GalleryComponent, GalleryEditComponent, GalleryAddComponent, AboutComponent, KeysPipe],
    exports: [
        RouterModule,
        GalleryComponent,
        GalleryEditComponent,
        GalleryAddComponent,
        AboutComponent,
        PaginationModule,
        Ng4LoadingSpinnerModule,
    ],
})
export class GalleryModule { }
