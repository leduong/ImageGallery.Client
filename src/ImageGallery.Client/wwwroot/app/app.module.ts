import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations'; // this is needed!
import { NgModule, Inject } from '@angular/core';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { TranslateService, TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppComponent } from './app.component';

import { CoreModule } from './core/core.module';
import { LayoutModule } from './layout/layout.module';
import { SharedModule } from './shared/shared.module';
import { RoutesModule } from './routes/routes.module';
import { AuthService } from './services/auth.service';

import { AuthModule, OidcSecurityService, OpenIDImplicitFlowConfiguration } from 'angular-auth-oidc-client';
import { JwtModule } from '@auth0/angular-jwt';
import { HttpModule } from '@angular/http';
import { HasPayingUserRoleAuthenticationGuard } from './guards/hasPayingUserRoleAuthenticationGuard';

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
        AuthService,
        OidcSecurityService,
        HasPayingUserRoleAuthenticationGuard
    ],
    bootstrap: [AppComponent]
})
export class AppModule {

    clientConfiguration: any;

    constructor(private oidcSecurityService: OidcSecurityService,
        private http: HttpClient,
        @Inject('ORIGIN_URL') originUrl: string
    ) {
        console.log('Ctor of [AuthService]');

        this.configClient().subscribe((config: any) => {
            this.clientConfiguration = config;

            const openIdImplicitFlowConfiguration = new OpenIDImplicitFlowConfiguration();
            openIdImplicitFlowConfiguration.stsServer = this.clientConfiguration.authority;
            openIdImplicitFlowConfiguration.redirect_url = originUrl + 'callback';

            console.log(`redirect_url -> ${openIdImplicitFlowConfiguration.redirect_url}`)

            openIdImplicitFlowConfiguration.client_id = this.clientConfiguration.clientId;
            openIdImplicitFlowConfiguration.response_type = 'id_token token';
            openIdImplicitFlowConfiguration.scope = 'roles openid profile address country imagegalleryapi subscriptionlevel';
            openIdImplicitFlowConfiguration.post_logout_redirect_uri = originUrl;

            console.log(`post_logout_redirect_uri -> ${openIdImplicitFlowConfiguration.post_logout_redirect_uri}`);

            openIdImplicitFlowConfiguration.forbidden_route = '/forbidden';
            openIdImplicitFlowConfiguration.unauthorized_route = '/unauthorized';
            openIdImplicitFlowConfiguration.auto_userinfo = true;
            openIdImplicitFlowConfiguration.log_console_warning_active = true;
            openIdImplicitFlowConfiguration.log_console_debug_active = false;
            openIdImplicitFlowConfiguration.max_id_token_iat_offset_allowed_in_seconds = 10;
            openIdImplicitFlowConfiguration.log_console_debug_active = true;
            openIdImplicitFlowConfiguration.log_console_warning_active = true;

            this.oidcSecurityService.setupModule(openIdImplicitFlowConfiguration);
        });
    }

    configClient() {

        console.log('window.location', window.location);
        console.log('window.location.href', window.location.href);
        console.log('window.location.origin', window.location.origin);
        console.log(`${window.location.origin}/api/ClientAppSettings`);

        return this.http.get(`${window.location.origin}/api/ClientAppSettings`);
    }
}

export function getBaseUrl() {
    return document.getElementsByTagName('base')[0].href;
}