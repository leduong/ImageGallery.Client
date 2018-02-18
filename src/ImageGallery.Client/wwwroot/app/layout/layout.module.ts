import { NgModule } from '@angular/core';

import { LayoutComponent } from './layout.component';

import { SharedModule } from '../shared/shared.module';
import { UserManagementService } from '../services/user.service';

@NgModule({
    imports: [
        SharedModule
    ],
    declarations: [
        LayoutComponent
    ],
    exports: [
        LayoutComponent
    ],
    providers: [
        UserManagementService,
    ]
})
export class LayoutModule { }
