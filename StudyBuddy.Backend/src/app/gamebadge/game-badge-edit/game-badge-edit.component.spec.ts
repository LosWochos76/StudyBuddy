import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameBadgeEditComponent } from './game-badge-edit.component';

describe('GameBadgeEditComponent', () => {
  let component: GameBadgeEditComponent;
  let fixture: ComponentFixture<GameBadgeEditComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GameBadgeEditComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GameBadgeEditComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
