import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders} from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { AuthorizationService } from './authorization.service';
import { FcmToken } from '../model/fcmToken';

@Injectable({
  providedIn: 'root'
})
export class FcmTokenService {

  private url = environment.api_url;


  constructor(    private http:HttpClient,
                  private auth:AuthorizationService,
  ) { }

  async GetAll() {
    if (!this.auth.isLoggedIn())
      return null;

    let path = `${this.url}FcmToken`;
    let result = await this.http.get<FcmToken>(path, {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })

    }).toPromise();
    
    

    return result;

  }
}
