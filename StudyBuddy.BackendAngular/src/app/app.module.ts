import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { AuthorizationService } from './shared/authorization.service';
import { ReactiveFormsModule } from '@angular/forms';
import { AngularFireModule } from '@angular/fire';
import { AngularFireDatabaseModule } from '@angular/fire/database';
import { AngularFireAuthModule } from '@angular/fire/auth';
import { LoginComponent } from './auth/login/login.component';
import { LogoutComponent } from './auth/logout/logout.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { StudyProgramListComponent } from './studyprogram/study-program-list/study-program-list.component';
import { StudyProgramEditComponent } from './studyprogram/study-program-edit/study-program-edit.component';
import { StudyProgramService } from './shared/study-program.service';
import { TermEditComponent } from './term/term-edit/term-edit/term-edit.component';
import { TermListComponent } from './term/term-list/term-list/term-list.component';
import { TermService } from './shared/term.service';
import { RegisterUserComponent } from './user/register-user/register-user.component';
import { UserService } from './shared/user.service';
import { environment } from 'src/environments/environment';
import { UserListComponent } from './user/user-list/user-list.component';
import { UserEditComponent } from './user/user-edit/user-edit.component';
import { RegisterUserSuccessComponent } from './user/register-user-success/register-user-success.component';
import { TeamListComponent } from './team/team-list/team-list.component';
import { TeamEditComponent } from './team/team-edit/team-edit.component';
import { TeamService } from './shared/team.service';
import { UserSelectComponent } from './user/user-select/user-select.component';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { InfoComponent } from './misc/info/info.component';

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
    InfoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    AngularFireModule.initializeApp(environment.firebase),
    AngularFireDatabaseModule,
    AngularFireAuthModule,
    FontAwesomeModule
  ],
  providers: [
    AuthorizationService,
    StudyProgramService,
    TermService,
    UserService,
    TeamService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
