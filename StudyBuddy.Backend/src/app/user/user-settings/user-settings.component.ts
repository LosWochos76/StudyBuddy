import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';
import { StudyProgramService } from 'src/app/services/study-program.service';
import { TermService } from 'src/app/services/term.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-settings',
  templateUrl: './user-settings.component.html',
  styleUrls: ['./user-settings.component.css']
})
export class UserSettingsComponent implements OnInit {
  obj: User = null;
  form: FormGroup;

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private router: Router,
    private service: UserService,
    private study_program_service: StudyProgramService,
    private term_service: TermService,
    private auth: AuthorizationService) {

    this.form = new FormGroup({
      firstname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      lastname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      nickname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl(""),
      password_confirm: new FormControl(""),
      role: new FormControl(1)
    });
  };

  async ngOnInit() {
    this.obj = this.auth.getUser()

    this.form.setValue({
      firstname: this.obj.firstname,
      lastname: this.obj.lastname,
      nickname: this.obj.nickname,
      email: this.obj.email,
      password: "",
      password_confirm: "",
      role: this.obj.role
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save settings of current user!");
    this.obj.copyValues(this.form.value);

    if (!this.obj.email.endsWith("@hshl.de") && !this.obj.email.endsWith("@stud.hshl.de")) {
      this.form.setErrors({ 'emailnotfromhshl': true });
      return;
    }

    let result = await this.service.userIdByNickname(this.form.controls.nickname.value);
    if (result != 0 && result != this.obj.id) {
      this.form.setErrors({ 'nicknamealreadyinuse': true });
      return;
    }

    result = await this.service.userIdByEmail(this.obj.email);
    if (result != 0 && result != this.obj.id) {
      this.form.setErrors({ 'emailalreadyinuse': true });
      return;
    }

    if (this.obj.password != "") {
      if (this.obj.password.length < 6) {
        this.form.setErrors({ 'passwordunsafe': true });
        return;
      }

      if (this.obj.password != this.form.controls.password_confirm.value) {
        this.form.setErrors({ 'passwordmismatch': true });
        return;
      }
    }

    if (this.form.invalid) {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.service.save(this.obj);
    this.router.navigate(['challenge']);
  }

  onCancel() {
    this.router.navigate(['challenge']);
  }

  onSendResetPasswortEmail() {
    this.auth.sendPassworResetMail(this.obj.email);
  }
}