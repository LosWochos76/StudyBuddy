import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GameObjectStatisticsComponent } from './gameobjectstatistics.component';

describe('LogoutComponent', () => {
    let component: GameObjectStatisticsComponent;
    let fixture: ComponentFixture<GameObjectStatisticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
        declarations: [GameObjectStatisticsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
      fixture = TestBed.createComponent(GameObjectStatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});