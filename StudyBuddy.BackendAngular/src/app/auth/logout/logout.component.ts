import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthorizationService } from 'src/app/shared/authorization.service';
import { LoggingService } from 'src/app/shared/loging.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(
    private logger:LoggingService,
    private auth:AuthorizationService,
    private router:Router) {
      this.auth.changed.subscribe((result) => {
        if (!result)
          this.router.navigate(['/login']);
      });
      
      this.auth.logout();
    }

  ngOnInit(): void {
  }

}
