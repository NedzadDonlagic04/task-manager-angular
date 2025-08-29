import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateTaskPage } from './update-task-page';

describe('UpdateTaskPage', () => {
    let component: UpdateTaskPage;
    let fixture: ComponentFixture<UpdateTaskPage>;

    beforeEach(async () => {
        await TestBed.configureTestingModule({
            imports: [UpdateTaskPage],
        }).compileComponents();

        fixture = TestBed.createComponent(UpdateTaskPage);
        component = fixture.componentInstance;
        fixture.detectChanges();
    });

    it('should create', () => {
        expect(component).toBeTruthy();
    });
});
