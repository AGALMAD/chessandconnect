import { Routes } from '@angular/router';
import { HomeComponent } from './pages/home/home.component';
import { LoginComponent } from './pages/login/login.component';
import { RegisterComponent } from './pages/register/register.component';
import { ChessMenuComponent } from './pages/chess-menu/chess-menu.component';
import { redirectionGuard } from './guards/redirection.guard.component';
import { Connect4MenuComponent } from './pages/connect4-menu/connect4-menu.component';
import { UserProfileComponent } from './pages/user-profile/user-profile.component';

export const routes: Routes = [
    {path: "", component: HomeComponent},
    {path: "login", component: LoginComponent},
    {path: "register", component: RegisterComponent},
    {path: "chess", component: ChessMenuComponent, canActivate: [redirectionGuard]},
    {path: "connect4", component: Connect4MenuComponent, canActivate: [redirectionGuard]},
    {path: "profile", component: UserProfileComponent,canActivate: [redirectionGuard]}

];
