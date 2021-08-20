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
    RegisterUserComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    AngularFireModule.initializeApp(environment.firebase),
    AngularFireDatabaseModule,
    AngularFireAuthModule
  ],
  providers: [
    AuthorizationService,
    StudyProgramService,
    TermService,
    UserService
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
