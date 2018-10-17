import { Injectable, Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Subscription } from 'rxjs/Subscription';

import { OAuthService } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';

@Injectable()
export class AuthService /*implements OnInit, OnDestroy*/ {
  isAuthorizedSubscription: Subscription;
  isAuthorized: boolean;

  constructor(private oAuthService: OAuthService, private router: Router) {

  }

  checkUserRole(role: string): Observable<boolean> {
    var self = this;
    return new Observable((observer) => {

      console.log("checkUserRole next value");
      // observable execution
      observer.next(innerCheckUserRole());
    });

    function innerCheckUserRole(): boolean {
      var claims = <any>self.oAuthService.getIdentityClaims();
      var hasvalidToken = self.oAuthService.hasValidAccessToken();

      if (!hasvalidToken || !claims || !claims.role) return false;

      var roleSplitted = claims.role.split(",");
      return !!roleSplitted.find((item) => { return !!item && item.trim().toLowerCase() === role.toLowerCase(); });
    }
  }

  getUser() {
    return this.oAuthService.getIdentityClaims();
  }

  getIsAuthorized(): Observable<boolean> {
    var self = this;
    return new Observable((observer) => {
      console.log("getIsAuthorized next value");
      observer.next(self.oAuthService.hasValidAccessToken());
    });
  }

  login() {
    console.log('[login] of AuthService');
  }

  refreshSession() {
    console.log('[refreshSession] AuthService');
    this.oAuthService.refreshToken();
  }

  logout() {
    localStorage.removeItem('page');
    localStorage.removeItem('limit');
    console.log('[logout] AuthService');
    this.oAuthService.logOut();
    this.router.navigate(["/login"]);
  }
}
