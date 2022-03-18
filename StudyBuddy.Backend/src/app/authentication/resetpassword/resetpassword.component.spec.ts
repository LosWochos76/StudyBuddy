/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { ResetpasswordComponent } from './resetpassword.component';

let component: ResetpasswordComponent;
let fixture: ComponentFixture<ResetpasswordComponent>;

describe('resetpassword component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ ResetpasswordComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(ResetpasswordComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});