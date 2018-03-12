import { Component, OnInit, OnDestroy, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Subscription } from 'rxjs';
import { OidcSecurityService } from 'angular-auth-oidc-client';
import { RolesConstants } from '../roles.constants';
import { AuthenticationService } from '../authentication.service'
import { BsModalRef, BsModalService } from 'ngx-bootstrap';
import { UserManagementService } from '../services/user.service';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { ToastsManager } from 'ng2-toastr/ng2-toastr';

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
    name = '';

    modalRef: BsModalRef;
    form: FormGroup;

    constructor(
        private authService: AuthService, 
        private oidcSecurityService: OidcSecurityService, 
        private authenticationService: AuthenticationService,
        private modalService: BsModalService,
        private userManagementService: UserManagementService,
        public toastr: ToastsManager, 
        vcr: ViewContainerRef,
    ) {
        this.form = new FormGroup({
            firstName: new FormControl(['', Validators.required]),
            lastName: new FormControl(['', Validators.required]),
            address: new FormControl(['', Validators.required]),
            address2: new FormControl(['', Validators.required]),
            city: new FormControl(['', Validators.required]),
            country: new FormControl(['', Validators.required]),
            state: new FormControl(['', Validators.required]),
        });
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
                    this.name = userData.given_name + ' ' + userData.family_name;
                    let roleName = userData.role;
                    console.log(`Role name is ${roleName}`);

                    if (roleName === RolesConstants.PayingUser) {
                        this.hasPayingUserRole = true;
                    }
                }
            });
        
        this.userManagementService.getUserInfo().subscribe(res => {
            if (res.json()) {
                this.form.controls.firstName.patchValue(res.json().firstName);
                this.form.controls.lastName.patchValue(res.json().lastName);
                this.form.controls.address.patchValue(res.json().address);
                this.form.controls.address2.patchValue(res.json().address2);
                this.form.controls.state.patchValue(res.json().state);
                this.form.controls.city.patchValue(res.json().city);
                this.form.controls.country.patchValue(res.json().country);
            }
        });
    }

    ngOnDestroy(): void {
        console.log(`[ngOnDestroy]`);

        this.isAuthorizedSubscription.unsubscribe();
        this.userDataSubscription.unsubscribe();
    }

    public refreshSession() {
        this.authService.refreshSession();
    }

    public logout() {
        console.log(`[AuthService] -> [logout]`);

        this.authenticationService.logout().subscribe(() => {
            this.authService.logout();
        });
    }

    openModal(template: TemplateRef<any>) {
        this.modalRef = this.modalService.show(template);
    }

    saveUserInfo() {
        this.toastr.success('Profile has been saved successfully!', 'Success!', {showCloseButton: true});
        this.modalRef.hide();
    }
}