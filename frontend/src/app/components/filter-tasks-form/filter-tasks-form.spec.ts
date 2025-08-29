import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FilterTasksForm } from './filter-tasks-form';

describe('FilterTasksForm', () => {
    let component: FilterTasksForm;
    let fixture: ComponentFixture<FilterTasksForm>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [FilterTasksForm],
        }).compileComponents();

        fixture = TestBed.createComponent(FilterTasksForm);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
