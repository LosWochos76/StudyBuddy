import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthorizationService } from './services/authorization.service';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './authentication/login/login.component';
import { LogoutComponent } from './authentication/logout/logout.component';
import { PageNotFoundComponent } from './misc/page-not-found/page-not-found.component';
import { RegisterUserComponent } from './user/register-user/register-user.component';
import { UserService } from './services/user.service';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { RegisterUserSuccessComponent } from './user/register-user-success/register-user-success.component';
import { UserSelectComponent } from './user/user-select/user-select.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { ChallengeListComponent } from './challenge/challenge-list/challenge-list.component';
import { ChallengeEditComponent } from './challenge/challenge-edit/challenge-edit.component';
import { ChallengeService } from './services/challenge.service';
import { CategoryPipe } from './services/category.pipe';
import { HttpClientModule } from '@angular/common/http';
import { CreateSeriesComponent } from './challenge/create-series/create-series.component';
import { GameBadgeListComponent } from './gamebadge/game-badge-list/game-badge-list.component';
import { GameBadgeEditComponent } from './gamebadge/game-badge-edit/game-badge-edit.component';
import { GameBadgeService } from './services/gamebadge.service';
import { ChallengeSelectComponent } from './challenge/challenge-select/challenge-select.component';
import { UserSettingsComponent } from './user/user-settings/user-settings.component';
import { RequestService } from './services/request.service';
import { RequestTypePipe } from './services/requesttype.pipe';
import { RequestEditComponent } from './request/request-edit/request-edit.component';
import { RequestListComponent } from './request/request-list/request-list.component';
import { TagListComponent } from './tag/tag-list/tag-list.component';
import { TagEditComponent } from './tag/tag-edit/tag-edit.component';
import { ChallengeSuccessComponent } from './challenge/challenge-success/challenge-success.component';
import { BroadcastComponent } from './broadcast/broadcast/broadcast.component';
import { FcmTokenListComponent } from './broadcast/fcm-token-list/fcm-token-list.component';
import { LoggingListComponent } from './misc/logging-list/logging-list.component';
import { GameBadgeSuccessComponent } from './gamebadge/game-badge-success/game-badge-success.component';
import { BusinessEventListComponent } from './businessevent/businessevent-list/businessevent-list.component';
import { BusinessEventEditComponent } from './businessevent/businessevent-edit/businessevent-edit.component';
import { NgxPaginationModule } from 'ngx-pagination';
import { ForgotpasswordComponent } from './authentication/forgotpassword/forgotpassword.component';
import { ResetpasswordComponent } from './authentication/resetpassword/resetpassword.component';
import { UserInfoComponent } from './user/user-info/user-info.component';
import { NavigationService } from './services/navigation.service';
import { UserStatisticsService } from './services/userstatistics.service';
import { NgxEchartsModule } from 'ngx-echarts';
import { NotverifiedComponent } from './authentication/notverified/notverified.component';
import { VerifyemailComponent } from './authentication/verifyemail/verifyemail.component';
import { RecoverComponent } from './authentication/recover/recover.component';
import { SendEmailComponent } from './user/send-email/send-email.component';
import { NotificationListComponent } from './notification/notification-list/notification-list.component';
import { NotificationEditComponent } from './notification/notification-edit/notification-edit.component';
import { NotificationUserMetadataListComponent } from './notification-user-metadata-list/notification-metadata-list/notification-user-metadata-list.component';
import { FormsModule } from '@angular/forms';

@NgModule({
  declarations: [
    AppComponent,
    CategoryPipe,
    RequestTypePipe,
    LoginComponent,
    LogoutComponent,
    PageNotFoundComponent,
    RegisterUserComponent,
    UserListComponent,
    UserEditComponent,
    RegisterUserSuccessComponent,
    UserSelectComponent,
    ChallengeListComponent,
    ChallengeEditComponent,
    CreateSeriesComponent,
    GameBadgeListComponent,
    GameBadgeEditComponent,
    ChallengeSelectComponent,
    UserSettingsComponent,
    RequestEditComponent,
    RequestListComponent,
    TagListComponent,
    TagEditComponent,
    ChallengeSuccessComponent,
    BroadcastComponent,
    FcmTokenListComponent,
    LoggingListComponent,
    GameBadgeSuccessComponent,
    BusinessEventListComponent,
    BusinessEventEditComponent,
    ForgotpasswordComponent,
    ResetpasswordComponent,
    UserInfoComponent,
    NotverifiedComponent,
    VerifyemailComponent,
    RecoverComponent,
    SendEmailComponent,
    NotificationListComponent,
    NotificationEditComponent,
    NotificationUserMetadataListComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FontAwesomeModule,
      NgxPaginationModule,
      FormsModule,
    NgxEchartsModule.forRoot({
      echarts: () => import('echarts')
    }),
  ],
  providers: [
    AuthorizationService,
    UserService,
    ChallengeService,
    GameBadgeService,
    RequestService,
    UserStatisticsService,
    NavigationService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
