import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
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

  isUserInRoleSubscription: Subscription;
  hasPayingUserRole: boolean;

  constructor(private authService: AuthService) {
  }

  ngOnInit() {
    console.log(`[ngOnInit]`);

    this.isAuthorizedSubscription = this.authService.getIsAuthorized().subscribe(
      (isAuthorized: boolean) => {
        console.log(`[AuthService] -> [getIsAuthorized] raised with ${isAuthorized}`);

        this.isAuthorized = isAuthorized;
      });

    this.isUserInRoleSubscription = this.authService.checkUserRole(RolesConstants.PayingUser).subscribe(
      (isInRole: boolean) => {
        console.log(`[AuthService] -> [checkUserRole] raised with ${isInRole}`);

        this.hasPayingUserRole = isInRole;
      });
  }

  ngOnDestroy(): void {
    console.log(`[ngOnDestroy]`)

    this.isAuthorizedSubscription.unsubscribe();
    this.isUserInRoleSubscription.unsubscribe();
  }

  public refreshSession() {
    this.authService.refreshSession();
  }

  public logout() {
    console.log(`[AuthService] -> [logout]`)

    this.authService.logout();
  }
}
