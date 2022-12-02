import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ChallengeService } from 'src/app/services/challenge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { UserService } from 'src/app/services/user.service';


@Component({
  selector: 'app-challenge-edit',
  templateUrl: './challenge-edit.component.html',
  styleUrls: ['./challenge-edit.component.css']
})
export class ChallengeEditComponent implements OnInit {
  id: number = 0;
  obj: Challenge = null;
  form: FormGroup;
  user: User = null;
  all_users: User[] = [];

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private service: ChallengeService,
    private auth: AuthorizationService,
    private user_service: UserService,
    private navigation: NavigationService) {
    this.navigation.startSaveHistory();
    this.user = this.auth.getUser();

    this.form = new FormGroup({
      name: new FormControl("", [Validators.required, Validators.minLength(3)]),
      description: new FormControl(""),
      points: new FormControl(0, [Validators.required, Validators.pattern("^[1-9][0-9]*$")]),
      validity_start: new FormControl("", [Validators.required]),
      validity_end: new FormControl("", [Validators.required]),
      category: new FormControl("", [Validators.required]),
      latitude: new FormControl("", [Validators.pattern('\\-?\\d*\\.?\\d*')]),
      longitude: new FormControl("", [Validators.pattern('\\-?\\d*\\.?\\d*')]),
      radius: new FormControl("", [Validators.pattern('\\-?\\d*\\.?\\d*')]),
      keyword: new FormControl(""),
      prove: new FormControl("", [Validators.required]),
      tags: new FormControl("")
    });

    if (this.user.isAdmin())
      this.form.addControl("owner_id", new FormControl(0));
  };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    let tags = "";

    if (this.id != 0) {
      this.obj = await this.service.byId(this.id);
    } else {
      this.obj = new Challenge();
      this.obj.owner_id = this.auth.getUser().id;
    }

    if (this.user.isAdmin())
      this.all_users = (await this.user_service.getAll()).objects;

    let keyword = "";
    let latitude = 51.682730;
    let longitude = 7.840930;
    let radius = 100;

    if (this.obj.prove == 4) {
      var parts = this.obj.prove_addendum.split(";");
      latitude = +parts[0];
      longitude = +parts[1];
      radius = +parts[2];
    }

    if (this.obj.prove == 5) {
      keyword = this.obj.prove_addendum;
    }

    if (this.user.isAdmin()) {
      this.form.setValue({
        name: this.obj.name,
        description: this.obj.description,
        points: this.obj.points,
        validity_start: this.obj.validity_start,
        validity_end: this.obj.validity_end,
        category: this.obj.category,
        prove: this.obj.prove,
        owner_id: this.obj.owner_id,
        tags: this.obj.tags,
        latitude: latitude,
        longitude: longitude,
        radius: radius,
        keyword: keyword
      });
    } else {
      this.form.setValue({
        name: this.obj.name,
        description: this.obj.description,
        points: this.obj.points,
        validity_start: this.obj.validity_start,
        validity_end: this.obj.validity_end,
        category: this.obj.category,
        prove: this.obj.prove,
        tags: this.obj.tags,
        latitude: latitude,
        longitude: longitude,
        radius: radius,
        keyword: keyword
      });
    }
  }

  async onSubmit() {
    this.logger.debug("Trying to save a Challenge!");
    this.obj.copyValues(this.form.value);

    if (!this.obj.hasName())
      this.form.controls.name.setErrors({ 'invalidname': true });
    else
      this.form.controls.name.setErrors(null);

    if (!this.obj.isPeriodValid())
      this.form.controls.validity_end.setErrors({ 'invalidperiod': true });
    else
      this.form.controls.validity_end.setErrors(null);

    if (this.obj.prove == 5 && !this.obj.hasKeyword())
      this.form.controls.keyword.setErrors({ 'invalidkeyword': true });
    else
      this.form.controls.keyword.setErrors(null);

    if (!this.obj.validDescription())
      this.form.controls.description.setErrors({ 'invaliddescription': true });
    else
      this.form.controls.description.setErrors(null);

    if (this.form.invalid) {
      this.logger.debug("Data is invalid!");
      return;
    }

    await this.service.save(this.obj);
    this.navigation.goBack();
  }

  onCancel() {
    this.navigation.goBack();
  }
}