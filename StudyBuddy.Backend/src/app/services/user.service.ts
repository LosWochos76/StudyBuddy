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
  users_cache: UserList = null;

  constructor(
    private logger: LoggingService,
    private http: HttpClient,
      private auth: AuthorizationService) { }

  async getAll(page:number): Promise<UserList> {
    if (!this.auth.isLoggedIn())
      return null;
    

      this.logger.debug("Getting all Users");
      var query = {};
      query['start'] = (page - 1) * 10;
      query['count'] = 10;
    let result = await this.http.get(this.url + "User",
        {
        params: query,
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    this.users_cache = new UserList(result);
      return new UserList(result);
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

    this.users_cache = null;
    if ('status' in result)
      this.changed.emit();
  }

  async byId(id: number): Promise<User> {
    if (!this.auth.isLoggedIn())
      return null;
    
    if (this.users_cache == null)
      await this.getAll(1);

    if (this.users_cache != null) {
      for (let i=0; i<this.users_cache.objects.length; i++)
        if (this.users_cache.objects[i].id == id)
          return this.users_cache.objects[i];
    }

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

    this.users_cache = null;
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

  async getFriends(id: number): Promise<object> {
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

    return new UserList(result);
  }

  async getUsersHavingBadge(badge_id: number): Promise<UserList> {
    if (!this.auth.isLoggedIn())
      return null;

    this.logger.debug("Getting all Users having gamebadge " + badge_id);
    let result = await this.http.get(this.url + "GameBadge/" + badge_id + "/User",
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    return new UserList(result);
  }

  async addBadgeToUser(user_id:number, badge_id:number) {
    if (!this.auth.isLoggedIn())
      return null;

    let result = await this.http.post(this.url + "User/" + user_id + "/GameBadge/" + badge_id, null,
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();
  }
}