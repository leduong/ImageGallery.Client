import { NgModule } from '@angular/core';
import { GalleryComponent } from './gallery/gallery.component';
import { GalleryEditComponent } from './gallery-edit/gallery-edit.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';

@NgModule({
    imports: [
        SharedModule
    ],
    declarations: [GalleryComponent, GalleryEditComponent],
    exports: [
        RouterModule,
        GalleryComponent,
        GalleryEditComponent
    ]
})
export class GalleryModule { }
