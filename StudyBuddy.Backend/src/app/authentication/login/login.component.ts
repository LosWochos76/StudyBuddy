import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginStatus } from 'src/app/model/loginresult';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  form: FormGroup;
  login_error: number = 0;
  _email: string;

  constructor(
    private logger: LoggingService,
    private route: ActivatedRoute,
    private auth: AuthorizationService,
    private router: Router) {
    this.form = new FormGroup({
      email: new FormControl("", [Validators.required, Validators.email]),
      password: new FormControl("", [Validators.required])
    });

    this.auth.changed.subscribe((result) => {
      if (result == LoginStatus.Success) {
        this.router.navigate(['/challenge']);
      }

      if (result == LoginStatus.EmailNotVerified) {
        this.router.navigate(['/login/verificationrequired', {
          email: this.form.controls.email.value
        }])
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

    if (result.status != 0)
      this.login_error = result.status;
  }

  async onPasswordReset() {
    this.router.navigate(['/login/forgotpassword', { 
      email: this.form.controls.email.value 
    }]);
  }
}