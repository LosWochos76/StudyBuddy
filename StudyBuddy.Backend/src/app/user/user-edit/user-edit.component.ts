import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-edit',
  templateUrl: './user-edit.component.html',
  styleUrls: ['./user-edit.component.css']
})
export class UserEditComponent implements OnInit {
  id: number = 0;
  obj: User = null;
  form: FormGroup;

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private router: Router,
    private service: UserService,
    private auth: AuthorizationService) {

    this.form = new FormGroup({
      firstname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      lastname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      nickname: new FormControl("", [Validators.required, Validators.minLength(3)]),
      role: new FormControl(1),
      friends: new FormControl([])
    });
  };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];

    if (this.id != 0) {
      this.obj = await this.service.byId(this.id);
    } else {
      this.obj = new User();
    }

    this.form.setValue({
      firstname: this.obj.firstname,
      lastname: this.obj.lastname,
      nickname: this.obj.nickname,
      role: this.obj.role,
      friends: await this.service.getFriends(this.id)
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save a user!");
    this.obj.copyValues(this.form.value);

    let result = await this.service.userIdByNickname(this.obj.nickname);
    if (result != 0 && result != this.obj.id) {
      this.form.setErrors({ 'nicknamealreadyinuse': true });
      return;
    }

    let friends = this.form.controls.friends.value;
    if (friends.findIndex(id => id == this.id) > -1) {
      this.form.setErrors({ 'cannotfriendwithself': true });
      return;
    }

    if (this.form.invalid) {
      this.logger.debug("Data is invalid!");
      return;
    }

    this.service.save(this.obj);

    if (this.obj.role == 1) {
      this.service.setFriends(this.obj.id, this.form.controls.friends.value);
    } else {
      this.service.setFriends(this.obj.id, []);
    }

    this.router.navigate(['user']);
  }

  onCancel() {
    this.router.navigate(['user']);
  }

  onSendResetPasswortEmail() {
    this.auth.sendPassworResetMail(this.obj.email);
  }
}