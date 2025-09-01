import { Component, EventEmitter, inject, OnInit, Output } from '@angular/core';
import {
    FormArray,
    FormControl,
    ReactiveFormsModule,
    UntypedFormGroup,
} from '@angular/forms';
import {
    MatDatepicker,
    MatDatepickerModule,
} from '@angular/material/datepicker';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import TagDTO from '../../dtos/tag.dto';
import { TagService } from '../../services/tag.service';
import { TaskStateService } from '../../services/task-state.service';
import TaskStateDTO from '../../dtos/task-state.dto';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { startDateBeforeEndDateValidator } from '../../validators/start-date-before-end-date.validator';

export interface FilterData {
    searchTerm: string;
    taskStateName: string;
    deadlineStart: string;
    deadlineEnd: string;
    createdAtStart: string;
    createdAtEnd: string;
    tagNames: string[];
}

@Component({
    selector: 'app-filter-tasks-form',
    imports: [
        ReactiveFormsModule,
        MatFormField,
        MatLabel,
        MatInputModule,
        MatDatepicker,
        MatDatepickerModule,
        MatOption,
        MatSelect,
        MatCheckboxModule,
    ],
    templateUrl: './filter-tasks-form.html',
    styleUrl: './filter-tasks-form.scss',
    standalone: true,
})
export class FilterTasksForm implements OnInit {
    protected readonly filterFormGroup = new UntypedFormGroup({
        searchTerm: new FormControl(''),
        taskState: new FormControl(''),
        deadlineStart: new FormControl('', {
            nonNullable: true,
            validators: [
                startDateBeforeEndDateValidator('deadlineStart', 'deadlineEnd'),
            ],
        }),
        deadlineEnd: new FormControl('', {
            nonNullable: true,
        }),
        createdAtStart: new FormControl('', {
            nonNullable: true,
            validators: [
                startDateBeforeEndDateValidator(
                    'createdAtStart',
                    'createdAtEnd',
                ),
            ],
        }),
        createdAtEnd: new FormControl('', {
            nonNullable: true,
        }),
    });

    protected tags: TagDTO[] = [];
    protected taskStates: TaskStateDTO[] = [];

    @Output() onValueChanged = new EventEmitter<string>();

    private tagService = inject(TagService);
    private taskStateService = inject(TaskStateService);

    public ngOnInit(): void {
        this.taskStateService.getTaskStates().subscribe({
            next: (taskStates) => {
                this.taskStates = taskStates;
            },
            error: (error) =>
                console.error(`Error while loading task states -> ${error}`),
        });

        this.tagService.getTags().subscribe({
            next: (tags) => {
                this.tags = tags;

                const tagControls = tags.map(
                    () => new FormControl(false, { nonNullable: true }),
                );

                this.filterFormGroup.addControl(
                    'tags',
                    new FormArray(tagControls),
                );
            },
            error: (error) =>
                console.error(`Error while loading tags -> ${error}`),
        });

        this.filterFormGroup.valueChanges.subscribe({
            next: (filterObj) => {
                const tagNames: string[] = [];

                for (let i = 0; i < filterObj.tags.length; ++i) {
                    if (filterObj.tags[i]) {
                        tagNames.push(this.tags[i].name);
                    }
                }

                const filterData: FilterData = {
                    ...filterObj,
                    taskStateName: filterObj.taskState,
                    deadlineStart: filterObj.deadlineStart ?? '',
                    deadlineEnd: filterObj.deadlineEnd ?? '',
                    createdAtStart: filterObj.createdAtStart ?? '',
                    createdAtEnd: filterObj.createdAtEnd ?? '',
                    tagNames: tagNames,
                };

                this.onValueChanged.emit(JSON.stringify(filterData));
            },
            error: (error: any) =>
                console.error(`Filter form value changed error -> ${error}`),
        });

        this.filterFormGroup.get('deadlineEnd')?.valueChanges.subscribe(() => {
            this.filterFormGroup.get('deadlineStart')?.updateValueAndValidity();
        });

        this.filterFormGroup.get('createdAtEnd')?.valueChanges.subscribe(() => {
            this.filterFormGroup
                .get('createdAtStart')
                ?.updateValueAndValidity();
        });
    }

    public get deadlineStartControl() {
        return this.filterFormGroup.get('deadlineStart');
    }

    public get createdAtStartControl() {
        return this.filterFormGroup.get('createdAtStart');
    }
}
