import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { StudyProgram } from 'src/app/model/studyprogram';
import { Term } from 'src/app/model/term';
import { User } from 'src/app/model/user';
import { LoggingService } from 'src/app/services/loging.service';
import { passwordMatchValidator } from 'src/app/services/passwordMatchValidator';
import { StudyProgramService } from 'src/app/services/study-program.service';
import { TermService } from 'src/app/services/term.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-register-user',
  templateUrl: './register-user.component.html',
  styleUrls: ['./register-user.component.css']
})
export class RegisterUserComponent implements OnInit {
  form: FormGroup;
  study_programs: StudyProgram[] = [];
  terms: Term[] = [];

  constructor(
    private logger: LoggingService,
    private router: Router,
    private service: UserService,
    private study_program_service: StudyProgramService,
    private term_service: TermService) {

    this.form = new FormGroup({
      firstname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      lastname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      nickname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", [Validators.required, Validators.minLength(6)]),
      password_confirm: new FormControl("", [Validators.required, Validators.minLength(6)]),
      study_program_id: new FormControl(""),
      enrolled_since_id: new FormControl("")
    }, {
      validators: passwordMatchValidator,
      updateOn: 'blur'
    });
  };

  async ngOnInit() {
    this.study_programs = await this.study_program_service.getAll();
    this.terms = await this.term_service.getAll();
  }

  async onSubmit() {
    this.logger.debug("Trying to register a user!");
    let obj = new User();
    obj.copyValues(this.form.value);

    if (!obj.email.endsWith("@hshl.de") && !obj.email.endsWith("@stud.hshl.de")) {
      this.form.setErrors({ 'emailnotfromhshl': true });
      return;
    }

    let result = await this.service.userIdByNickname(obj.nickname);
    if (result != 0) {
      this.form.setErrors({ 'nicknamealreadyinuse': true });
      return;
    }

    result = await this.service.userIdByEmail(obj.email);
    if (result != 0 && result != obj.id) {
      this.form.setErrors({ 'emailalreadyinuse': true });
      return;
    }

    if (this.form.invalid) {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.service.save(obj);
    this.router.navigate(['registersuccess']);
  }
}