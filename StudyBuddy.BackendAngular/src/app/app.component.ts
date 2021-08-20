import { Component, OnInit } from '@angular/core';
import { User } from './model/user';
import { AuthorizationService } from './shared/authorization.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  is_logged_in:boolean = false;
  current_user:User = null;

  constructor(private auth:AuthorizationService) { }

  ngOnInit(): void {
    this.is_logged_in = this.auth.isLoggedIn();
    this.auth.changed.subscribe(result => { 
      this.is_logged_in = result;
      this.current_user = this.auth.getUser();
      console.log(this.current_user);
    });
  }
}
