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
    private logger:LoggingService,
    private http:HttpClient,
    private auth:AuthorizationService) { }

  async getCount():Promise<number> {
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

  async getAll():Promise<User[]> {
    if (!this.auth.isLoggedIn())
      return null;

    let objects:User[] = [];
    this.logger.debug("Getting all Users");
    let result = await this.http.get(this.url + "User", 
    {
      headers: new HttpHeaders({ Authorization: this.auth.getToken() })
    }).toPromise();

    for (let obj in result)
      objects.push(User.fromApi(result[obj]));

    return objects;
  }

  async remove(id:number) {
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

  async byId(id:number):Promise<User> {
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

  async save(obj:User) {
    let data = obj.toApi();
    let result = null;

    if (obj.id == 0) {
      this.logger.debug("Saving new User");
      result = await this.http.post(this.url + "User", data).toPromise();
    } else {
      this.logger.debug("Saving existing User");
      result = await this.http.put(this.url + "User/" + obj.id, data, 
      {
        headers: new HttpHeaders({ Authorization: this.auth.getToken() })
      }).toPromise();
    }
    
    if ('status' in result)
      this.changed.emit();
  }

  async userIdByNickname(nickname:string):Promise<number> {
    let path = this.url + "User/UserIdByNickname/" + nickname;
    this.logger.debug("Searching for User by nickname");
    let result = await this.http.get(path).toPromise();
    
    if ('error' in result) {
      this.logger.debug("Object not found!");
      return 0;
    }
    else
      return result['id'];
  }

  async userIdByEmail(email:string):Promise<number> {
    let path = this.url + "User/UserIdByEmail/" + email;
    this.logger.debug("Searching for User by email");
    let result = await this.http.get(path).toPromise();
    
    if ('error' in result) {
      this.logger.debug("Object not found!");
      return 0;
    }
    else
      return result['id'];
  }
}