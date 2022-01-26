import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ChallengeService } from 'src/app/services/challenge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';
import { faGraduationCap, faPeopleArrows, faTasks } from '@fortawesome/free-solid-svg-icons';
import { ChallengeList } from '../../model/challengelist';

@Component({
  selector: 'app-challenge-list',
  templateUrl: './challenge-list.component.html',
  styleUrls: ['./challenge-list.component.css']
})
export class ChallengeListComponent implements OnInit {
    page = 1;
    total = 0;
    fullList:ChallengeList;
  objects: Challenge[] = [];
  selected: Challenge = null;
  timeout: any = null;
  user: User = null;
  owners_cache = new Map();
  image;

  constructor(
    private logger: LoggingService,
    private service: ChallengeService,
    private router: Router,
    private auth: AuthorizationService,
    private user_service: UserService) {
    this.user = this.auth.getUser();
  }

    async ngOnInit() {
        this.fullList = await this.service.getAll(this.page);
        this.objects = this.fullList.objects;
        this.total = this.fullList.count;
    this.service.changed.subscribe(async () => {
        this.objects = (await this.service.getAll(this.page)).objects;
    });

    if (this.user.isAdmin())
      this.loadOwners();
    }

    async onTableDataChange(event) {
        this.page = event;
        this.fullList = await this.service.getAll(event);
        this.objects = this.fullList.objects;
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

  onKeySearch(event: any) {
    clearTimeout(this.timeout);
    var $this = this;
    this.timeout = setTimeout(function () {
      if (event.keyCode != 13) {
        $this.onSearch(event.target.value);
      }
    }, 1000);
  }

  private async onSearch(value: string) {
      if (value == "")
          this.objects = (await this.service.getAll(this.page)).objects;
    else
      this.objects = (await this.service.byText(value)).objects;
  }

  private async loadOwners() {
    for (let obj of this.objects)
      this.owners_cache.set(obj.owner, await this.user_service.byId(obj.owner));
  }

  getOwnerName(id: number) {
    if (this.owners_cache.has(id)) {
      let owner = this.owners_cache.get(id);
      return owner.fullName();
    }

    return "";
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