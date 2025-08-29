import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const deadlineDateAheadValidator = (
    hasDeadlineControlName: string,
    deadlineTimeControlName: string,
): ValidatorFn => {
    return (control: AbstractControl): ValidationErrors | null => {
        if (!control.parent) {
            return null;
        }

        const hasDeadlineControl = control.parent.get(hasDeadlineControlName);
        const deadlineTimeControl = control.parent.get(deadlineTimeControlName);

        if (!hasDeadlineControl || !deadlineTimeControl) {
            throw new Error('Invalid control names passed to form validator');
        } else if (hasDeadlineControl && !hasDeadlineControl.value) {
            return null;
        }

        const deadline = new Date(control.value);
        const now = new Date();

        now.setHours(0, 0, 0, 0);

        if (isNaN(deadline.getTime()) || deadline < now) {
            return { deadlineDateAhead: true };
        } else if (deadlineTimeControl) {
            deadlineTimeControl.updateValueAndValidity();
        }

        return null;
    };
};
