import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewTasksTable } from './view-tasks-table';

describe('ViewTasksTable', () => {
  let component: ViewTasksTable;
  let fixture: ComponentFixture<ViewTasksTable>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ViewTasksTable]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ViewTasksTable);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
