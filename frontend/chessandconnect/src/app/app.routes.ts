import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { UserProfileComponent } from './pages/user-profile/user-profile.component';

export const routes: Routes = [
    {path: "", component: HomeComponent},
    {path: "log", component: LoginComponent},
    {path: "reg", component: RegisterComponent},
    {path: "userProfile", component: UserProfileComponent}

];
