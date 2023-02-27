import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ItemCompletionStatisticsComponent } from './itemcompletionstatistics.component';

describe('LogoutComponent', () => {
    let component: ItemCompletionStatisticsComponent;
    let fixture: ComponentFixture<ItemCompletionStatisticsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
        declarations: [ItemCompletionStatisticsComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
      fixture = TestBed.createComponent(ItemCompletionStatisticsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});