import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { GameBadgeList } from 'src/app/model/gamebadge.list';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ChallengeService } from 'src/app/services/challenge.service';
import { GameBadgeService } from 'src/app/services/gamebadge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { NavigationService } from 'src/app/services/navigation.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-success',
  templateUrl: './challenge-success.component.html',
  styleUrls: ['./challenge-success.component.css']
})
export class ChallengeSuccessComponent implements OnInit {
  id: number = 0;
  challenge: Challenge = new Challenge();
  objects: User[] = [];
  selected: User = null;
  current_user: User = null;
  all_users: User[] = [];
  possible_game_badges: GameBadgeList = new GameBadgeList();

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private user_service: UserService,
    private challenge_service: ChallengeService,
    private gamebadge_service: GameBadgeService,
    private router: Router,
    private auth: AuthorizationService,
    private navigation: NavigationService) {
      navigation.startSaveHistory(); 
      this.current_user = this.auth.getUser();
    }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    this.challenge = await this.challenge_service.byId(this.id);
    this.possible_game_badges = await this.gamebadge_service.getBadgesForChallenge(this.id);

    await this.loadObjects();
  }

  async loadObjects() {
    this.objects = (await this.user_service.getUsersThatAcceptedChallenge(this.id)).objects;

    if (!this.current_user.isAdmin())
      return;

    this.all_users = [];
    var result = await this.user_service.getAll();
    var users = result.objects;
    
    for (var user of users) {
      if (!this.hasChallenge(user.id))
        this.all_users.push(user);
    }
  }

  hasChallenge(user_id:number) {
    return (this.objects.find(obj => obj.id == user_id) != undefined);
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

    this.logger.debug("User wants to remove the acceptance of a user from a challenge");
    await this.challenge_service.removeAcceptance(this.id, this.selected.id);
    await this.loadObjects();
  }

  goBack() {
    this.navigation.goBack("/challenge");
  }

  async onAddUser(user_id:number) {
    await this.challenge_service.addAcceptance(this.id, user_id);
    await this.loadObjects();
  }

  onGotoBadge(badge_id:number) {
    this.router.navigate(['/gamebadge/', badge_id]);
  }
}