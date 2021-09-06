import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthorizationService } from './services/authorization.service';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './auth/login/login.component';
import { LogoutComponent } from './auth/logout/logout.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { StudyProgramListComponent } from './studyprogram/study-program-list/study-program-list.component';
import { StudyProgramEditComponent } from './studyprogram/study-program-edit/study-program-edit.component';
import { StudyProgramService } from './services/study-program.service';
import { TermEditComponent } from './term/term-edit/term-edit/term-edit.component';
import { TermListComponent } from './term/term-list/term-list/term-list.component';
import { TermService } from './services/term.service';
import { RegisterUserComponent } from './user/register-user/register-user.component';
import { UserService } from './services/user.service';
import { environment } from 'src/environments/environment';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { RegisterUserSuccessComponent } from './user/register-user-success/register-user-success.component';
import { TeamListComponent } from './team/team-list/team-list.component';
import { TeamEditComponent } from './team/team-edit/team-edit.component';
import { TeamService } from './services/team.service';
import { UserSelectComponent } from './user/user-select/user-select.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { InfoComponent } from './misc/info/info.component';
import { ImprintComponent } from './misc/imprint/imprint.component';
import { PrivacyComponent } from './misc/privacy/privacy.component';
import { ChallengeListComponent } from './challenge/challenge-list/challenge-list.component';
import { ChallengeEditComponent } from './challenge/challenge-edit/challenge-edit.component';
import { ChallengeService } from './services/challenge.service';
import { CategoryPipe } from './services/category.pipe';
import { HttpClientModule } from '@angular/common/http';
import { CreateSeriesComponent } from './challenge/create-series/create-series.component';
import { GameBadgeListComponent } from './gamebadge/game-badge-list/game-badge-list.component';
import { GameBadgeEditComponent } from './gamebadge/game-badge-edit/game-badge-edit.component';
import { GameBadgeService } from './services/gamebadge.service';

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    LogoutComponent,
    PageNotFoundComponent,
    StudyProgramListComponent,
    StudyProgramEditComponent,
    TermEditComponent,
    TermListComponent,
    RegisterUserComponent,
    UserListComponent,
    UserEditComponent,
    RegisterUserSuccessComponent,
    TeamListComponent,
    TeamEditComponent,
    UserSelectComponent,
    InfoComponent,
    ImprintComponent,
    PrivacyComponent,
    ChallengeListComponent,
    ChallengeEditComponent,
    CategoryPipe,
    CreateSeriesComponent,
    GameBadgeListComponent,
    GameBadgeEditComponent,
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FontAwesomeModule
  ],
  providers: [
    AuthorizationService,
    StudyProgramService,
    TermService,
    UserService,
    TeamService,
    ChallengeService,
    GameBadgeService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
