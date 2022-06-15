/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { RecoverComponent } from './recover.component';

let component: RecoverComponent;
let fixture: ComponentFixture<RecoverComponent>;

describe('recover component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ RecoverComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(RecoverComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});