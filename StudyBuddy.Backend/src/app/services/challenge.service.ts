import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Challenge } from '../model/challenge';
import { ChallengeList } from '../model/challenge.list';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class ChallengeService {
  @Output() changed = new EventEmitter();
  private url = environment.api_url;

  constructor(
    private logger: LoggingService,
    private auth: AuthorizationService,
    private http: HttpClient) { }

  async getAll(page: number = -1, text: string = ""): Promise<ChallengeList> {
    if (!this.auth.isLoggedIn())
      return null;

    this.logger.debug("Loading all challenges");

    var query = {};
    if (page != -1) {
      query['start'] = (page - 1) * 10;
      query['count'] = 10;
    }

    if (this.auth.getUser().isAdmin())
      query['withOwner'] = true;

    if (this.auth.getUser().isTeacher())
      query['OwnerId'] = this.auth.getUser().id;

    if (text != "")
      query['SearchText'] = text;

    let result = await this.http.get(this.url + "Challenge",
      {
        params: query,
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    return ChallengeList.fromResult(result);
  }

  async getAccepted(user_id: number, page: number = -1): Promise<ChallengeList> {
    if (!this.auth.isLoggedIn())
      return null;

    this.logger.debug("Loading accepted challenges for user " + user_id);

    var query = {};
    if (page != -1) {
      query['start'] = (page - 1) * 10;
      query['count'] = 10;
    }

    query["onlyAccepted"] = true;
    query['currentUserId'] = user_id;

    let result = await this.http.get(this.url + "User/" + user_id + "/AcceptedChallenges/",
      {
        params: query,
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    return ChallengeList.fromResult(result);
  }

  async byId(id: number): Promise<Challenge> {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "Challenge/" + id;
    this.logger.debug("Loading Challenge from " + path);
    let result = await this.http.get(path,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    if (result == null) {
      this.logger.debug("Object not found!");
      return null;
    }
    else
      return Challenge.fromApi(result);
  }

  async remove(id: number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "Challenge/" + id;
    this.logger.debug("Removing Challenge from " + path);
    let result = await this.http.delete(path,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    this.changed.emit();
  }

  async save(obj: Challenge) {
    if (!this.auth.isLoggedIn())
      return null;

    let data = obj.toApi();
    let result = null;

    if (obj.id == 0) {
      this.logger.debug("Saving new Challenge");
      result = await this.http.post(this.url + "Challenge", data,
        {
          headers: new HttpHeaders({ Authorization: this.auth.getToken() })
        }).toPromise();

      obj.id = result["id"];
    } else {
      this.logger.debug("Saving existing Challenge");
      result = await this.http.put(this.url + "Challenge/" + obj.id, data,
        {
          headers: new HttpHeaders({ Authorization: this.auth.getToken() })
        }).toPromise();
    }

    this.changed.emit();
  }

  async createSeries(id: number, days_add: number, count: number) {
    if (!this.auth.isLoggedIn())
      return null;

    let data = { 'challengeId': id, 'daysAdd': days_add, 'count': count };
    let result = null;
    this.logger.debug("Creating series of Challenges");
    result = await this.http.post(this.url + "Challenge/CreateSeries/", data,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    this.changed.emit();
  }

  async getQrCode(id: number): Promise<Blob> {
    this.logger.debug("Loading QR-Code of Challenge " + id);

    let result = await this.http.get(this.url + "Challenge/" + id + "/QrCode/",
      {
        responseType: 'blob',
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    return result;
  }

  async removeAcceptance(challenge_id: number, user_id: number) {
    this.logger.debug("Removing challenge acceptance");

    let result = await this.http.delete(this.url + "Challenge/" + challenge_id + "/User/" + user_id,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
  }

  async addAcceptance(challenge_id: number, user_id: number) {
    if (!this.auth.isLoggedIn())
      return null;

    let result = await this.http.post(this.url + "Challenge/" + challenge_id + "/User/" + user_id, null,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
  }
}