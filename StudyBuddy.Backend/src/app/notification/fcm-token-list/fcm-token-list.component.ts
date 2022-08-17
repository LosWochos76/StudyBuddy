import { Component, OnInit } from '@angular/core';
import { FcmTokenList } from 'src/app/model/fcmtokenlist';
import { FcmTokenService } from 'src/app/services/fcm-token.service';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-fcm-token-list',
  templateUrl: './fcm-token-list.component.html',
  styleUrls: ['./fcm-token-list.component.css']
})
export class FcmTokenListComponent implements OnInit {
  page = 1;
  tokens:FcmTokenList = null;
  user_cache = new Map();

  constructor(
    private fcm_token_service: FcmTokenService,
    private user_service: UserService
  ) { }

  async ngOnInit() {
    this.loadData();
  }

  async loadData() {
    this.tokens = await this.fcm_token_service.getAll();
    for (let obj of this.tokens.objects) {
      this.user_cache.set(obj.userID, await this.user_service.byId(obj.userID));
    }
  }

  getFullNamOfUser(id:number) {
    if (this.user_cache.has(id))
      return this.user_cache.get(id).fullName();
    else
      return "";
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