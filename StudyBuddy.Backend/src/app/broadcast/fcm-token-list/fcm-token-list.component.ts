import { Component, OnInit } from '@angular/core';
import { FcmTokenList } from 'src/app/model/fcmtoken.list';
import { FcmTokenService } from 'src/app/services/fcm-token.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-fcm-token-list',
  templateUrl: './fcm-token-list.component.html',
  styleUrls: ['./fcm-token-list.component.css']
})
export class FcmTokenListComponent implements OnInit {
  page = 1;
  tokens:FcmTokenList = new FcmTokenList();
  user_cache = new Map();

  constructor(
    private fcm_token_service: FcmTokenService,
    private user_service: UserService
  ) { }

  async ngOnInit() {
    await this.loadData();
  }

  async loadData() {
    this.tokens = await this.fcm_token_service.getAll();
    await this.loadUsers();
  }

  async loadUsers() {
    for (let obj of this.tokens.objects) {
      if (obj.userID != 0 && !this.user_cache.has(obj.userID)) {
        let user = await this.user_service.byId(obj.userID)
        this.user_cache.set(obj.userID, user);
      }
    }
  }

  getFullNamOfUser(id:number) {
    if (id != 0 && this.user_cache.has(id)) {
      let user = this.user_cache.get(id);
      return user != null ? user.fullName() : "";
    } else {
      return "";
    }
  }

  async onTableDataChange(event) {
    this.page = event;
    this.loadData();
  }

  async onDeleteOld() {
    await this.fcm_token_service.deleteOld();
    await this.loadData();
  }
}