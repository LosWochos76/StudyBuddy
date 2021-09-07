import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { GameBadge } from 'src/app/model/gamebadge';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ChallengeService } from 'src/app/services/challenge.service';
import { GameBadgeService } from 'src/app/services/gamebadge.service';
import { LoggingService } from 'src/app/services/loging.service';

@Component({
  selector: 'app-game-badge-edit',
  templateUrl: './game-badge-edit.component.html',
  styleUrls: ['./game-badge-edit.component.css']
})
export class GameBadgeEditComponent implements OnInit {
  id:number = 0;
  obj:GameBadge = null;
  form:FormGroup;

  constructor(
    private logger:LoggingService, 
    private route:ActivatedRoute,
    private router:Router,
    private service:GameBadgeService,
    private auth:AuthorizationService,
    private challenge_service:ChallengeService) { 
      this.form = new FormGroup({
        name: new FormControl("", [Validators.required, Validators.minLength(3)]),
        required_coverage: new FormControl(0.5, [Validators.required]),
        challenges: new FormControl([], [Validators.required])
      });
    }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    let challenges:number[] = [];

    if (this.id != 0) {
      this.obj = await this.service.byId(this.id);
      challenges = await this.challenge_service.ofBadge(this.id);
    } else {
      this.obj = new GameBadge();
      this.obj.owner = this.auth.getUser().id;
    }

    this.form.setValue({
      name: this.obj.name,
      required_coverage: this.obj.required_coverage,
      challenges: challenges
    });
  }

  async onSubmit() {
    this.logger.debug("Trying to save a GameBadge!");
    this.obj.copyValues(this.form.value);
    let challenges = this.form.controls.challenges.value;

    if (challenges.length == 0) {
      this.form.setErrors({'missingchallenge': true});
      return;
    }

    if (this.form.invalid)
    {
      this.logger.debug("Data is invalid!");
      return;
    }

    await this.service.save(this.obj);
    await this.service.setChallenges(this.obj.id, challenges);
    this.router.navigate(["gamebadge"]);
  }

  onCancel() {
    this.router.navigate(['gamebadge']);
  }
}