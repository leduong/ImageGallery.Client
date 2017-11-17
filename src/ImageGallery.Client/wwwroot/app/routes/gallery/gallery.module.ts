import { NgModule } from '@angular/core';
import { GalleryComponent } from './gallery/gallery.component';
import { Routes, RouterModule } from '@angular/router';
import { SharedModule } from '../../shared/shared.module';

const routes: Routes = [
    { path: '', component: GalleryComponent },
];

@NgModule({
    imports: [
        SharedModule,
        RouterModule.forChild(routes)
    ],
    declarations: [GalleryComponent],
    exports: [
        RouterModule
    ]
})
export class GalleryModule { }
