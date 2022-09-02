import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { ChallengeList } from 'src/app/model/challenge.list';
import { GameBadgeList } from 'src/app/model/gamebadge.list';
import { User } from 'src/app/model/user';
import { ChallengeService } from 'src/app/services/challenge.service';
import { GameBadgeService } from 'src/app/services/gamebadge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';
import { faGraduationCap, faPeopleArrows, faTasks } from '@fortawesome/free-solid-svg-icons';
import { GameBadge } from 'src/app/model/gamebadge';
import { NavigationService } from 'src/app/services/navigation.service';
import { UserStatisticsService } from 'src/app/services/userstatistics.service';
import { UserList } from 'src/app/model/userlist';

@Component({
  selector: 'app-user-info',
  templateUrl: './user-info.component.html',
  styleUrls: ['./user-info.component.css']
})
export class UserInfoComponent implements OnInit {
  id: number = 0;
  obj: User = new User();
  accepted_challenges_page = 1;
  accepted_challenges:ChallengeList = new ChallengeList();
  received_badges_page = 1;
  received_badges:GameBadgeList = new GameBadgeList();
  friends:UserList = new UserList();
  friends_page:number = 1;
  
  constructor(
    private logger: LoggingService,
    private navigation: NavigationService,
    private route: ActivatedRoute,
    private router: Router,
    private user_service: UserService,
    private challenge_service: ChallengeService,
    private badge_service: GameBadgeService,
    private user_statistics_service: UserStatisticsService) { 
      this.navigation.startSaveHistory();
    }

  async ngOnInit() {
    this.id = this.route.snapshot.params['id'];
    this.obj = await this.user_service.byId(this.id);
    this.accepted_challenges = await this.challenge_service.getAccepted(this.id);
    this.received_badges = await this.badge_service.getReceivedBadgesOfUser(this.id);
    this.friends = await this.user_service.getFriends(this.id);
  }

  async onAcceptedChallengesPaginate(event) {
    this.accepted_challenges_page = event;
    this.accepted_challenges = await this.challenge_service.getAccepted(this.id, event);
  }

  async onReceivedBadgesPaginate(event) {
    this.received_badges_page = event;
    this.received_badges = await this.badge_service.getReceivedBadgesOfUser(this.id, event);
  }

  async onFriendsPaginate(event) {
    this.friends_page = event;
    this.friends = await this.user_service.getFriends(this.id, event);
  }

  getCategorgyIcon(obj: Challenge) {
    if (obj.category == 1)
      return faGraduationCap;

    if (obj.category == 2)
      return faPeopleArrows;

    return faTasks;
  }

  onEditChallenge(obj:Challenge) {
    this.logger.debug("User wants to edit a Challenge");
    this.router.navigate(['/challenge', obj.id]);
  }

  onEditBadge(obj:GameBadge) {
    this.logger.debug("User wants to edit a GameBadge");
    this.router.navigate(['/gamebadge', obj.id]);
  }

  onEditUser(obj:User) {
    this.logger.debug("User wants to edit a User");
    this.router.navigate(['/user', obj.id]);
  }

  goBack() {
    this.navigation.goBack();
  }
}