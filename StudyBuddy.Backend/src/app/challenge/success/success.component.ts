import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ChallengeService } from 'src/app/services/challenge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-success',
  templateUrl: './success.component.html',
  styleUrls: ['./success.component.css']
})
export class SuccessComponent implements OnInit {
  id: number = 0;
  objects: User[] = [];
  selected: User = null;
  current_user: User = null;

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private user_service: UserService,
    private challenge_service: ChallengeService,
    private router: Router,
    private auth: AuthorizationService,) { 
      this.current_user = this.auth.getUser();
    }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    this.objects = await this.user_service.getAcceptedUsersOfChallenge(this.id);
  }

  onSelect(obj: User) {
    this.logger.debug("User selected a Challenge");
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  async onRemove() {
    if (!this.current_user.isAdmin || !this.isSelected())
      return;

    this.logger.debug("User wants to remove the acceptance of a user from a challenge");
    await this.challenge_service.removeAcceptance(this.id, this.selected.id);
    this.objects = await this.user_service.getAcceptedUsersOfChallenge(this.id);
  }

  goBack() {
    this.router.navigate(['challenge']);
  }
}
