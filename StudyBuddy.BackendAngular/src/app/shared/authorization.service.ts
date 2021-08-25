import { Injectable, EventEmitter } from '@angular/core';
import { AngularFireAuth } from '@angular/fire/auth';
import { User } from '../model/user';
import { UserService } from './user.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {
  private user:User = null;
  changed:EventEmitter<boolean> = new EventEmitter<boolean>();

  constructor(
    private auth:AngularFireAuth,
    private service:UserService) { }

  async login(email:string, password:string):Promise<boolean> {
    try {
      email = email.toLowerCase();
      let firebase_user = await this.auth.signInWithEmailAndPassword(email, password);
      this.user = await this.service.byEmail(email);

      if (!this.user.isStudent()) {
        this.changed.emit(true);
        return true;
      } else {
        this.user = null;
        await this.auth.signOut();
        return false;
      }
    } catch {
      return false;
    }
  }

  getUser() {
    return this.user;
  }

  async logout() {
    let result = await this.auth.signOut();
    this.changed.emit(false);
  }
  
  isLoggedIn() {
    return this.user != null;
  }

  sendPassworResetMail(email:string) {
    email = email.toLowerCase();
    this.auth.sendPasswordResetEmail(email);
  }
}