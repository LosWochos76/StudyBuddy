/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { VerifyemailComponent } from './verifyemail.component';

let component: VerifyemailComponent;
let fixture: ComponentFixture<VerifyemailComponent>;

describe('verifyemail component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ VerifyemailComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(VerifyemailComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});