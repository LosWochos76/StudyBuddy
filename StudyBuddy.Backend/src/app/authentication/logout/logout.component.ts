import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';
import { User } from '../../model/user';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {
    current_user: User = null;

  constructor(
    private auth:AuthorizationService,
    private router:Router) {
      this.auth.changed.subscribe((result) => {
          if (result != 0) {
              this.router.navigate(['/login']);
          }
          
          
      });
      
      this.auth.logout();
    }

  ngOnInit(): void {
  }

}
