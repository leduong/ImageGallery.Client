import { Injectable } from '@angular/core';
import { CanActivate} from '@angular/router';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { RolesConstants } from '../roles.constants';

@Injectable()
export class HasPayingUserRoleAuthenticationGuard implements CanActivate {

    private hasPayingUserRole = false;
    private isAuthorized: boolean;

    constructor(
        private oidcSecurityService: OidcSecurityService
    ) { }

    canActivate(): boolean {
        console.log(`[HasAdminRoleAuthenticationGuard] -> [canActivate]`)

        this.oidcSecurityService.getIsAuthorized().subscribe(
            (isAuthorized: boolean) => {
                console.log(`[HasPayingUserRoleAuthenticationGuard] -> [OidcSecurityService] -> [getIsAuthorized] raised with ${isAuthorized}`)

                this.isAuthorized = isAuthorized;
            });

        this.oidcSecurityService.getUserData().subscribe(
            (userData: any) => {
                console.log(`[HasPayingUserRoleAuthenticationGuard] -> [OidcSecurityService] -> [getUserData] raised`)

                if (userData && userData !== '') {
                    for (let i = 0; i < userData.role.length; i++) {
                        if (userData.role[i] === RolesConstants.PayingUser) {
                            this.hasPayingUserRole = true;
                        }
                    }
                }
            });

        return this.hasPayingUserRole && this.isAuthorized;
    }
}