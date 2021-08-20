import { EventEmitter, Injectable, Output } from '@angular/core';
import { AngularFireDatabase } from '@angular/fire/database';
import { Term } from '../model/term';
import { LoggingService } from './loging.service';

@Injectable({
  providedIn: 'root'
})
export class TermService {
  @Output() changed = new EventEmitter();

  constructor(
    private db: AngularFireDatabase,
    private logger: LoggingService) { }

  async getAll():Promise<Term[]> {
    let result:Term[] = [];
    let reference = this.db.database.ref("terms").orderByChild("acronym");
    let snapshot = await reference.once("value");

    snapshot.forEach((snapshot) => {
      let val = snapshot.val();
      let p = new Term(snapshot.key, val.shortname, val.name, val.start, val.end);
      result.push(p);
    });

    return result;
  }

  async remove(id:string) {
    this.db.database.ref().child("terms").child(id).remove();
    this.changed.emit();
  }

  async byId(id:string):Promise<Term> {
    let reference = this.db.database.ref().child("terms").child(id);
    let snapshot = await reference.get();

    if (snapshot.exists()) {
        let val = snapshot.val();
        return new Term(snapshot.key, val.shortname, val.name, val.start, val.end);
    }

    return null;
  }

  async save(obj:Term) {
    let tmp = { 
      'shortname': obj.shortname, 
      'name': obj.name,
      'start': obj.start,
      'end': obj.end
    };

    if (obj.id == "") {
      let ref = this.db.database.ref("terms").push();
			obj.id = ref.key;
			ref.set(tmp);
    } else {
      this.db.database.ref("terms").child(obj.id).set(tmp);
    }

    this.changed.emit();
  }
}
