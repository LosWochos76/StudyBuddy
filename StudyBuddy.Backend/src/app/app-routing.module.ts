import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './authentication/login/login.component';
import { LogoutComponent } from './authentication/logout/logout.component';
import { ChallengeEditComponent } from './challenge/challenge-edit/challenge-edit.component';
import { ChallengeListComponent } from './challenge/challenge-list/challenge-list.component';
import { CreateSeriesComponent } from './challenge/create-series/create-series.component';
import { ChallengeSuccessComponent } from './challenge/challenge-success/challenge-success.component';
import { GameBadgeEditComponent } from './gamebadge/game-badge-edit/game-badge-edit.component';
import { GameBadgeListComponent } from './gamebadge/game-badge-list/game-badge-list.component';
import { PageNotFoundComponent } from './misc/page-not-found/page-not-found.component';
import { RequestEditComponent } from './request/request-edit/request-edit.component';
import { RequestListComponent } from './request/request-list/request-list.component';
import { RouteGuardService } from './route-guard';
import { TagEditComponent } from './tag/tag-edit/tag-edit.component';
import { TagListComponent } from './tag/tag-list/tag-list.component';
import { RegisterUserSuccessComponent } from './user/register-user-success/register-user-success.component';
import { RegisterUserComponent } from './user/register-user/register-user.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserSettingsComponent } from './user/user-settings/user-settings.component';
import { NotificationPageComponent } from "./notification/notification-page/notification-page.component";
import { NotificationBroadcastComponent } from "./notification/notification-broadcast/notification-broadcast.component";
import { FcmTokenListComponent } from './notification/fcm-token-list/fcm-token-list.component';
import { LoggingListComponent } from './misc/logging-list/logging-list.component';
import { GameBadgeSuccessComponent } from './gamebadge/game-badge-success/game-badge-success.component';
import { BusinessEventListComponent } from './businessevent/businessevent-list/businessevent-list.component';
import { BusinessEventEditComponent } from './businessevent/businessevent-edit/businessevent-edit.component';
import { ForgotpasswordComponent } from './authentication/forgotpassword/forgotpassword.component';
import { ResetpasswordComponent } from './authentication/resetpassword/resetpassword.component';
import { UserInfoComponent } from './user/user-info/user-info.component';
import { NotverifiedComponent } from './authentication/notverified/notverified.component';
import { VerifyemailComponent } from './authentication/verifyemail/verifyemail.component';

const routes: Routes = [
  { path: 'register', component: RegisterUserComponent },
  { path: 'registersuccess', component: RegisterUserSuccessComponent },
  { path: 'challenge', component: ChallengeListComponent, canActivate: [RouteGuardService] },
  { path: 'challenge/:id', component: ChallengeEditComponent, canActivate: [RouteGuardService] },
  { path: 'challenge/create_series/:id', component: CreateSeriesComponent, canActivate: [RouteGuardService] },
  { path: 'challengesuccess/:id', component: ChallengeSuccessComponent, canActivate: [RouteGuardService] },
  { path: 'gamebadge', component: GameBadgeListComponent, canActivate: [RouteGuardService] },
  { path: 'gamebadge/:id', component: GameBadgeEditComponent, canActivate: [RouteGuardService] },
  { path: 'gamebadgesuccess/:id', component: GameBadgeSuccessComponent, canActivate: [RouteGuardService] },
  { path: 'user', component: UserListComponent, canActivate: [RouteGuardService] },
  { path: 'user/:id', component: UserEditComponent, canActivate: [RouteGuardService] },
  { path: 'userinfo/:id', component: UserInfoComponent, canActivate: [RouteGuardService] },
  { path: 'usersettings', component: UserSettingsComponent, canActivate: [RouteGuardService] },
  { path: 'request', component: RequestListComponent, canActivate: [RouteGuardService] },
  {
    path: 'notification', component: NotificationPageComponent, canActivate: [RouteGuardService],
    children: [
      { path: 'broadcast', component: NotificationBroadcastComponent },
      { path: 'tokens', component: FcmTokenListComponent },
      { path: '', redirectTo: 'broadcast', pathMatch: 'full' },
    ]
  },
  { path: 'request/:id', component: RequestEditComponent, canActivate: [RouteGuardService] },
  { path: 'tag', component: TagListComponent, canActivate: [RouteGuardService] },
  { path: 'tag/:id', component: TagEditComponent, canActivate: [RouteGuardService] },
  { path: 'login', component: LoginComponent },
  { path: 'login/forgotpassword', component: ForgotpasswordComponent },
  { path: 'login/resetpassword', component: ResetpasswordComponent },
  { path: 'login/verificationrequired', component: NotverifiedComponent },
  { path: 'login/verifyemail', component: VerifyemailComponent },
  { path: 'logout', component: LogoutComponent },
  { path: 'logging', component: LoggingListComponent, canActivate: [RouteGuardService] },
  { path: 'businessevent', component: BusinessEventListComponent, canActivate: [RouteGuardService] },
  { path: 'businessevent/:id', component: BusinessEventEditComponent, canActivate: [RouteGuardService] },
  { path: '', redirectTo: 'challenge', pathMatch: 'full' },
  { path: '**', component: PageNotFoundComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
