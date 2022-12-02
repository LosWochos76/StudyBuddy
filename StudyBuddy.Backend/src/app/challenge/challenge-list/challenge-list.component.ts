import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ChallengeService } from 'src/app/services/challenge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { faGraduationCap, faPeopleArrows, faTasks } from '@fortawesome/free-solid-svg-icons';
import { NavigationService } from 'src/app/services/navigation.service';

@Component({
  selector: 'app-challenge-list',
  templateUrl: './challenge-list.component.html',
  styleUrls: ['./challenge-list.component.css']
})
export class ChallengeListComponent implements OnInit {
    form: {
        filter: string;
    };
    config: any;
    objects: Challenge[] = [];
    selected: Challenge = null;
    timeout: any = null;
    user: User = null;
    image;

    constructor(
        private logger: LoggingService,
        private navigation: NavigationService,
        private service: ChallengeService,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private auth: AuthorizationService) {
        this.navigation.startSaveHistory();
        this.activatedRoute = activatedRoute;
        this.router = router;
        this.user = this.auth.getUser();
        this.form = {
            filter: (activatedRoute.snapshot.params.filter || "")
        };
        this.config = {
            currentPage: 1,
            itemsPerPage: 10,
            totalItems: 0
        };
        this.router.navigate([{ filter: this.form.filter }], { queryParams: { page: this.config.currentPage } })
      
    }

    async ngOnInit() {
        this.router.navigate([{ filter: this.form.filter }], { queryParams: { page: this.config.currentPage } })
        this.activatedRoute.queryParams.subscribe(
            params => this.config.currentPage = params['page'] ? params['page'] : 1);
        var result = await this.service.getAll(this.config.currentPage, this.form.filter);
        this.objects = result.objects;
        this.config.totalItems = result.count;
        this.service.changed.subscribe(async () => {
            var result = await this.service.getAll(this.config.currentPage, this.form.filter);
            this.objects = result.objects;
            this.config.totalItems = result.count;
        });
    }

    async onTableDataChange(event) {
        this.config.currentPage = event;
        var fullList = await this.service.getAll(event, this.form.filter);
        this.objects = fullList.objects;
        this.router.navigate([{ filter: this.form.filter }],
            {
                queryParams: { page: event },
                relativeTo: this.activatedRoute,
                replaceUrl: true
            }
        );
    }

  onSelect(obj: Challenge) {
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  isSeriable() {
    return this.selected != null && (this.selected.parent == null || this.selected.parent == 0);
  }

  onDelete() {
    if (!this.isSelected())
      return;

    if (!confirm("Wollen Sie die Herausforderung wirklich löschen?"))
      return;

    this.logger.debug("User wants to delete a Challenge");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onEdit() {
    if (!this.isSelected())
      return;

    this.logger.debug("User wants to edit a Challenge");
    this.router.navigate(['/challenge', this.selected.id]);
  }

  onAdd() {
    this.logger.debug("User wants to add a Challenge");
    this.router.navigate(['/challenge/0']);
  }

  onCreateSeries() {
    if (!this.isSelected())
      return;

    this.logger.debug("User wants to create a clone of a challenge");
    this.router.navigate(['challenge/create_series/', this.selected.id]);
  }
  public applyFilter() {
        this.onSearch(this.form.filter);
        this.applyFilterToRoute();
  }

  private async onSearch(filter: string) {
        clearTimeout(this.timeout);
        var $this = this;
        this.timeout = setTimeout(async function () {
            var result = await $this.service.getAll(this.Page, filter);
            $this.objects = result.objects;
            $this.config.totalItems = result.count;
            
        }, 1000);
    
  }

  private applyFilterToRoute() : void {
      this.router.navigate([{ filter: this.form.filter }],
          {
                queryParams: { page: this.config.currentPage },
                relativeTo: this.activatedRoute,
                replaceUrl: true
            }
        );
        document.title = `Search: ${this.form.filter}`;
  }

  isQrCodeable() {
    return (this.selected != null && this.selected.prove == 2);
  }
   
  async getQrCodeImage() {
    let blob = await this.service.getQrCode(this.selected.id);

    let file = new FileReader();
    file.addEventListener("load", () => {
      this.image = file.result;
    });

    file.readAsDataURL(blob);
  }

  onSuccess() {
    if (!this.isSelected())
      return;

    this.logger.debug("User wants to see success of Challenge");
    this.router.navigate(['/challengesuccess/', this.selected.id]);
  }

  getCategorgyIcon(obj: Challenge) {
    if (obj.category == 1)
      return faGraduationCap;

    if (obj.category == 2)
      return faPeopleArrows;

    return faTasks;
  }
}