import { AbstractControl, ValidationErrors, ValidatorFn } from "@angular/forms";

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
        const MINUTES_IN_AN_HOUR = 60;
        const SECONDS_IN_A_MINUTE = 60;
        const MILLISECONDS_IN_A_SECOND = 1000;
        const oneHourFromNow = new Date(
            now.getTime() +
                MINUTES_IN_AN_HOUR *
                    SECONDS_IN_A_MINUTE *
                    MILLISECONDS_IN_A_SECOND,
        );

        if (oneHourFromNow <= deadline) {
            return { deadlineTimeAhead: true };
        }

        return null;
    };
};
