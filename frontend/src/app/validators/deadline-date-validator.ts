import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

export const deadlineDateAheadValidator = (
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

        now.setHours(0, 0, 0, 0);

        if (isNaN(deadline.getTime()) || deadline < now) {
            return { deadlineDateAhead: true };
        }

        return null;
    };
};
