import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { ChallengeList } from 'src/app/model/challengelist';
import { GameBadgeList } from 'src/app/model/gamebadgelist';
import { User } from 'src/app/model/user';
import { ChallengeService } from 'src/app/services/challenge.service';
import { GameBadgeService } from 'src/app/services/gamebadge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';
import { faGraduationCap, faPeopleArrows, faTasks } from '@fortawesome/free-solid-svg-icons';
import { GameBadge } from 'src/app/model/gamebadge';
import { NavigationService } from 'src/app/services/navigation.service';
import { UserStatisticsService } from 'src/app/services/userstatistics.service';
import { Score } from 'src/app/model/score';
import { UserList } from 'src/app/model/userlist';
import { EChartsOption } from 'echarts/types/dist/echarts';

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
  score:Score = new Score();
  friends:UserList = new UserList();
  friends_page:number = 1;
  chartOption: EChartsOption;
  
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
    this.score = await this.user_statistics_service.getScore(this.id);
    this.friends = await this.user_service.getFriends(this.id);

    await this.loadDataForChart();
  }

  async loadDataForChart() {
    var trend = await this.user_statistics_service.getTrend(this.id);

    this.chartOption = {
      tooltip: {
        trigger: 'axis',
        axisPointer: {
          type: 'cross',
          label: {
            backgroundColor: '#6a7985'
          }
        }
      },
      xAxis: {
        type: 'category',
        boundaryGap: false,
        data: trend.getXAxis(),
      },
      yAxis: {
        type: 'value',
      },
      legend: {},
      series: [
        {
          name: 'Lernen',
          type: 'line',
          stack: 'Total',
          data: trend.getLearningSeries(),
          areaStyle: {},
          emphasis: {
            focus: 'series'
          },
          smooth: true,
        },
        {
          name: 'Netzwerken',
          type: 'line',
          stack: 'Total',
          data: trend.getNetworkingSeries(),
          areaStyle: {},
          emphasis: {
            focus: 'series'
          },
          smooth: true,
        },
        {
          name: 'Organisieren',
          type: 'line',
          stack: 'Total',
          data: trend.getOrganizingSeries(),
          areaStyle: {},
          emphasis: {
            focus: 'series'
          },
          smooth: true,
        },
      ],
    };
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