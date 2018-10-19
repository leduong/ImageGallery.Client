import { Component, OnInit, ViewContainerRef } from '@angular/core'
import { ActivatedRoute, Params, Router } from '@angular/router'

import 'rxjs/add/operator/first'
import 'rxjs/add/operator/toPromise'

import { GalleryService } from '../../../gallery.service'
import { IEditImageViewModel } from '../../../shared/interfaces'
import { Observable } from 'rxjs/Observable'
import { ToastrService } from 'ngx-toastr'

@Component({
  selector: 'app-gallery-edit',
  templateUrl: './gallery-edit.component.html',
  styleUrls: ['./gallery-edit.component.scss'],
  providers: [GalleryService],
})
export class GalleryEditComponent implements OnInit {
  editImageViewModel: IEditImageViewModel

  categories: string[] = ['Landscapes', 'Portraits', 'Animals']

  constructor(
    private readonly galleryService: GalleryService,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    public toastr: ToastrService,
    vcr: ViewContainerRef
  ) {
    //this.toastr.setRootViewContainerRef(vcr);
  }

  async ngOnInit() {
    console.log(`[ngOnInit] app-gallery-edit`)

    const imageId = await this.getImageIdAsync()

    console.log(`Image id: ${imageId}`)

    this.getEditImageViewModel(imageId)
  }

  public onSubmit() {
    console.log(`[onSubmit] app-gallery-edit`)

    this.galleryService
      .postEditImageViewModel(this.editImageViewModel)
      .subscribe(
        response => {},
        (err: any) => {
          this.toastr.error('Failed to edit image!', 'Oops!', {
            closeButton: true,
          })
          console.log(err)
        },
        () => {
          console.log('postEditImageViewModel() posted EditImageViewModel')
          localStorage.setItem('isEdited', 'yes')
          this.router.navigate(['/'])
        }
      )
  }

  private async getImageIdAsync(): Promise<string> {
    const params = await this.activatedRoute.paramMap.first().toPromise()
    const imageId = params.get('id')
    return imageId
  }

  private getEditImageViewModel(imageId: string) {
    this.galleryService.getEditImageViewModel(imageId).subscribe(
      (response: IEditImageViewModel) => {
        this.editImageViewModel = response
      },
      (err: any) => console.log(err),
      () => console.log('getEditImageViewModel() retrieved EditImageViewModel')
    )
  }
}
