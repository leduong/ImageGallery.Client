import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { SharedModule } from '../../shared/shared.module';
import { LoginComponent } from './login/login.component';
import { RegisterComponent } from './register/register.component';
import { RecoverComponent } from './recover/recover.component';
import { MaintenanceComponent } from './maintenance/maintenance.component';

import { ReCaptchaService } from '../../reCaptchaCallback'

/* Use this routes definition in case you want to make them lazy-loaded */

@NgModule({
  imports: [
    SharedModule,
  ],
  declarations: [
    LoginComponent,
    RegisterComponent,
    RecoverComponent,
    MaintenanceComponent,
  ],
  providers: [
    ReCaptchaService
  ],
  exports: [
    RouterModule,
    LoginComponent,
    RegisterComponent,
    RecoverComponent,
    MaintenanceComponent,
  ]
})
export class PagesModule { }
