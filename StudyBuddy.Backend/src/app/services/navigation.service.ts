import { Injectable } from '@angular/core'
import { Location } from '@angular/common'
import { Router, NavigationEnd } from '@angular/router'

@Injectable()
export class NavigationService {
  private history: string[] = []

  constructor(private router: Router, private location: Location) { }

  public startSaveHistory():void{
    this.router.events.subscribe((event) => {
        if (event instanceof NavigationEnd) {
          this.history.push(event.urlAfterRedirects)
        }
      })
  }

  public getHistory(): string[] {
    return this.history;
  }

  public goBack(default_url:string = "/"): void {
    this.history.pop();
 
    if (this.history.length > 0) {
      this.location.back()
    } else {
      this.router.navigateByUrl(default_url)
    }
  }

  public getPreviousUrl(): string {
    if (this.history.length > 0) {
        return this.history[this.history.length - 2];
    }

    return '';
  }
}