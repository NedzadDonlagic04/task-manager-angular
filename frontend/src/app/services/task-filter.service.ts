import { Injectable } from '@angular/core';
import { TaskTableRow } from '../utils/task-table-row';
import { ProcessedFilterTaskFormValue } from './filter-task-form.service';

@Injectable({
    providedIn: 'root',
})
export class TaskFilterService {
    public doesRowMatchFilter(
        row: TaskTableRow,
        processedFilterFormValue: ProcessedFilterTaskFormValue,
    ): boolean {
        if (
            !this._doesRowMatchSearchTerm(
                row,
                processedFilterFormValue.searchTerm,
            )
        ) {
            return false;
        } else if (
            !this._doesRowMatchTaskStateName(
                row,
                processedFilterFormValue.taskStateName,
            )
        ) {
            return false;
        } else if (
            !this._doesRowMatchTagNames(row, processedFilterFormValue.tagNames)
        ) {
            return false;
        } else if (
            !this._doesRowMatchDeadlineRange(
                row,
                processedFilterFormValue.deadlineStart,
                processedFilterFormValue.deadlineEnd,
            )
        ) {
            return false;
        } else if (
            !this._doesRowMatchCreatedAtRange(
                row,
                processedFilterFormValue.createdAtStart,
                processedFilterFormValue.createdAtEnd,
            )
        ) {
            return false;
        }

        return true;
    }

    private _doesRowMatchSearchTerm(
        row: TaskTableRow,
        searchTerm: string,
    ): boolean {
        if (!searchTerm) {
            return true;
        }

        searchTerm = searchTerm.toLowerCase();

        return (
            row.title.toLowerCase().includes(searchTerm) ||
            row.description.toLowerCase().includes(searchTerm)
        );
    }

    private _doesRowMatchTaskStateName(
        row: TaskTableRow,
        taskStateName: string,
    ): boolean {
        return !taskStateName || row.taskStateName === taskStateName;
    }

    private _doesRowMatchTagNames(
        row: TaskTableRow,
        tagNames: string[],
    ): boolean {
        if (tagNames.length === 0) {
            return true;
        }

        return tagNames.every((tagName: string) =>
            row.tagNames.includes(tagName),
        );
    }

    private _doesRowMatchDeadlineRange(
        row: TaskTableRow,
        deadlineStart: Date | null,
        deadlineEnd: Date | null,
    ): boolean {
        if (!deadlineStart && !deadlineEnd) {
            return true;
        } else if (!row.deadline) {
            return false;
        }

        const deadline = this._stripTime(row.deadline);

        if (deadlineStart && deadline < this._stripTime(deadlineStart)) {
            return false;
        }
        if (deadlineEnd && deadline > this._stripTime(deadlineEnd)) {
            return false;
        }

        return true;
    }

    private _doesRowMatchCreatedAtRange(
        row: TaskTableRow,
        createdAtStart: Date | null,
        createdAtEnd: Date | null,
    ): boolean {
        if (!createdAtStart && !createdAtEnd) {
            return true;
        }

        const createdAt = this._stripTime(row.createdAt);

        if (createdAtStart && createdAt < this._stripTime(createdAtStart)) {
            return false;
        }
        if (createdAtEnd && createdAt > this._stripTime(createdAtEnd)) {
            return false;
        }

        return true;
    }

    private _stripTime(date: Date): Date {
        const zeroedOutTimeDate: Date = new Date(date);
        zeroedOutTimeDate.setHours(0, 0, 0, 0);
        return zeroedOutTimeDate;
    }
}
