import { Component, ComponentRef, OnInit, ViewContainerRef } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form:FormGroup;
    login_error: boolean = false;
    login_error_2: boolean = false;
    _email: string;

  constructor(
    private logger:LoggingService,
    private route:ActivatedRoute,
    private auth:AuthorizationService,
    private router:Router) { 
      this.form = new FormGroup({
        email: new FormControl("", [Validators.required, Validators.email]),
        password: new FormControl("", [Validators.required])
      });

      this.auth.changed.subscribe((result) => {
          if (result == 0)
          {
              this.router.navigate(['/challenge']);
          }

              
          if (result == 1)
          {
              this.router.navigate(['/login/verificationrequired', { email: this.form.controls.email.value }])
          }
              
      });
    }

    ngOnInit(): void {
        this._email = this.route.snapshot.paramMap.get('email');
  }

  async onSubmit() {
    let email = this.form.controls.email.value;
    let password = this.form.controls.password.value;
    let result = await this.auth.login(email, password);
      if (result.status == 3) {
          this.logger.debug("No User found.");
          this.login_error = true;
      }
      if (result.status == 2) {
          this.logger.debug("Wrong credentials!")
          this.login_error_2 = true;
      }
  }

    async onPasswordReset() {
        this.router.navigate(['/login/forgotpassword', { email: this.form.controls.email.value }]);
  }
}