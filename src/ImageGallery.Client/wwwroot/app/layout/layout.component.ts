import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss'],
    providers: [AuthService]
})
export class LayoutComponent implements OnInit, OnDestroy {
    isAuthorizedSubscription: Subscription;
    isAuthorized: boolean;

    constructor(private authService: AuthService) {
    }

    ngOnInit() {
        console.log(`[LayoutComponent] -> [ngOnInit]`);

        this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe(
            (isAuthorized: boolean) => {
                console.log(`[AuthService] -> [getIsAuthorized] raised with ${isAuthorized}`);

                this.isAuthorized = isAuthorized;
            });
    }

    ngOnDestroy(): void {
        this.isAuthorizedSubscription.unsubscribe();
    }

    public refreshSession() {
        this.authService.refreshSession();
    }

    public logout() {
        this.authService.logout();
    }
}
