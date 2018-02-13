import { Injectable } from '@angular/core';
import { CanActivate } from '@angular/router';
import { RolesConstants } from '../roles.constants';
import { OAuthModule, OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';

@Injectable()
export class HasPayingUserRoleAuthenticationGuard implements CanActivate {

  private hasPayingUserRole = false;
  private isAuthorized: boolean;

  constructor(
    private oAuthService: OAuthService
  ) { }

  canActivate(): boolean {
    console.log(`[HasAdminRoleAuthenticationGuard] -> [canActivate]`);

    this.isAuthorized = this.oAuthService.hasValidAccessToken();
    console.log(`[HasPayingUserRoleAuthenticationGuard] -> [OidcSecurityService] -> [getIsAuthorized] raised with ${this.isAuthorized}`);

    var userData = <any>this.oAuthService.getIdentityClaims();
    console.log(`[HasPayingUserRoleAuthenticationGuard] -> [OidcSecurityService] -> [getUserData] raised`);

    if (!userData || !userData.role) return false;

    return userData.role === RolesConstants.PayingUser && this.isAuthorized;
  }
}