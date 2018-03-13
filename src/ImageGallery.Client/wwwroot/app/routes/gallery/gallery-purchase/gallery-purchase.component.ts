import { Component, OnInit, PLATFORM_ID, Inject } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

import 'rxjs/add/operator/first';
import 'rxjs/add/operator/toPromise';

import { GalleryService } from '../../../gallery.service';
import { IEditImageViewModel } from '../../../shared/interfaces';

import { isPlatformBrowser } from "@angular/common";
import { IStripeChargeModel } from "./../../../shared/interfaces"

import { StripeTokenHandler } from "./../../../services/stripe_token_handler.service"

import { SettingsService } from "./../../../core/settings/settings.service"

@Component({
  selector: 'app-gallery-purchase',
  templateUrl: './gallery-purchase.component.html',
  styleUrls: ['./gallery-purchase.component.scss'],
  providers: [GalleryService]
})
export class GalleryPurchaseComponent implements OnInit {
  message: string;
  private stripe: any;
  private card: any;
  editImageViewModel: IEditImageViewModel;
  isCardFormValid: boolean;

  categories: string[] = ['Landscapes', 'Portraits', 'Animals'];

  constructor(private readonly galleryService: GalleryService,
    private readonly stripeTokenHandler: StripeTokenHandler,
    private activatedRoute: ActivatedRoute,
    @Inject(PLATFORM_ID) private readonly platformId: string,
    public settings: SettingsService) {
  }

  async ngOnInit() {
    console.log(`[ngOnInit] app-gallery-purchase`);

    const imageId = await this.getImageIdAsync();

    console.log(`Image id: ${imageId}`);

    this.getPurchaseImageViewModel(imageId);

    if (isPlatformBrowser(this.platformId)) {
      this.stripe = (window as any).Stripe(this.settings.stringApi.publishable_key);
      const elements = this.stripe.elements();

      const style = {
        base: {
          color: "#32325d",
          lineHeight: "24px",
          fontFamily: '"Helvetica Neue", Helvetica, sans-serif',
          fontSmoothing: "antialiased",
          fontSize: "16px",
          '::placeholder': {
            color: "#aab7c4"
          }
        },
        invalid: {
          color: "#fa755a",
          iconColor: "#fa755a"
        }
      };

      // Create an instance of the card Element
      this.card = elements.create("card", { style: style, hidePostalCode: true });

      // Add an instance of the card Element into the `card-element` <div>
      this.card.mount("#card-element");

      this.card.addEventListener("change", (event: any) => {
        this.isCardFormValid = !!event.error;
        if (event.error) {
          this.message = event.error.message;
        } else {
          this.message = "";
        }
      });
    }
  }

  public onSubmit() {
    this.message = "Loading...";
    event.preventDefault();

    this.stripe.createToken(this.card).then((result: any) => {
      console.log(result);
      if (result.error) {
        // Inform the user if there was an error
        this.message = result.error.message;
      } else {
        this.message = `Success! Card token ${result.token.id}`;
        const model: IStripeChargeModel = {
          token: result.token.id,
          amount: 100,
          currency: "usd",
          description: "chargeDescription",
          email: "userEmail"
        };
        this.stripeTokenHandler.charge(model)
          .subscribe((data) => { this.message = `ChargeId=${data.value}`; });
      }
    });
  }

  private async getImageIdAsync(): Promise<string> {
    const params = await this.activatedRoute.paramMap.first().toPromise();
    const imageId = params.get('id');
    return imageId;
  }

  private getPurchaseImageViewModel(imageId: string) {
    this.galleryService.getEditImageViewModel(imageId)
      .subscribe((response: IEditImageViewModel) => {
        this.editImageViewModel = response;
      },
      (err: any) => console.log(err),
      () => console.log('getEditImageViewModel() retrieved EditImageViewModel'));
  }
}
