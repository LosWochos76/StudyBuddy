import { DatePipe } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class LoggingService {
  private is_debug_mode:boolean = true;
  private datepipe:DatePipe = new DatePipe('en-US')
  
  constructor() { }

  private currDate() {
    return this.datepipe.transform(new Date(), 'yyyy-MM-dd HH:mm:ss');
  }

  debug(msg:any) {
    if (!this.is_debug_mode)
      return;

    console.log(this.currDate() + " DEBUG: " + JSON.stringify(msg));
  }

  error(msg:any) {
    console.log(this.currDate() + " ERROR: " + JSON.stringify(msg));
  }
}
