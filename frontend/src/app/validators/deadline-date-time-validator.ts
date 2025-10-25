import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';
import { applyOffset } from '../utils/date-time-offset.utils';

export const deadlineDateTimeAheadValidator = (
    hasDeadlineControlName: string,
    deadlineDateControlName: string,
    deadlineTimeControlName: string,
): ValidatorFn => {
    return (formGroup: AbstractControl): ValidationErrors | null => {
        const hasDeadlineControl = formGroup.get(hasDeadlineControlName);
        const deadlineDateControl = formGroup.get(deadlineDateControlName);
        const deadlineTimeControl = formGroup.get(deadlineTimeControlName);

        if (
            !hasDeadlineControl ||
            !deadlineDateControl ||
            !deadlineTimeControl
        ) {
            throw new Error('Invalid control names passed to form validator');
        } else if (!hasDeadlineControl.value) {
            return null;
        }

        const deadlineDate = new Date(deadlineDateControl.value);
        deadlineDate.setHours(0);
        deadlineDate.setMinutes(0);
        deadlineDate.setSeconds(0);
        deadlineDate.setMilliseconds(0);

        const deadlineTime = new Date(deadlineTimeControl.value);

        const deadline = applyOffset(deadlineDate, {
            hours: deadlineTime.getHours(),
            minutes: deadlineTime.getMinutes(),
        });

        const oneHourFromNow = applyOffset(new Date(), {
            hours: 1,
            minutes: 2,
        });

        if (deadline <= oneHourFromNow) {
            return { deadlineDateTimeInvalid: true };
        }

        return null;
    };
};
