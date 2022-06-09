import { Component, ComponentRef, Input, OnInit, ViewContainerRef } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { VerifyEmailData } from '../../model/verifyemaildata';
import { AuthorizationService } from '../../services/authorization.service';
import { LoggingService } from 'src/app/services/loging.service';

@Component({
    selector: 'app-verifyemail',
    templateUrl: './verifyemail.component.html',
    styleUrls: ['./verifyemail.component.css']
})
export class VerifyemailComponent implements OnInit {

    showSuccess: boolean = false;
    showError: boolean = false;
    private _token: string;
    private _email: string;

    constructor(
        private router: Router,
        private logger: LoggingService,
        private route: ActivatedRoute,
        private service: AuthorizationService) {

    }

    ngOnInit(): void {
        this._token = this.route.snapshot.queryParams['token'];
        this._email = this.route.snapshot.queryParams['email'];
        const verifyEmailData: VerifyEmailData = {
            token: this._token,
            email: this._email
        }
        let result = this.service.verifyEmail(verifyEmailData);
        if ('status' in result && result['status'] == 'ok')
            this.showSuccess = true; //logik in service verlegen
        else {
            this.logger.debug(`Verification for ${this._email} failed.`)
            console.log(result);
            this.showError = true;
        }
            
    }

    async toLogin() {
        this.router.navigate(['login']);
    }
}