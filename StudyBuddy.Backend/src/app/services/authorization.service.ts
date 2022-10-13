import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, EventEmitter } from '@angular/core';
import { environment } from 'src/environments/environment';
import { LoginResult, LoginStatus } from '../model/loginresult';
import { ResetPasswordData } from '../model/resetpassworddata';
import { User } from '../model/user';
import { VerifyEmailData } from '../model/verifyemaildata';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  private url = environment.api_url;
  private user: User = null;
  private token: string = null;
  changed: EventEmitter<LoginStatus> = new EventEmitter<LoginStatus>();

  constructor(
    private http: HttpClient) { }

  async login(email: string, password: string): Promise<LoginResult> {
    const response = await this.http.post(this.url + 'Login', {
      'email': email,
      'password': password
    }).toPromise();

    if (response == null)
      return null;

    let result = LoginResult.fromApi(response);
    if (result.status == LoginStatus.Success) {
      if (result.user.isStudent())
      {
        result.status = LoginStatus.IsStudent;
        result.user = null;
        return result;
      }
      
      if (result.user.isAdmin() || result.user.isTeacher())
      {
        this.user = result.user;
        this.token = response['token'];
        this.changed.emit(0);
        return result;
      }
    }

    if (result.status == LoginStatus.EmailNotVerified) {
      this.changed.emit(LoginStatus.EmailNotVerified);
      return result;
    }
    else 
      return result;
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
    this.changed.emit(1);
  }

  isLoggedIn() {
    return this.user != null;
  }

  async sendPassworResetMail(email: string): Promise<boolean> {
    var result = await this.http.post(this.url + 'Login/SendPasswortResetMail', "\"" + email + "\"",
      {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      }).toPromise();

    return 'status' in result && result['status'] == 'ok';
  }

  async resetPassword(obj: ResetPasswordData) {
    var data = {
      "token": obj.token,
      "email": obj.email,
      "password": obj.password
    }
    var response = await this.http.post(this.url + 'Login/ResetPassword', data).toPromise();
    if (response == null)
      return null;
    let result = LoginResult.fromApi(response);
    if (result.status == 0) {
      return result;
    }
    else
      return null;

  }

  async sendVerificationMail(email: string): Promise<boolean> {
    var result = await this.http.post(this.url + 'Login/SendVerificationMail', "\"" + email + "\"",
      {
        headers: new HttpHeaders({ 'Content-Type': 'application/json' })
      }).toPromise();

    return 'status' in result && result['status'] == 'ok';
  }

  async verifyEmail(obj: VerifyEmailData) {
    var data = {
      "token": obj.token,
      "email": obj.email
    }

    return await this.http.post(this.url + 'Login/VerifyEmail', data).toPromise();
  }

  async enableAccount(email: string, password: string): Promise<LoginResult> {
    const response = await this.http.post(this.url + 'Login/EnableAccount', {
      'email': email,
      'password': password
    }).toPromise();

    if (response == null)
      return null;

    let result = LoginResult.fromApi(response);
    return result.status == 7 ? result : null;
  }
}