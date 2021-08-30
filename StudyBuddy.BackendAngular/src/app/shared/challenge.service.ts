import { EventEmitter, Injectable, Output } from '@angular/core';
import { AngularFireDatabase } from '@angular/fire/database';
import { Challenge } from '../model/challenge';
import { AuthorizationService } from './authorization.service';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class ChallengeService {
  @Output() changed = new EventEmitter();
  
  constructor(
    private db: AngularFireDatabase,
    private logger: LoggingService,
    private auth: AuthorizationService) { }

  private fromAnyToObject(val):Challenge {
    let p = new Challenge();
    p.name = val.name;
    p.description = val.description;
    p.points = val.points;
    p.validity_start = val.validity_start;
    p.validity_end = val.validity_end;
    p.category = val.category;
    p.owner = val.owner;
    p.created = val.created;
    p.prove = val.prove;
    p.parent = val.parent;
    p.study_program = val.study_program;
    p.enrolled_since = val.enrolled_since;
    return p;
  }

  private fromSingleSnapshot(snapshot):Challenge {
    let val = snapshot.val();
    let id = Object.keys(val)[0];
    val = val[id];
    let p = this.fromAnyToObject(val);
    p.id = id;
    return p;
  }

  private fromSnapshot(snapshot):Challenge {
    let val = snapshot.val();
    let p = this.fromAnyToObject(val);
    p.id = snapshot.key;
    return p;
  }

  async getAll():Promise<Challenge[]> {
    let result:Challenge[] = [];
    let reference = null;
    let user = this.auth.getUser();

    if (user.isAdmin())
      reference = this.db.database.ref("challenges");
    else
      reference = this.db.database.ref("challenges").orderByChild("owner").equalTo(user.id);

    let snapshot = await reference.once("value");

    snapshot.forEach((snapshot) => {
      result.push(this.fromSnapshot(snapshot));
    });

    return result;
  }

  async getCount() {
    let reference = this.db.database.ref("challenges");
    let snapshot = await reference.get();
    return snapshot.numChildren();
  }

  async remove(id:string) {
    this.db.database.ref().child("challenges").child(id).remove();
    this.changed.emit();
  }

  async byId(id:string):Promise<Challenge> {
    let reference = this.db.database.ref().child("challenges").child(id);
    let snapshot = await reference.get();

    if (snapshot.exists())
        return this.fromSnapshot(snapshot);

    return null;
  }

  // Die maximal dumme Art das richtige Object zu suchen! Muss unbedingt verbessert werden!
  async byText(text:string):Promise<Challenge[]> {
    let result:Challenge[] = [];
    let objects = await this.getAll();

    for (let obj of objects)
      if (obj.name.includes(text) || obj.description.includes(text))
        result.push(obj);

    return result;
  }

  async save(obj:Challenge) {
    let tmp = {
      'name': obj.name, 
      'description': obj.description,
      'points': obj.points,
      'validity_start': obj.validity_start,
      'validity_end': obj.validity_end,
      'category': obj.category,
      'owner': obj.owner,
      'created': obj.created,
      'prove': obj.prove,
      'parent': obj.parent,
      'study_program': obj.study_program,
      'enrolled_since': obj.enrolled_since
    };

    if (obj.id == "") {
      let ref = this.db.database.ref("challenges").push();
			obj.id = ref.key;
			ref.set(tmp);
    } else {
      this.db.database.ref("challenges").child(obj.id).set(tmp);
    }

    this.changed.emit();
  }
}
