import { Component, OnInit } from '@angular/core';
import { Location } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-unauthorized',
    templateUrl: 'unauthorized.component.html'
})
export class UnauthorizedComponent implements OnInit {

    constructor(private location: Location, private authService: AuthService) {

    }

    ngOnInit() {
        console.log(`[UnauthorizedComponent] -> [ngOnInit]`);
    }

    login() {
        this.authService.login();
    }

    goback() {
        this.location.back();
    }
}