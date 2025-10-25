import { Component, DestroyRef, inject, input, OnInit } from '@angular/core';
import {
    CreateUpdateTaskFormControls,
    TaskFormService,
} from '../../services/task-form.service';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatCard, MatCardTitle } from '@angular/material/card';
import { MatError, MatFormField, MatLabel } from '@angular/material/form-field';
import {
    MatDatepicker,
    MatDatepickerModule,
} from '@angular/material/datepicker';
import { MatTimepickerModule } from '@angular/material/timepicker';
import TagDTO from '../../dtos/tag.dto';
import { TagService } from '../../services/tag.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpErrorResponse } from '@angular/common/http';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatInputModule } from '@angular/material/input';
import { ActivatedRoute, Router } from '@angular/router';
import TaskCreateUpdateDTO from '../../dtos/task/task-create-update.dto';
import { TaskService } from '../../services/task.service';
import TaskReadDTO from '../../dtos/task/task-read.dto';
import { forkJoin } from 'rxjs';
import { ErrorStateMatcher } from '@angular/material/core';

@Component({
    selector: 'app-task-form',
    imports: [
        ReactiveFormsModule,
        MatCard,
        MatCardTitle,
        MatFormField,
        MatLabel,
        MatError,
        MatDatepicker,
        MatDatepickerModule,
        MatTimepickerModule,
        FormsModule,
        MatCheckboxModule,
        MatInputModule,
    ],
    providers: [TaskFormService],
    templateUrl: './task-form.html',
    styleUrl: './task-form.scss',
    standalone: true,
})
export class TaskForm implements OnInit {
    public readonly formType = input.required<'create' | 'update'>();

    protected readonly taskFormGroup: FormGroup<CreateUpdateTaskFormControls>;
    protected tags: TagDTO[] = [];

    private readonly _taskFormService = inject(TaskFormService);
    private readonly _tagService = inject(TagService);
    private readonly _taskService = inject(TaskService);
    private readonly _destroyRef = inject(DestroyRef);
    private readonly _activatedRoute = inject(ActivatedRoute);
    private readonly _router = inject(Router);
    private _taskId: string | null = null;

    public constructor() {
        this.taskFormGroup =
            this._taskFormService.createCreateOrUpdateTaskFormGroup();
    }

    public ngOnInit(): void {
        if (this.formType() == 'create') {
            this._loadTags();
        } else {
            const taskId =
                this._activatedRoute.snapshot.paramMap.get('task-id');

            if (!taskId) {
                console.error("Task id doesn't exist");
                this._router.navigate(['/view-tasks']);
                return;
            }

            this._taskId = taskId;
            this._loadTagsAndTaskForUpdate(taskId);
        }
    }

    protected onSubmit(): void {
        if (this.formType() === 'create') {
            this._handleCreateSubmit();
        } else {
            this._handleUpdateSubmit();
        }
    }

    protected getDeadlineErrorMatcher(): ErrorStateMatcher {
        return {
            isErrorState: () => {
                return this.taskFormGroup.errors?.['deadlineDateTimeInvalid'];
            },
        };
    }

    private _handleCreateSubmit(): void {
        const taskCreateDTO: TaskCreateUpdateDTO =
            this._taskFormService.createTaskCreateUpdateDTO(
                this.taskFormGroup.getRawValue(),
                this.tags,
            );

        this._taskService
            .createTask(taskCreateDTO)
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe({
                next: (_: TaskReadDTO) =>
                    this._router.navigate(['/view-tasks']),
                error: (error: HttpErrorResponse) =>
                    console.error(
                        `Error while creating task -> ${error.message}`,
                    ),
            });
    }

    private _handleUpdateSubmit(): void {
        if (this._taskId === null) {
            console.error("Task id doesn't exist");
            this._router.navigate(['/view-tasks']);
            return;
        }

        const taskUpdateDTO: TaskCreateUpdateDTO =
            this._taskFormService.createTaskCreateUpdateDTO(
                this.taskFormGroup.getRawValue(),
                this.tags,
            );

        this._taskService
            .updateTask(this._taskId, taskUpdateDTO)
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe({
                next: (_: unknown) => this._router.navigate(['/view-tasks']),
                error: (error: HttpErrorResponse) =>
                    console.error(`Error while updating task -> ${error}`),
            });
    }

    private _loadTags(): void {
        this._tagService
            .getTags()
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe({
                next: (tags: TagDTO[]) => {
                    this.tags = tags;
                    this._taskFormService.initializeTaskFormGroup(
                        this.taskFormGroup,
                        null,
                        tags,
                    );
                },
                error: (error: HttpErrorResponse) =>
                    console.error(
                        `Error while gettings tags -> ${error.message}`,
                    ),
            });
    }

    private _loadTagsAndTaskForUpdate(taskId: string): void {
        forkJoin({
            task: this._taskService.getTask(taskId),
            tags: this._tagService.getTags(),
        })
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe({
                next: ({
                    task,
                    tags,
                }: {
                    task: TaskReadDTO;
                    tags: TagDTO[];
                }) => {
                    this.tags = tags;
                    this._taskFormService.initializeTaskFormGroup(
                        this.taskFormGroup,
                        task,
                        tags,
                    );
                },
                error: (error: HttpErrorResponse) =>
                    console.error(
                        `Error while loading task and tags -> ${error.message}`,
                    ),
            });
    }
}
