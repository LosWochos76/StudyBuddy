import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { GameBadgeService } from 'src/app/services/gamebadge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-game-badge-success',
  templateUrl: './game-badge-success.component.html',
  styleUrls: ['./game-badge-success.component.css']
})
export class GameBadgeSuccessComponent implements OnInit {
  id: number = 0;
  objects: User[] = [];
  selected: User = null;
  current_user: User = null;

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private user_service: UserService,
    private badge_service: GameBadgeService,
    private router: Router,
    private auth: AuthorizationService,) { 
      this.current_user = this.auth.getUser();
    }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    //this.objects = await this.user_service.getAcceptedUsersOfChallenge(this.id);
  }

  onSelect(obj: User) {
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  async onRemove() {
    if (!this.current_user.isAdmin || !this.isSelected())
      return;

    this.logger.debug("User wants to remove of  of a user from a challenge");
    //await this.challenge_service.removeAcceptance(this.id, this.selected.id);
    //this.objects = await this.user_service.getAcceptedUsersOfChallenge(this.id);
  }

  goBack() {
    this.router.navigate(['challenge']);
  }
}