import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { RolesConstants } from '../roles.constants';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss'],
    providers: [AuthService]
})
export class LayoutComponent implements OnInit, OnDestroy {
    isAuthorizedSubscription: Subscription;
    isAuthorized: boolean;

    userDataSubscription: Subscription;
    hasPayingUserRole: boolean;

    constructor(private authService: AuthService, private oidcSecurityService: OidcSecurityService) {
    }

    ngOnInit() {
        console.log(`[ngOnInit]`);

        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe(
            (isAuthorized: boolean) => {
                console.log(`[AuthService] -> [getIsAuthorized] raised with ${isAuthorized}`);

                this.isAuthorized = isAuthorized;
            });

        this.userDataSubscription = this.oidcSecurityService.getUserData().subscribe(
            (userData: any) => {
                console.log('[OidcSecurityService] -> [getUserData] raised with userData');

                if (userData && userData !== '' && userData.role !== '') {
                    let roleName = userData.role;
                    console.log(`Role name is ${roleName}`)

                    if (roleName === RolesConstants.PayingUser) {
                        this.hasPayingUserRole = true;
                    }
                }
            });
    }

    ngOnDestroy(): void {
        console.log(`[ngOnDestroy]`)

        this.isAuthorizedSubscription.unsubscribe();
        this.userDataSubscription.unsubscribe()
    }

    public refreshSession() {
        this.authService.refreshSession();
    }

    public logout() {
        console.log(`[AuthService] -> [logout]`)

        this.authService.logout();
    }
}
