import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { GameBadge } from 'src/app/model/gamebadge';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { GameBadgeService } from 'src/app/services/gamebadge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-game-badge-edit',
  templateUrl: './game-badge-edit.component.html',
  styleUrls: ['./game-badge-edit.component.css']
})
export class GameBadgeEditComponent implements OnInit {
  id: number = 0;
  obj: GameBadge = null;
  form: FormGroup;
  user: User = null;
  all_users: User[] = [];

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private router: Router,
    private service: GameBadgeService,
    private auth: AuthorizationService,
    private user_service: UserService) {
    this.user = this.auth.getUser();

    this.form = new FormGroup({
      name: new FormControl("", [Validators.required, Validators.minLength(3)]),
      description: new FormControl(""),
      required_coverage: new FormControl(0.5, [Validators.required]),
      tags: new FormControl("", [Validators.required])
    });

    if (this.user.isAdmin())
      this.form.addControl("owner", new FormControl(0));
  }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    let tags = "";

    if (this.id != 0) {
      this.obj = await this.service.byId(this.id);
    } else {
      this.obj = new GameBadge();
      this.obj.owner = this.auth.getUser().id;
    }

    if (this.user.isAdmin())
      this.all_users = await this.user_service.getAll();

    if (this.user.isAdmin()) {
      this.form.setValue({
        name: this.obj.name,
        description: this.obj.description,
        required_coverage: this.obj.required_coverage,
        tags: this.obj.tags,
        owner: this.obj.owner
      });
    } else {
      this.form.setValue({
        name: this.obj.name,
        description: this.obj.description,
        required_coverage: this.obj.required_coverage,
        tags: this.obj.tags
      });
    }
  }

  async onSubmit() {
    this.logger.debug("Trying to save a GameBadge!");
    this.obj.copyValues(this.form.value);

    if (this.form.invalid) {
      this.logger.debug("Data is invalid!");
      return;
    }

    await this.service.save(this.obj);
    this.router.navigate(["gamebadge"]);
  }

  onCancel() {
    this.router.navigate(['gamebadge']);
  }
}