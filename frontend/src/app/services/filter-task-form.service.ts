import { inject, Injectable } from '@angular/core';
import {
    FormArray,
    FormControl,
    FormGroup,
    NonNullableFormBuilder,
} from '@angular/forms';
import { FormControlsToRawValue } from '../utils/extract-raw-values-from-form.utils';
import { startDateBeforeEndDateValidator } from '../validators/start-date-before-end-date.validator';
import TagDTO from '../dtos/tag.dto';

export interface FilterTaskFormControls {
    searchTerm: FormControl<string>;
    deadlineStart: FormControl<string>;
    deadlineEnd: FormControl<string>;
    createdAtStart: FormControl<string>;
    createdAtEnd: FormControl<string>;
    taskStateName: FormControl<string>;
    selectedTags: FormArray<FormControl<boolean>>;
}

export type FilterTaskFormValue =
    FormControlsToRawValue<FilterTaskFormControls>;

export interface ProcessedFilterTaskFormValue {
    searchTerm: string;
    deadlineStart: Date | null;
    deadlineEnd: Date | null;
    createdAtStart: Date | null;
    createdAtEnd: Date | null;
    taskStateName: string;
    tagNames: string[];
}

@Injectable()
export class FilterTasksFormService {
    private readonly _nonNullableFormBuilder = inject(NonNullableFormBuilder);

    public createFilterTaskFormGroup(): FormGroup<FilterTaskFormControls> {
        const filterTaskFormGroup =
            this._nonNullableFormBuilder.group<FilterTaskFormControls>(
                {
                    searchTerm: this._nonNullableFormBuilder.control(''),
                    deadlineStart: this._nonNullableFormBuilder.control(''),
                    deadlineEnd: this._nonNullableFormBuilder.control(''),
                    createdAtStart: this._nonNullableFormBuilder.control(''),
                    createdAtEnd: this._nonNullableFormBuilder.control(''),
                    taskStateName: this._nonNullableFormBuilder.control(''),
                    selectedTags: this._nonNullableFormBuilder.array<boolean>(
                        [],
                    ),
                },
                {
                    validators: [
                        startDateBeforeEndDateValidator(
                            'deadlineStart',
                            'deadlineEnd',
                        ),
                        startDateBeforeEndDateValidator(
                            'createdAtStart',
                            'createdAtEnd',
                        ),
                    ],
                },
            );

        return filterTaskFormGroup;
    }

    public setTagsArrayControl(
        filterTaskFormGroup: FormGroup<FilterTaskFormControls>,
        tagsCount: number,
    ): void {
        for (let i = 0; i < tagsCount; ++i) {
            filterTaskFormGroup.controls.selectedTags.push(
                this._nonNullableFormBuilder.control(false),
            );
        }
    }

    public createProcessedFilterTaskFormValue(
        filterTaskFormValue: FilterTaskFormValue,
        tags: TagDTO[],
    ): ProcessedFilterTaskFormValue {
        return {
            searchTerm: filterTaskFormValue.searchTerm.trim(),
            deadlineStart: new Date(filterTaskFormValue.deadlineStart),
            deadlineEnd: new Date(filterTaskFormValue.deadlineEnd),
            createdAtStart: new Date(filterTaskFormValue.createdAtStart),
            createdAtEnd: new Date(filterTaskFormValue.createdAtEnd),
            taskStateName: filterTaskFormValue.taskStateName,
            tagNames: this.getTagNamesFromTagsAndSelectedTags(
                tags,
                filterTaskFormValue.selectedTags,
            ),
        };
    }

    public getTagNamesFromTagsAndSelectedTags(
        tags: TagDTO[],
        selectedTags: boolean[],
    ): string[] {
        if (tags.length !== selectedTags.length) {
            throw RangeError(
                `Tags array length (${tags.length}) and selected tags array length (${selectedTags.length}) do not match`,
            );
        }

        return tags
            .filter((_: TagDTO, index: number) => selectedTags[index])
            .map((tag: TagDTO) => tag.name);
    }
}
