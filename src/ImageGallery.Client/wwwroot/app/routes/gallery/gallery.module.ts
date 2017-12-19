import { NgModule } from '@angular/core';
import { GalleryComponent } from './gallery/gallery.component';
import { GalleryEditComponent } from './gallery-edit/gallery-edit.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';
import { GalleryAddComponent } from './gallery-add/gallery-add.component';
import { AboutComponent } from './about/about.component';
import { KeysPipe } from '../../pipes/keys.pipe';

@NgModule({
    imports: [
        SharedModule
    ],
    declarations: [GalleryComponent, GalleryEditComponent, GalleryAddComponent, AboutComponent, KeysPipe],
    exports: [
        RouterModule,
        GalleryComponent,
        GalleryEditComponent,
        GalleryAddComponent,
        AboutComponent
    ]
})
export class GalleryModule { }
