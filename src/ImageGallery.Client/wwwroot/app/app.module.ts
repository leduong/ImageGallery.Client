import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // this is needed!
import { NgModule, Inject } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { TranslateService, TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';
import { ToastModule } from 'ng2-toastr/ng2-toastr';

import { AppComponent } from './app.component';

import { CoreModule } from './core/core.module';
import { LayoutModule } from './layout/layout.module';
import { SharedModule } from './shared/shared.module';
import { RoutesModule } from './routes/routes.module';
import { AuthService } from './services/auth.service';
import { AuthenticationService } from './authentication.service';

import { JwtModule } from '@auth0/angular-jwt';
import { HttpModule } from '@angular/http';
import { HasPayingUserRoleAuthenticationGuard } from './guards/hasPayingUserRoleAuthenticationGuard';
import { AuthGuard } from './guards/authGuard';
import { OAuthModule, OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';

// https://github.com/ocombe/ng2-translate/issues/218
export function createTranslateLoader(http: HttpClient) {
  return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    HttpModule,
    HttpClientModule,
    BrowserAnimationsModule,
    CoreModule,
    LayoutModule,
    SharedModule.forRoot(),
    RoutesModule,
    OAuthModule.forRoot(),
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: (createTranslateLoader),
        deps: [HttpClient]
      }
    }),
    JwtModule.forRoot({
      config: {
        tokenGetter: () => {
          return JSON.parse(sessionStorage.getItem('authorizationData'));
        }
      }
    }),
    ToastModule.forRoot()
  ],
  providers: [
    { provide: 'ORIGIN_URL', useFactory: getBaseUrl },
    AuthService,
    HasPayingUserRoleAuthenticationGuard,
    AuthGuard,
    { provide: 'Window', useValue: window }
  ],
  bootstrap: [AppComponent]
})
export class AppModule {

  clientConfiguration: any;

  constructor(
    private oauthService: OAuthService,
    private http: HttpClient,
    @Inject('Window') private window: Window,
    @Inject('ORIGIN_URL') originUrl: string
  ) {
    console.log('Ctor of [AuthService]');

    this.configClient().subscribe((config: any) => {
      this.clientConfiguration = config;

      var _LTracker = this.window["_LTracker"] || [];

      const logglyClientConfiguration = this.clientConfiguration.logglyClientConfiguration;


      console.log("Here!");
      console.dir(this.clientConfiguration);


      _LTracker.push({
        'logglyKey': logglyClientConfiguration.logglyKey,
        'sendConsoleErrors': true,
        'tag': 'JavaScript'
      });

      const openIdConnectConfiguration = this.clientConfiguration.openIdConnectConfiguration;
      this.oauthService.configure({
        issuer: openIdConnectConfiguration.authority,
        clientId: openIdConnectConfiguration.clientId,
        redirectUri: openIdConnectConfiguration.redirectUri,
        responseType: openIdConnectConfiguration.responseType,
        scope: openIdConnectConfiguration.scope,
        postLogoutRedirectUri: openIdConnectConfiguration.postLogoutRedirectUri,
        dummyClientSecret: openIdConnectConfiguration.clientSecret,
        oidc: false
      });
      this.oauthService.tokenValidationHandler = new JwksValidationHandler();
      this.oauthService.loadDiscoveryDocumentAndTryLogin();
    });
  }

  configClient() {

    console.log('window.location', this.window.location);
    console.log('window.location.href', this.window.location.href);
    console.log('window.location.origin', this.window.location.origin);
    console.log(`${this.window.location.origin}/api/ClientAppSettings`);

    return this.http.get(`${this.window.location.origin}/api/ClientAppSettings`);
  }
}

export function getBaseUrl() {
  return document.getElementsByTagName('base')[0].href;
}