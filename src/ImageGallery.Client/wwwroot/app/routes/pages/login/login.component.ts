import { Component, OnInit } from '@angular/core';
import { SettingsService } from '../../../core/settings/settings.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { CustomValidators } from 'ng2-validation';
import { OAuthService } from 'angular-oauth2-oidc';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  valForm: FormGroup;
  private alertMessage: string;

  constructor(public settings: SettingsService, private oauthService: OAuthService, fb: FormBuilder, private router: Router) {
    console.log("login.component.ts");
    //oath
    if (oauthService.hasValidAccessToken()) {
      this.router.navigate(["home"]);
    }

    //form validation
    this.valForm = fb.group({
      'login': [null, Validators.compose([Validators.required])],
      'password': [null, Validators.required]
    });
  }

  submitForm($ev, value: any) {
    $ev.preventDefault();
    for (let c in this.valForm.controls) {
      this.valForm.controls[c].markAsTouched();
    }
    if (this.valForm.valid) {
      this.loginWithPassword(value.login, value.password);
    }
  }

  loginWithPassword(login: string, password: string) {
    console.log(`login: ${login} + password: ${password}`);
    this
      .oauthService
      .fetchTokenUsingPasswordFlowAndLoadUserProfile(login, password)
      .then(() => {
        this.router.navigate(['/']);
      })
      .catch((err) => {
        this.alertMessage = "Invalid request";
      });
  }

  ngOnInit() {}
}
