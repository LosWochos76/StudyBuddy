import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { LoggingService } from 'src/app/services/loging.service';
import { Notification } from 'src/app/model/notification';
import { NotificationService } from 'src/app/services/notification.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css']
})
export class NotificationListComponent implements OnInit {
  page = 1;
  total = 0;
  objects: Notification[] = [];
  selected: Notification = null;
  user_cache = new Map();

  constructor(
    private logger: LoggingService,
    private service: NotificationService,
    private router: Router,
    private user_service: UserService) { }

  async ngOnInit() {
    var fullList = await this.service.getAll(this.page);
    this.objects = fullList.objects;
    this.total = fullList.count;
    this.service.changed.subscribe(async () => {
      var result = await this.service.getAll(this.page)
      this.objects = result.objects;
      this.total = result.count;
    });

    this.loadUserCache();
  }

  async onTableDataChange(event) {
    this.page = event;
    var fullList = await this.service.getAll(event);
    this.objects = fullList.objects;
  }

  private loadUserCache() {
    for (let obj of this.objects) {
      this.addToUserCache(obj.owner_id);
    }
  }

  private async addToUserCache(id: number) {
    if (id != 0 && !this.user_cache.has(id)) {
      var obj = await this.user_service.byId(id);
      
      if (obj != null)
        this.user_cache.set(id, obj);
    }
  }

  getFullNamOfUser(id: number) {
    if (id != 0 && this.user_cache.has(id))
      return this.user_cache.get(id).fullName();

    return "";
  }

  onSelect(obj: Notification) {
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  onDelete() {
    if (!this.isSelected())
      return;

    this.logger.debug("User wants to delete a Notifcation");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onEdit() {
    console.log("Hello World", this.selected);
    
    if (!this.isSelected())
      return;

    this.logger.debug("User wants to edit a Challenge");
    this.router.navigate(['/notification/', this.selected.id]);
  }
}
