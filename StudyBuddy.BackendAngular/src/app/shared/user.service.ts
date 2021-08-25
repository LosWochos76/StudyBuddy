import { EventEmitter, Injectable, Output } from '@angular/core';
import { AngularFireDatabase } from '@angular/fire/database';
import { User } from '../model/user';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  @Output() changed = new EventEmitter();
  
  constructor(
    private db: AngularFireDatabase,
    private logger: LoggingService) { }

  private fromSingleSnapshot(snapshot):User {
    let val = snapshot.val();
    let id = Object.keys(val)[0];
    val = val[id];

    let p = new User();
    p.id = id;
    p.firstname = val.firstname;
    p.lastname = val.lastname;
    p.nickname = val.nickname;
    p.email = val.email;
    p.role = val.role;
    p.study_program_id = val.study_program_id;
    p.enrolled_since_term_id = val.enrolled_since_term_id;
    p.firebase_user_id = val.firebase_user_id;
    return p;
  }

  private fromSnapshot(snapshot):User {
    let val = snapshot.val();
    let p = new User();
    p.id = snapshot.key;
    p.firstname = val.firstname;
    p.lastname = val.lastname;
    p.nickname = val.nickname;
    p.email = val.email;
    p.role = val.role;
    p.study_program_id = val.study_program_id;
    p.enrolled_since_term_id = val.enrolled_since_term_id;
    p.firebase_user_id = val.firebase_user_id;
    return p;
  }

  async getAll():Promise<User[]> {
    let result:User[] = [];
    let reference = this.db.database.ref("users").orderByChild("nickname");
    let snapshot = await reference.once("value");

    snapshot.forEach((snapshot) => {
      result.push(this.fromSnapshot(snapshot));
    });

    return result;
  }

  async getCount() {
    let reference = this.db.database.ref("users");
    let snapshot = await reference.get();
    return snapshot.numChildren();
  }

  async remove(id:string) {
    this.db.database.ref().child("users").child(id).remove();
    this.changed.emit();
  }

  async byId(id:string):Promise<User> {
    let reference = this.db.database.ref().child("users").child(id);
    let snapshot = await reference.get();

    if (snapshot.exists())
        return this.fromSnapshot(snapshot);

    return null;
  }

  async byNickname(nickname:string):Promise<User> {
    nickname = nickname.toLowerCase();
    let reference = this.db.database.ref("users").orderByChild("nickname").equalTo(nickname);
    let snapshot = await reference.get();

    if (snapshot.exists())
        return this.fromSingleSnapshot(snapshot);

    return null;
  }

  async byEmail(email:string):Promise<User> {
    email = email.toLowerCase();
    let reference = this.db.database.ref("users").orderByChild("email").equalTo(email);
    let snapshot = await reference.get();

    if (snapshot.exists())
        return this.fromSingleSnapshot(snapshot);

    return null;
  }

  async byFirebaseUserId(firebase_user_id:string):Promise<User> {
    let reference = this.db.database.ref("users").orderByChild("firebase_user_id").equalTo(firebase_user_id);
    let snapshot = await reference.get();

    if (snapshot.exists())
        return this.fromSingleSnapshot(snapshot);

    return null;
  }

  async save(obj:User) {
    let tmp = { 
      'firstname': obj.firstname, 
      'lastname': obj.lastname,
      'nickname': obj.nickname.toLowerCase(),
      'email': obj.email.toLowerCase(),
      'role': obj.role,
      'study_program_id': obj.study_program_id,
      'enrolled_since_term_id': obj.enrolled_since_term_id,
      'firebase_user_id': obj.firebase_user_id
    };

    if (obj.id == "") {
      let ref = this.db.database.ref("users").push();
			obj.id = ref.key;
			ref.set(tmp);
    } else {
      this.db.database.ref("users").child(obj.id).set(tmp);
    }

    this.changed.emit();
  }
}
