import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TaskStatisticsPage } from './task-statistics-page';

describe('TaskStatisticsPage', () => {
    let component: TaskStatisticsPage;
    let fixture: ComponentFixture<TaskStatisticsPage>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [TaskStatisticsPage],
        }).compileComponents();

        fixture = TestBed.createComponent(TaskStatisticsPage);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
