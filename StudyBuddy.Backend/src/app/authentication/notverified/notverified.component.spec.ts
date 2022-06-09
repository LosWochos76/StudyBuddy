/// <reference path="../../../../node_modules/@types/jasmine/index.d.ts" />
import { TestBed, async, ComponentFixture, ComponentFixtureAutoDetect } from '@angular/core/testing';
import { BrowserModule, By } from "@angular/platform-browser";
import { NotverifiedComponent } from './notverified.component';

let component: NotverifiedComponent;
let fixture: ComponentFixture<NotverifiedComponent>;

describe('notverified component', () => {
    beforeEach(async(() => {
        TestBed.configureTestingModule({
            declarations: [ NotverifiedComponent ],
            imports: [ BrowserModule ],
            providers: [
                { provide: ComponentFixtureAutoDetect, useValue: true }
            ]
        });
        fixture = TestBed.createComponent(NotverifiedComponent);
        component = fixture.componentInstance;
    }));

    it('should do something', async(() => {
        expect(true).toEqual(true);
    }));
});