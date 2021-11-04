import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameBadgeSuccessComponent } from './game-badge-success.component';

describe('GameBadgeSuccessComponent', () => {
  let component: GameBadgeSuccessComponent;
  let fixture: ComponentFixture<GameBadgeSuccessComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GameBadgeSuccessComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GameBadgeSuccessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
