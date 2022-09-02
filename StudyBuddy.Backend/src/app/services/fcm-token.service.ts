import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { environment } from 'src/environments/environment';
import { AuthorizationService } from './authorization.service';
import { FcmToken } from '../model/fcmToken';
import { FcmTokenList } from '../model/fcmtoken.list';

@Injectable({
  providedIn: 'root'
})
export class FcmTokenService {
  private url = environment.api_url;

  constructor(private http: HttpClient,
    private auth: AuthorizationService,
  ) { }

  async getAll(page:number = -1):Promise<FcmTokenList> {
    if (!this.auth.isLoggedIn())
      return null;

    var query = {};
    if (page != -1) {
      query['start'] = (page - 1) * 10;
      query['count'] = 10;
    }

    let path = `${this.url}FcmToken`;
    let result = await this.http.get<FcmToken>(path, {
      params: query,
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    return FcmTokenList.fromResult(result);
  }

  async deleteOld() {
    if (!this.auth.isLoggedIn())
      return null;

    let path = `${this.url}FcmToken`;
    let result = await this.http.delete<FcmToken>(path, {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    return result['status'] == 'ok';
  }
}