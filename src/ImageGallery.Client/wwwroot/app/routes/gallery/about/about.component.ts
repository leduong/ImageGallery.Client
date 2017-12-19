import { Component, OnInit } from '@angular/core';
import { OidcSecurityCommon } from 'angular-auth-oidc-client';
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

    constructor(private oidcSecurityCommon: OidcSecurityCommon) {
    }

    ngOnInit() {
        this.id_token = this.oidcSecurityCommon.accessToken;
        this.access_token = this.oidcSecurityCommon.idToken;
        this.userData = this.oidcSecurityCommon.userData;

        console.log(`User data: ${this.userData}`);
    }
}
