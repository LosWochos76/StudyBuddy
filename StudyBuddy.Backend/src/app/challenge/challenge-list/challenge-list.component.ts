import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Challenge } from 'src/app/model/challenge';
import { User } from 'src/app/model/user';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { ChallengeService } from 'src/app/services/challenge.service';
import { LoggingService } from 'src/app/services/loging.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-challenge-list',
  templateUrl: './challenge-list.component.html',
  styleUrls: ['./challenge-list.component.css']
})
export class ChallengeListComponent implements OnInit {
  objects:Challenge[] = [];
  selected:Challenge = null;
  timeout: any = null;
  user:User = null;
  owners_cache = new Map();
  image;

  constructor(
    private logger:LoggingService, 
    private service:ChallengeService,
    private router:Router,
    private auth:AuthorizationService,
    private user_service:UserService) { 
      this.user = this.auth.getUser();
    }

  async ngOnInit() {
    this.objects = await this.service.getAll();
    this.service.changed.subscribe(async () => {
      this.objects = await this.service.getAll();
    });

    if (this.user.isAdmin())
      this.loadOwners();
  }
  
  onSelect(obj:Challenge) {
    this.logger.debug("User selected a Challenge");
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
      this.objects = await this.service.getAll();
    else
      this.objects = await this.service.byText(value);
  }

  private async loadOwners() {
    for (let obj of this.objects)
      this.owners_cache.set(obj.owner,await this.user_service.byId(obj.owner));
  }

  getOwnerName(id:number) {
    if (this.owners_cache.has(id)) {
      let owner = this.owners_cache.get(id);
      return owner.fullName();
    }

    return "";
  }

  async getQrCodeImage() {
    let blob = await this.service.getQrCode(this.selected.id);

    let file = new FileReader();
    file.addEventListener("load", () => {
      this.image = file.result;
    });

    file.readAsDataURL(blob);
  }
}