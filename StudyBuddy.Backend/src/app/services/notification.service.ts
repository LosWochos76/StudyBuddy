import { Injectable } from '@angular/core';
import {environment} from "../../environments/environment";
import {HttpClient, HttpHeaders} from "@angular/common/http";
import {AuthorizationService} from "./authorization.service";

@Injectable({
  providedIn: 'root'
})
export class NotificationService {

  private url = environment.api_url;


  constructor(    private http:HttpClient,
                  private auth:AuthorizationService,
  ) { }

  async broadcastNotification(title: string, body: string) {
    if (!this.auth.isLoggedIn())
      return null;

    let path = `${this.url}PushNotification`;
    let result = await this.http.post(path,{
      title,
      body,
    }, {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    return  result;

  }
}
