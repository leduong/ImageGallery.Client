import { LayoutComponent } from '../layout/layout.component';

import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { RecoverComponent } from './pages/recover/recover.component';
import { LockComponent } from './pages/lock/lock.component';
import { MaintenanceComponent } from './pages/maintenance/maintenance.component';
import { Error404Component } from './pages/error404/error404.component';
import { Error500Component } from './pages/error500/error500.component';

import { GalleryComponent } from './gallery/gallery/gallery.component';
import { GalleryEditComponent } from './gallery/gallery-edit/gallery-edit.component';
import { GalleryModule } from './gallery/gallery.module';
import { GalleryAddComponent } from './gallery/gallery-add/gallery-add.component';
import { AboutComponent } from './gallery/about/about.component';
import { HasPayingUserRoleAuthenticationGuard } from '../guards/hasPayingUserRoleAuthenticationGuard';

export const routes = [

    {
        path: '',
        component: LayoutComponent,
        children: [
            { path: '', component: GalleryComponent },
            { path: 'gallery-add', component: GalleryAddComponent, canActivate: [HasPayingUserRoleAuthenticationGuard] },
            { path: 'gallery-edit/:id', component: GalleryEditComponent },
            { path: 'about', component: AboutComponent }
        ]
    },

    // Not lazy-loaded routes
    { path: 'login', component: LoginComponent },
    { path: 'register', component: RegisterComponent },
    { path: 'recover', component: RecoverComponent },
    { path: 'lock', component: LockComponent },
    { path: 'maintenance', component: MaintenanceComponent },
    { path: '404', component: Error404Component },
    { path: '500', component: Error500Component },

    // Not found
    { path: '**', redirectTo: '' }

];
