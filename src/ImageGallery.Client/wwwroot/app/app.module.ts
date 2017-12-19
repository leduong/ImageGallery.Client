import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // this is needed!
import { NgModule } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { TranslateService, TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppComponent } from './app.component';

import { CoreModule } from './core/core.module';
import { LayoutModule } from './layout/layout.module';
import { SharedModule } from './shared/shared.module';
import { RoutesModule } from './routes/routes.module';
import { AuthService } from './services/auth.service';

import { AuthModule, OidcSecurityService } from 'angular-auth-oidc-client';
import { JwtModule } from '@auth0/angular-jwt';
import { HttpModule } from '@angular/http';

// https://github.com/ocombe/ng2-translate/issues/218
export function createTranslateLoader(http: HttpClient) {
    return new TranslateHttpLoader(http, './assets/i18n/', '.json');
}

@NgModule({
    declarations: [
        AppComponent
    ],
    imports: [
        AuthModule.forRoot(),
        HttpModule,
        HttpClientModule,
        BrowserAnimationsModule, // required for ng2-tag-input
        CoreModule,
        LayoutModule,
        SharedModule.forRoot(),
        RoutesModule,
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
          })
    ],
    providers: [
        { provide: 'ORIGIN_URL', useFactory: getBaseUrl },
        { provide: 'IDENTITY_URL', useFactory: identityUrlFactory },
        AuthService,
        OidcSecurityService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}

export function identityUrlFactory() {
    return "https://auth.informationcart.com";
}