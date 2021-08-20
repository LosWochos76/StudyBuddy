import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './auth/login/login.component';
import { LogoutComponent } from './auth/logout/logout.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { RouteGuardService } from './route-guard';
import { StudyProgramEditComponent } from './studyprogram/study-program-edit/study-program-edit.component';
import { StudyProgramListComponent } from './studyprogram/study-program-list/study-program-list.component';
import { TermEditComponent } from './term/term-edit/term-edit/term-edit.component';
import { TermListComponent } from './term/term-list/term-list/term-list.component';
import { RegisterUserComponent } from './user/register-user/register-user.component';

const routes: Routes = [
  { path: 'register', component: RegisterUserComponent },
  { path: 'studyprogram', component: StudyProgramListComponent, canActivate: [RouteGuardService]},
  { path: 'studyprogram/:id', component: StudyProgramEditComponent, canActivate: [RouteGuardService]},
  { path: 'term', component: TermListComponent, canActivate: [RouteGuardService]},
  { path: 'term/:id', component: TermEditComponent, canActivate: [RouteGuardService]},
  { path: 'login', component: LoginComponent },
  { path: 'logout', component: LogoutComponent },
  { path: '', redirectTo: 'term', pathMatch: 'full'},
  { path: '**', component: PageNotFoundComponent } 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
