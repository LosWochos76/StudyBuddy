import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../model/user';
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

  async getCount(): Promise<number> {
    if (!this.auth.isLoggedIn())
      return null;

    let path = this.url + "User/Count";
    this.logger.debug("Getting count of Users");
    let result = await this.http.get(path,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    return result as number;
  }

  async getAll(): Promise<User[]> {
    if (!this.auth.isLoggedIn())
      return null;

    let objects: User[] = [];
    this.logger.debug("Getting all Users");
    let result = await this.http.get(this.url + "User",
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    for (let index in result)
      objects.push(User.fromApi(result[index]));

    return objects;
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

  async getFriends(id: number): Promise<User[]> {
    if (!this.auth.isLoggedIn())
      return null;

    let objects: User[] = [];
    this.logger.debug("Getting friends of " + id);
    let result = await this.http.get(this.url + "User/Friends/" + id,
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();

    for (let index in result)
      objects.push(result[index].id);

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
}