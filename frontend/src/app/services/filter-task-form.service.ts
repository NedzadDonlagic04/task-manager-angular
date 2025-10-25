import { inject, Injectable } from '@angular/core';
import {
    FormArray,
    FormControl,
    FormGroup,
    NonNullableFormBuilder,
} from '@angular/forms';
import { FormControlsToRawValue } from '../utils/extract-raw-values-from-form.utils';
import TagDTO from '../dtos/tag.dto';

export interface DateRangeControls {
    start: FormControl<Date | null>;
    end: FormControl<Date | null>;
}

export interface FilterTaskFormControls {
    searchTerm: FormControl<string>;
    deadlineRange: FormGroup<DateRangeControls>;
    createdAtRange: FormGroup<DateRangeControls>;
    taskStateName: FormControl<string>;
    selectedTags: FormArray<FormControl<boolean>>;
}

export type FilterTaskFormValue =
    FormControlsToRawValue<FilterTaskFormControls>;

export interface ProcessedFilterTaskFormValue {
    searchTerm: string;
    deadline: {
        start: Date | null;
        end: Date | null;
    };
    createdAt: {
        start: Date | null;
        end: Date | null;
    };
    taskStateName: string;
    tagNames: string[];
}

@Injectable()
export class FilterTasksFormService {
    private readonly _nonNullableFormBuilder = inject(NonNullableFormBuilder);

    public createFilterTaskFormGroup(): FormGroup<FilterTaskFormControls> {
        const filterTaskFormGroup =
            this._nonNullableFormBuilder.group<FilterTaskFormControls>({
                searchTerm: this._nonNullableFormBuilder.control(''),
                deadlineRange: this._nonNullableFormBuilder.group({
                    start: this._nonNullableFormBuilder.control<Date | null>(
                        null,
                    ),
                    end: this._nonNullableFormBuilder.control<Date | null>(
                        null,
                    ),
                }),
                createdAtRange: this._nonNullableFormBuilder.group({
                    start: this._nonNullableFormBuilder.control<Date | null>(
                        null,
                    ),
                    end: this._nonNullableFormBuilder.control<Date | null>(
                        null,
                    ),
                }),
                taskStateName: this._nonNullableFormBuilder.control(''),
                selectedTags: this._nonNullableFormBuilder.array<boolean>([]),
            });

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
            deadline: {
                start: filterTaskFormValue.deadlineRange.start,
                end: filterTaskFormValue.deadlineRange.end,
            },
            createdAt: {
                start: filterTaskFormValue.createdAtRange.start,
                end: filterTaskFormValue.createdAtRange.end,
            },
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
