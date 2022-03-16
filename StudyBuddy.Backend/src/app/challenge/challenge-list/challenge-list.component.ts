import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
  page = 1;
  total = 0;
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
    private auth: AuthorizationService) {
    this.navigation.startSaveHistory();
    this.user = this.auth.getUser();
  }

  async ngOnInit() {
    var result = await this.service.getAll(this.page);
    this.objects = result.objects;
    this.total = result.count;

    this.service.changed.subscribe(async () => {
      var result = await this.service.getAll(this.page);
      this.objects = result.objects;
      this.total = result.count;
    });
  }

  async onTableDataChange(event) {
    this.page = event;
    var fullList = await this.service.getAll(event);
    this.objects = fullList.objects;
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

  private async onSearch(text: string) {
    var result = await this.service.getAll(this.page, text);
    this.objects = result.objects;
    this.total = result.count;
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