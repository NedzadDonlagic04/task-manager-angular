import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const deadlineTimeAheadAnHourValidator = (
    hasDeadlineControlName: string,
): ValidatorFn => {
    return (control: AbstractControl): ValidationErrors | null => {
        if (!control.parent) {
            return null;
        }

        const hasDeadline = control.parent.get(hasDeadlineControlName);

        if (hasDeadline && !hasDeadline.value) {
            return null;
        }

        const deadline = new Date(control.value);
        const now = new Date();
        const HOUR_IN_MILISECONDS = 3_600_000;
        const oneHourFromNow = new Date(now.getTime() + HOUR_IN_MILISECONDS);

        if (deadline <= oneHourFromNow) {
            return { deadlineTimeAhead: true };
        }

        return null;
    };
};
