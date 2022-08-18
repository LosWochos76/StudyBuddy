import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../model/user';
import { UserList } from '../model/userlist';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  @Output() changed = new EventEmitter();
  private url = environment.api_url;

  constructor(
    private logger: LoggingService,
    private http: HttpClient,
    private auth: AuthorizationService) { }

  async getAll(page: number = -1): Promise<UserList> {
    if (!this.auth.isLoggedIn())
      return null;

    this.logger.debug("Getting all Users");
    var query = {};

    if (page != -1) {
      query['start'] = (page - 1) * 10;
      query['count'] = 10;
    }

    let result = await this.http.get(this.url + "User",
      {
        params: query,
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    return UserList.fromResult(result);
  }

  async remove(id: number) {
    if (!this.auth.isLoggedIn())
      return;

    let path = this.url + "User/" + id;
    this.logger.debug("Removing User from " + path);
    let result = await this.http.delete(path,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    if ('status' in result)
      this.changed.emit();
  }

  async byId(id: number): Promise<User> {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "User/" + id;
    this.logger.debug("Loading User from " + path);
    let result = await this.http.get(path,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    if (result == null) {
      this.logger.debug("Object not found!");
      return null;
    }
    else
      return User.fromApi(result);
  }

  async save(obj: User) {
    let data = obj.toApi();
    console.log(data);
    let result = null;
    this.logger.debug("Saving User");

    if (obj.id == 0) {
      result = await this.http.post(this.url + "User", data).toPromise();
      obj.id = result['id'];
    } else {
      result = await this.http.put(this.url + "User/" + obj.id, data,
        {
          headers: new HttpHeaders({ Authorization: this.auth.getToken() })
        }).toPromise();
    }

    this.changed.emit();
  }

  async userIdByNickname(nickname: string): Promise<number> {
    let path = this.url + "User/UserIdByNickname/" + nickname;
    this.logger.debug("Searching for User by nickname");
    let result = await this.http.get(path).toPromise();
    return result['id'];
  }

  async userIdByEmail(email: string): Promise<number> {
    let path = this.url + "User/UserIdByEmail/" + email;
    this.logger.debug("Searching for User by email");
    let result = await this.http.get(path).toPromise();
    return result['id'];
  }

  async getFriendIds(id: number): Promise<object> {
    if (!this.auth.isLoggedIn())
      return null;

    let objects: User[] = [];
    this.logger.debug("Getting friends of " + id);
    let result = await this.http.get(this.url + "User/" + id + "/Friends",
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    for (let index in result['objects']) {
      objects.push(result['objects'][index].id);
    }

    return objects;
  }

  async getFriends(user_id: number, page: number = -1): Promise<UserList> {
    if (!this.auth.isLoggedIn())
      return null;

    var query = {};
    if (page != -1) {
      query['start'] = (page - 1) * 10;
      query['count'] = 10;
    }

    let objects: User[] = [];
    this.logger.debug("Getting friends of " + user_id);
    let result = await this.http.get(this.url + "User/" + user_id + "/Friends",
      {
        params: query,
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    return UserList.fromResult(result);
  }

  async setFriends(id: number, friends: number[]) {
    if (!this.auth.isLoggedIn())
      return null;

    let data = {
      'userID': id,
      'friends': friends
    };

    let result = await this.http.post(this.url + "User/Friends/", data,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
  }

  async getUsersThatAcceptedChallenge(challenge_id: number): Promise<UserList> {
    if (!this.auth.isLoggedIn())
      return null;

    this.logger.debug("Getting all Users that accepted challenge " + challenge_id);
    let result = await this.http.get(this.url + "Challenge/" + challenge_id + "/User",
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    return UserList.fromResult(result);
  }

  async getUsersHavingBadge(badge_id: number): Promise<UserList> {
    if (!this.auth.isLoggedIn())
      return null;

    this.logger.debug("Getting all Users having gamebadge " + badge_id);
    let result = await this.http.get(this.url + "GameBadge/" + badge_id + "/User",
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    return UserList.fromResult(result);
  }

  async addBadgeToUser(user_id: number, badge_id: number) {
    if (!this.auth.isLoggedIn())
      return null;

    let result = await this.http.post(this.url + "User/" + user_id + "/GameBadge/" + badge_id, null,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
  }

  async sendMail(user_id: number, subject: string, message: string) {
    if (!this.auth.isLoggedIn() && !this.auth.getUser().isAdmin)
      return false;

    let data = {
      "recipientID": user_id,
      "subject": subject,
      "message": message
    };

    let result = await this.http.post(this.url + "User/SendMail/" + user_id, data,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise().then(res => {
        return res['status'] == 'ok';
      }).catch(res => {
        console.log(res);
        return false;
      });

    return result;
  }
}