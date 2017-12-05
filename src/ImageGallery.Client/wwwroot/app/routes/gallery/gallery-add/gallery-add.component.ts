import { Component, OnInit } from '@angular/core';

import { GalleryService } from '../../../gallery.service';
import { IAddImageViewModel } from '../../../shared/interfaces';
import { Observable } from 'rxjs/Observable';
import { debug } from 'util';


@Component({
    selector: 'app-gallery-add',
    templateUrl: './gallery-add.component.html',
    styleUrls: ['./gallery-add.component.scss'],
    providers: [GalleryService]
})
export class GalleryAddComponent implements OnInit {

    addImageViewModel: IAddImageViewModel = { title: "", category: "Portraits", file: null };

    categories: string[] = ['Landscapes', 'Portraits', 'Animals'];

    constructor(private readonly galleryService: GalleryService) { }

    ngOnInit() {
        console.log(`[ngOnInit] app-gallery-add`);
    }

    onUpload(event: EventTarget) {
        let eventObj: MSInputMethodContext = <MSInputMethodContext> event;
        let target: HTMLInputElement = <HTMLInputElement> eventObj.target;
        let files: FileList = target.files;
        this.addImageViewModel.file = files[0];
    }

    public onSubmit() {
        console.log(`[onSubmit] app-gallery-add`);

        this.galleryService.postImageViewModel(this.addImageViewModel)
            .subscribe((response) => { },
            (err: any) => console.log(err),
            () => console.log('postImageViewModel() posted AddImageViewModel'));;
    }
}
