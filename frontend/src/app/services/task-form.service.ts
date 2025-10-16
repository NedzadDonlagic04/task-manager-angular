import { inject, Injectable } from '@angular/core';
import {
    FormArray,
    FormControl,
    FormGroup,
    NonNullableFormBuilder,
    Validators,
} from '@angular/forms';
import { FormControlsToRawValue } from '../utils/extract-raw-values-from-form.utils';
import { applyOffset } from '../utils/date-time-offset.utils';
import { deadlineDateTimeAheadValidator } from '../validators/deadline-date-time-validator';
import TagDTO from '../dtos/tag.dto';
import TaskCreateUpdateDTO from '../dtos/task/task-create-update.dto';
import TaskReadDTO from '../dtos/task/task-read.dto';

export interface CreateUpdateTaskFormControls {
    title: FormControl<string>;
    description: FormControl<string>;
    hasDeadline: FormControl<boolean>;
    deadlineDate: FormControl<Date>;
    deadlineTime: FormControl<Date>;
    selectedTags: FormArray<FormControl<boolean>>;
}

export type CreateUpateTaskFormValue =
    FormControlsToRawValue<CreateUpdateTaskFormControls>;

@Injectable()
export class TaskFormService {
    private readonly _nonNullableFormBuilder = inject(NonNullableFormBuilder);

    public createCreateOrUpdateTaskFormGroup(): FormGroup<CreateUpdateTaskFormControls> {
        const createUpdateTaskFormGroup =
            this._nonNullableFormBuilder.group<CreateUpdateTaskFormControls>(
                {
                    title: this._nonNullableFormBuilder.control('', {
                        validators: [
                            Validators.required,
                            Validators.minLength(3),
                            Validators.maxLength(50),
                        ],
                    }),
                    description: this._nonNullableFormBuilder.control('', {
                        validators: [Validators.maxLength(1_000)],
                    }),
                    hasDeadline: this._nonNullableFormBuilder.control(false),
                    deadlineDate: this._nonNullableFormBuilder.control(
                        new Date(),
                    ),
                    deadlineTime: this._nonNullableFormBuilder.control(
                        applyOffset(new Date(), { hours: 2 }),
                    ),
                    selectedTags: this._nonNullableFormBuilder.array<boolean>(
                        [],
                    ),
                },
                {
                    validators: [
                        deadlineDateTimeAheadValidator(
                            'hasDeadline',
                            'deadlineDate',
                            'deadlineTime',
                        ),
                    ],
                },
            );

        return createUpdateTaskFormGroup;
    }

    public initializeTaskFormGroup(
        createUpdateTaskFormGroup: FormGroup<CreateUpdateTaskFormControls>,
        task: TaskReadDTO | null,
        tags: TagDTO[],
    ): void {
        if (task) {
            this._patchTaskFormGroupWithTask(createUpdateTaskFormGroup, task);
        }

        this.setTagsArrayControlValues(
            createUpdateTaskFormGroup,
            tags,
            task === null ? [] : task.tagNames,
        );
    }

    public createTaskCreateUpdateDTO(
        taskFormValue: CreateUpateTaskFormValue,
        tags: TagDTO[],
    ): TaskCreateUpdateDTO {
        const tagIds: string[] = [];

        taskFormValue.selectedTags.forEach(
            (selectedTag: boolean, index: number) => {
                if (selectedTag) {
                    tagIds.push(tags[index].id);
                }
            },
        );

        const deadline = this._createDeadlineFromDateAndTime(taskFormValue);

        const taskCreateUpdateDTO: TaskCreateUpdateDTO =
            new TaskCreateUpdateDTO(
                taskFormValue.title.trim(),
                taskFormValue.description.trim(),
                deadline,
                tagIds,
            );

        return taskCreateUpdateDTO;
    }

    /**
     *   Calling this will clear the previous selected tags
     */
    public setTagsArrayControlValues(
        createUpdateTaskFormGroup: FormGroup<CreateUpdateTaskFormControls>,
        tags: TagDTO[],
        selectedTagNames: string[],
    ) {
        if (selectedTagNames.length > tags.length) {
            throw RangeError(
                `Task references ${selectedTagNames.length} tags, however only ${tags.length} were provided.`,
            );
        }

        const tagsArray = createUpdateTaskFormGroup.controls.selectedTags;
        tagsArray.clear();

        const selectedTagNamesSet = new Set(selectedTagNames);
        const selectedTags = tags.map((tag: TagDTO) =>
            selectedTagNamesSet.has(tag.name),
        );

        selectedTags.forEach((selectedTag: boolean) => {
            tagsArray.push(this._nonNullableFormBuilder.control(selectedTag));
        });
    }

    private _patchTaskFormGroupWithTask(
        createUpdateTaskFormGroup: FormGroup<CreateUpdateTaskFormControls>,
        task: TaskReadDTO,
    ): void {
        createUpdateTaskFormGroup.controls.title.setValue(task.title);
        createUpdateTaskFormGroup.controls.description.setValue(
            task.description,
        );
        createUpdateTaskFormGroup.controls.hasDeadline.setValue(false);

        if (task.deadline !== null) {
            createUpdateTaskFormGroup.controls.hasDeadline.setValue(true);

            const { deadlineDate, deadlineTime } = this._splitDeadline(
                task.deadline,
            );

            createUpdateTaskFormGroup.controls.deadlineDate.setValue(
                deadlineDate,
            );
            createUpdateTaskFormGroup.controls.deadlineTime.setValue(
                deadlineTime,
            );
        }
    }

    private _createDeadlineFromDateAndTime(
        createOrUpateTaskFormValue: CreateUpateTaskFormValue,
    ): Date | null {
        if (!createOrUpateTaskFormValue.hasDeadline) {
            return null;
        }

        const deadlineDate = createOrUpateTaskFormValue.deadlineDate;
        const deadlineTime = createOrUpateTaskFormValue.deadlineTime;

        const deadline = new Date(deadlineDate);

        deadline.setHours(
            deadlineTime.getHours(),
            deadlineTime.getMinutes(),
            deadlineTime.getSeconds(),
            deadlineTime.getMilliseconds(),
        );

        return deadline;
    }

    private _splitDeadline(deadline: Date): {
        deadlineDate: Date;
        deadlineTime: Date;
    } {
        const deadlineDate = new Date(deadline);
        const deadlineTime = new Date();

        deadlineTime.setHours(
            deadline.getHours(),
            deadline.getMinutes(),
            deadline.getSeconds(),
            deadline.getMilliseconds(),
        );

        return {
            deadlineDate,
            deadlineTime,
        };
    }
}
