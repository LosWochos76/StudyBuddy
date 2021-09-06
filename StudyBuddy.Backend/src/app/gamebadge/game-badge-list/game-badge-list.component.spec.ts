import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameBadgeListComponent } from './game-badge-list.component';

describe('GameBadgeListComponent', () => {
  let component: GameBadgeListComponent;
  let fixture: ComponentFixture<GameBadgeListComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ GameBadgeListComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(GameBadgeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
