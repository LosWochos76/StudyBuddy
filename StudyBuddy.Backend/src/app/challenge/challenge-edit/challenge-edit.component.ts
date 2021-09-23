import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { Tag } from 'src/app/model/tag';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ChallengeService } from 'src/app/services/challenge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { TagService } from 'src/app/services/tag.service';
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
    private router: Router,
    private service: ChallengeService,
    private auth: AuthorizationService,
    private user_service: UserService,
    private tag_service: TagService) {
    this.user = this.auth.getUser();

    this.form = new FormGroup({
      name: new FormControl("", [Validators.required, Validators.minLength(3)]),
      description: new FormControl(""),
      points: new FormControl(0, [Validators.required, Validators.pattern("^[1-9][0-9]*$")]),
      validity_start: new FormControl("", [Validators.required]),
      validity_end: new FormControl("", [Validators.required]),
      category: new FormControl("", [Validators.required]),
      prove: new FormControl("", [Validators.required]),
      tags: new FormControl("")
    });

    if (this.user.isAdmin())
      this.form.addControl("owner", new FormControl(0));
  };

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    let tags = "";

    if (this.id != 0) {
      this.obj = await this.service.byId(this.id);
      let tag_list = await this.tag_service.ofChallenge(this.id);
      console.log(tag_list);
      tags = Tag.toTagString(tag_list);
    } else {
      this.obj = new Challenge();
      this.obj.owner = this.auth.getUser().id;
    }

    if (this.user.isAdmin())
      this.all_users = await this.user_service.getAll();

    if (this.user.isAdmin()) {
      this.form.setValue({
        name: this.obj.name,
        description: this.obj.description,
        points: this.obj.points,
        validity_start: this.obj.validity_start,
        validity_end: this.obj.validity_end,
        category: this.obj.category,
        prove: this.obj.prove,
        owner: this.obj.owner,
        tags: tags
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
        tags: tags
      });
    }
  }

  async onSubmit() {
    this.logger.debug("Trying to save a Challenge!");
    this.obj.copyValues(this.form.value);

    if (!this.obj.isPeriodValid())
      this.form.controls.validity_end.setErrors({ 'invalidperiod': true });
    else
      this.form.controls.validity_end.setErrors(null);

    if (this.form.invalid) {
      this.logger.debug("Data is invalid!");
      return;
    }

    await this.service.save(this.obj);
    await this.tag_service.setForChallenge(this.obj.id, this.form.controls.tags.value);
    this.router.navigate(["challenge"]);
  }

  onCancel() {
    this.router.navigate(['challenge']);
  }
}