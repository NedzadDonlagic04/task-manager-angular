import {
    Component,
    DestroyRef,
    EventEmitter,
    inject,
    OnInit,
    Output,
} from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import {
    MatDatepicker,
    MatDatepickerModule,
} from '@angular/material/datepicker';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import TagDTO from '../../dtos/tag.dto';
import { TagService } from '../../services/tag.service';
import { MatOption } from '@angular/material/core';
import { MatSelect } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import {
    FilterTasksFormService,
    ProcessedFilterTaskFormValue,
} from '../../services/filter-task-form.service';
import { forkJoin } from 'rxjs';
import { TaskStateService } from '../../services/task-state.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import TaskStateDTO from '../../dtos/task-state.dto';

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
    providers: [FilterTasksFormService],
})
export class FilterTasksForm implements OnInit {
    @Output() protected filterChanged = new EventEmitter<string>();
    protected readonly filterTaskFormGroup;
    protected tags: TagDTO[] = [];
    protected taskStates: TaskStateDTO[] = [];

    private readonly _destroyRef = inject(DestroyRef);
    private readonly _tagService = inject(TagService);
    private readonly _taskStateService = inject(TaskStateService);
    private readonly _filterTaskFormService = inject(FilterTasksFormService);

    public constructor() {
        this.filterTaskFormGroup =
            this._filterTaskFormService.createFilterTaskFormGroup();
    }

    public ngOnInit(): void {
        this._loadTagsAndTaskStates();
    }

    private _loadTagsAndTaskStates(): void {
        forkJoin({
            tags: this._tagService.getTags(),
            taskStates: this._taskStateService.getTaskStates(),
        })
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe({
                next: ({
                    tags,
                    taskStates,
                }: {
                    tags: TagDTO[];
                    taskStates: TaskStateDTO[];
                }) => {
                    this.tags = tags;
                    this.taskStates = taskStates;

                    this._filterTaskFormService.setTagsArrayControl(
                        this.filterTaskFormGroup,
                        tags.length,
                    );

                    this._subscribeToFormGroupValueChanged();
                },
                error: (error: HttpErrorResponse) =>
                    console.error(
                        `Error while loading tags and task states -> ${error.message}`,
                    ),
            });
    }

    private _subscribeToFormGroupValueChanged(): void {
        this.filterTaskFormGroup.valueChanges
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe({
                next: () => {
                    const processedFilterTaskFormValue: ProcessedFilterTaskFormValue =
                        this._filterTaskFormService.createProcessedFilterTaskFormValue(
                            this.filterTaskFormGroup.getRawValue(),
                            this.tags,
                        );

                    this.filterChanged.emit(
                        JSON.stringify(processedFilterTaskFormValue),
                    );
                },
                error: (error: unknown) =>
                    console.error(
                        `Error occured while changing filter task form value -> ${error}`,
                    ),
            });
    }
}
