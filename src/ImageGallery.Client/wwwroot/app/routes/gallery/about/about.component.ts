import { Component, OnInit } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { KeysPipe } from '../../../pipes/keys.pipe';


@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.scss']
})
export class AboutComponent implements OnInit {

  public id_token: string;
  public access_token: string;
  public userData: any;

  constructor(private oauthService: OAuthService) { }

  ngOnInit() {
    this.id_token = this.oauthService.getIdToken();
    this.access_token = this.oauthService.getAccessToken();
    this.userData = this.oauthService.getIdentityClaims();

    console.log(`User data: ${this.userData}`);
  }

}
