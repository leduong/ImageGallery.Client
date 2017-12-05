import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from '../authentication.service';

@Component({
    selector: 'app-layout',
    templateUrl: './layout.component.html',
    styleUrls: ['./layout.component.scss'],
    providers: [AuthenticationService]
})
export class LayoutComponent implements OnInit {

    constructor(private readonly authenticationService: AuthenticationService) { }

    ngOnInit() {
        console.log(`[ngOnInit] app-layout`);
    }

    public onLogout() {
        this.authenticationService.logout()
            .subscribe((response) => { },
            (err: any) => console.log(err),
            () => console.log('logout()'));;
    }

}
