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
  login_error:boolean = false;

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
        if (result)
          this.router.navigate(['/challenge']);
      });
    }

  ngOnInit(): void {
  }

  async onSubmit() {
    let email = this.form.controls.email.value;
    let password = this.form.controls.password.value;
    let result = await this.auth.login(email, password);

    if (!result) {
      this.logger.debug("Wrong credentials!");
      this.login_error = true;
    }
  }

    async onPasswordReset() {
        this.router.navigate(['login/forgotpassword']);
  }
}