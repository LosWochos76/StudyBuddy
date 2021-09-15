import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { LogoutComponent } from './auth/logout/logout.component';
import { ChallengeEditComponent } from './challenge/challenge-edit/challenge-edit.component';
import { ChallengeListComponent } from './challenge/challenge-list/challenge-list.component';
import { ChallengeSelectComponent } from './challenge/challenge-select/challenge-select.component';
import { CreateSeriesComponent } from './challenge/create-series/create-series.component';
import { GameBadgeEditComponent } from './gamebadge/game-badge-edit/game-badge-edit.component';
import { GameBadgeListComponent } from './gamebadge/game-badge-list/game-badge-list.component';
import { ImprintComponent } from './misc/imprint/imprint.component';
import { InfoComponent } from './misc/info/info.component';
import { PrivacyComponent } from './misc/privacy/privacy.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { RequestEditComponent } from './request/request-edit/request-edit.component';
import { RequestListComponent } from './request/request-list/request-list.component';
import { RouteGuardService } from './route-guard';
import { StudyProgramEditComponent } from './studyprogram/study-program-edit/study-program-edit.component';
import { StudyProgramListComponent } from './studyprogram/study-program-list/study-program-list.component';
import { TermEditComponent } from './term/term-edit/term-edit/term-edit.component';
import { TermListComponent } from './term/term-list/term-list/term-list.component';
import { RegisterUserSuccessComponent } from './user/register-user-success/register-user-success.component';
import { RegisterUserComponent } from './user/register-user/register-user.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserSettingsComponent } from './user/user-settings/user-settings.component';

const routes: Routes = [
  { path: 'info', component: InfoComponent },
  { path: 'imprint', component: ImprintComponent },
  { path: 'privacy', component: PrivacyComponent },
  { path: 'register', component: RegisterUserComponent },
  { path: 'registersuccess', component: RegisterUserSuccessComponent },
  { path: 'challenge', component: ChallengeListComponent, canActivate: [RouteGuardService] },
  { path: 'challenge/:id', component: ChallengeEditComponent, canActivate: [RouteGuardService] },
  { path: 'challenge/create_series/:id', component: CreateSeriesComponent, canActivate: [RouteGuardService] },
  { path: 'gamebadge', component: GameBadgeListComponent, canActivate: [RouteGuardService] },
  { path: 'gamebadge/:id', component: GameBadgeEditComponent, canActivate: [RouteGuardService] },
  { path: 'user', component: UserListComponent, canActivate: [RouteGuardService] },
  { path: 'user/:id', component: UserEditComponent, canActivate: [RouteGuardService] },
  { path: 'usersettings', component: UserSettingsComponent, canActivate: [RouteGuardService] },
  { path: 'studyprogram', component: StudyProgramListComponent, canActivate: [RouteGuardService] },
  { path: 'studyprogram/:id', component: StudyProgramEditComponent, canActivate: [RouteGuardService] },
  { path: 'term', component: TermListComponent, canActivate: [RouteGuardService] },
  { path: 'term/:id', component: TermEditComponent, canActivate: [RouteGuardService] },
  { path: 'request', component: RequestListComponent, canActivate: [RouteGuardService] },
  { path: 'request/:id', component: RequestEditComponent, canActivate: [RouteGuardService] },
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
  { path: '', redirectTo: 'info', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
