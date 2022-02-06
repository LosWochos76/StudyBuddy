/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ForgotpasswordComponent } from './forgotpassword.component';

let component: ForgotpasswordComponent;
let fixture: ComponentFixture<ForgotpasswordComponent>;

describe('forgotpassword component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ForgotpasswordComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ForgotpasswordComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});