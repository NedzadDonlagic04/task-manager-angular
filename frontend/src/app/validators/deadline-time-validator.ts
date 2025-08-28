import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const deadlineTimeAheadAnHourValidator = (
    hasDeadlineControlName: string,
    deadlineDateControlName: string,
): ValidatorFn => {
    return (control: AbstractControl): ValidationErrors | null => {
        if (!control.parent) {
            return null;
        }

        const hasDeadlineControl = control.parent.get(hasDeadlineControlName);
        const deadlineDateControl = control.parent.get(deadlineDateControlName);

        if (hasDeadlineControl && !hasDeadlineControl.value) {
            return null;
        }

        const controlDate = new Date(control.value);

        const deadlineTime = new Date(deadlineDateControl?.value);
        deadlineTime.setHours(
            controlDate.getHours(),
            controlDate.getMinutes(),
            0,
            0,
        );

        const now = new Date();
        const HOUR_IN_MILISECONDS = 3_600_000;
        const oneHourFromNow = new Date(now.getTime() + HOUR_IN_MILISECONDS);

        if (deadlineTime <= oneHourFromNow) {
            return { deadlineTimeAhead: true };
        }

        return null;
    };
};
