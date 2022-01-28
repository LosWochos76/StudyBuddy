import { HttpClient } from '@angular/common/http';
import { Injectable, EventEmitter } from '@angular/core';
import { environment } from 'src/environments/environment';
import { User } from '../model/user';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  private url = environment.api_url;
  private user: User = null;
  private token: string = null;
  changed: EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(
    private http: HttpClient) { }

  async login(email: string, password: string): Promise<boolean> {
    const response = await this.http.post(this.url + 'Login', {
      'email': email,
      'password': password
    }).toPromise();

    if (response == null)
      return false;

    let user = User.fromApi(response['user']);

    if (!user.isStudent()) {
      this.user = user;
      this.token = response['token'];
      this.changed.emit(true);
      return true;
    }

    return false;
  }

  getUser() {
    return this.user;
  }

  getToken() {
    return this.token;
  }

  async logout() {
    this.user = null;
    this.token = null;
    this.changed.emit(false);
  }

  isLoggedIn() {
    return this.user != null;
  }

  sendPassworResetMail(email: string) {
    // ToDo: Implement!
  }
}