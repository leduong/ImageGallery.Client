import { Injectable, Component, OnInit, OnDestroy, Inject } from '@angular/core';
import { Observable } from 'rxjs/Rx';
import { Subscription } from 'rxjs/Subscription';

import { OidcSecurityService, OpenIDImplicitFlowConfiguration } from 'angular-auth-oidc-client';

@Injectable()
export class AuthService implements OnInit, OnDestroy {
    isAuthorizedSubscription: Subscription;
    isAuthorized: boolean;

    constructor(private oidcSecurityService: OidcSecurityService) {
        console.log(`Ctor of [AuthService]`);

        if (this.oidcSecurityService.moduleSetup) {
            console.log(`Property [moduleSetup] of [OidcSecurityService] configured properly`);

            this.doCallbackLogicIfRequired();
        } else {
            console.log(`Property [moduleSetup] of [OidcSecurityService] first time to load well know endpoints`);

            this.oidcSecurityService.onModuleSetup.subscribe(() => {
                console.log(`[onModuleSetup] raise finished call doCallbackLogicIfRequired`);

                this.doCallbackLogicIfRequired();
            });
        }
    }

    ngOnInit() {
        console.log(`[ngOnInit] AuthService`);

        this.isAuthorizedSubscription = this.oidcSecurityService.getIsAuthorized().subscribe(
            (isAuthorized: boolean) => {
                this.isAuthorized = isAuthorized;
            });
    }

    ngOnDestroy(): void {
        console.log(`[ngOnDestroy] AuthService`);

        this.isAuthorizedSubscription.unsubscribe();
        this.oidcSecurityService.onModuleSetup.unsubscribe();
    }

    getIsAuthorized(): Observable<boolean> {
        return this.oidcSecurityService.getIsAuthorized();
    }

    login() {
        localStorage.removeItem('page');
        localStorage.removeItem('limit');
        console.log('[login] of AuthService');
        this.oidcSecurityService.authorize();
    }

    refreshSession() {
        console.log('[refreshSession] AuthService');
        this.oidcSecurityService.authorize();
    }

    logout() {
        console.log('[logout] AuthService');
        this.oidcSecurityService.logoff();
    }

    private doCallbackLogicIfRequired() {
        let hash = window.location.hash;
        console.log(`location is ${location} and window.location.hash is ${hash}`)

        if (typeof location !== "undefined" && hash) {
            console.log(`[OidcSecurityService] -> [authorizedCallback()]`);

            this.oidcSecurityService.authorizedCallback();
        } else {
            console.log(`[OidcSecurityService] -> [getToken()]`);

            let token = this.oidcSecurityService.getToken();
            if (!token) {
                console.log(`[AuthService] -> [login] call`);

                this.login();
            }
        }
    }
}