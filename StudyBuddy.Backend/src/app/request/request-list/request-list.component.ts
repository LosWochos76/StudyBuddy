import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Request } from 'src/app/model/request';
import { User } from 'src/app/model/user';
import { LoggingService } from 'src/app/services/loging.service';
import { RequestService } from 'src/app/services/request.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-request-list',
  templateUrl: './request-list.component.html',
  styleUrls: ['./request-list.component.css']
})
export class RequestListComponent implements OnInit {
  objects:Request[] = [];
  selected:Request = null;
  user_cache = new Map();

  constructor(
    private logger:LoggingService, 
    private service:RequestService,
    private router:Router,
    private user_service:UserService) { }

  async ngOnInit() {
    this.objects = await this.service.getAll();
    this.service.changed.subscribe(async () => {
      this.objects = await this.service.getAll();
      this.loadUserCache();
    });

    this.loadUserCache();
  }

  private loadUserCache() {
    for (let obj of this.objects) {
      this.addToUserCache(obj.sender);
      this.addToUserCache(obj.recipient);
    }
  }

  private async addToUserCache(id:number) {
    if (!this.user_cache.has(id))
      this.user_cache.set(id, await this.user_service.byId(id));
  }

  getFullNamOfUser(id:number) {
    if (this.user_cache.has(id))
      return this.user_cache.get(id).fullName();
    
    return "";
  }
  
  onSelect(obj:Request) {
    this.logger.debug("User selected a Request");
    this.selected = obj;
  }

  isSelected() {
    return this.selected != null;
  }

  onDelete() {
    if (!this.isSelected())
      return;
    
    this.logger.debug("User wants to delete a Request");
    this.service.remove(this.selected.id);
    this.selected = null;
  }

  onAdd() {
    this.logger.debug("User wants to add a Request");
    this.router.navigate(['/request/0']);
  }

  onAccept() {
    this.logger.debug("User wants to accept a Request");
    this.service.accept(this.selected.id);
  }
}
