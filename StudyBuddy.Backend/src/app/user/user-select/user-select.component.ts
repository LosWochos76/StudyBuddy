import { Component, forwardRef, Input, OnInit } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { faArrowLeft, faArrowRight } from '@fortawesome/free-solid-svg-icons';
import { User } from 'src/app/model/user';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-user-select',
  templateUrl: './user-select.component.html',
  styleUrls: ['./user-select.component.css'],
  providers: [
    {
       provide: NG_VALUE_ACCESSOR,
       useExisting: forwardRef(() => UserSelectComponent),
       multi: true
    }
 ]
})
export class UserSelectComponent implements OnInit, ControlValueAccessor {
  @Input() selected:number[] = [];
  all:User[] = [];
  selected_objects:User[] = [];
  onChanged: any = () => { };
  onTouched: any = () => { };
  disabled = false;
  all_selected = null;
  selected_selected = null;

  constructor(
    private service:UserService,
    private library:FaIconLibrary) { 
      library.addIcons(faArrowLeft, faArrowRight)
    }

  writeValue(ids:number[]) {
    for (let id of ids)
      this.selected.push(id);
      
    this.reload();
  }

  registerOnChange(fn: any): void {
    this.onChanged = fn;
  }

  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
  }

  reload() {
    this.selected_objects = [];
    for (let id of this.selected)
      this.add(id, false);
  }

  async ngOnInit() {
    this.all = await this.service.getAll();
    this.reload();
  }

  onAllSelected(id:string) {
    this.all_selected = id;
  }

  onSelectedSelected(id:string) {
    this.selected_selected = id;
  }

  add(id:number, add_to_selected = true) {
    if (id == 0)
      return;
    
    if (add_to_selected)
      this.selected.push(id);

    let index = this.all.findIndex(x => x.id == id);
    if (index != -1) {
      this.selected_objects.push(this.all[index]);
      this.all.splice(index, 1);
    }

    this.all_selected = null;
    this.selected_selected = null;
    this.onChanged(this.selected);
  }

  remove(id:number) {
    if (id == 0)
      return;

    let index = this.selected.findIndex(x => x == id);
    this.all.push(this.selected_objects[index]);
    this.selected.splice(index, 1);
    this.selected_objects.splice(index, 1);
    this.all_selected = null;
    this.selected_selected = null;

    this.onChanged(this.selected);
  }
}