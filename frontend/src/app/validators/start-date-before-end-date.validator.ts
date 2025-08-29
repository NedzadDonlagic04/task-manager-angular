import { AbstractControl, ValidationErrors, ValidatorFn } from '@angular/forms';

export const startDateBeforeEndDateValidator = (
    startDateControlName: string,
    endDateControlName: string,
): ValidatorFn => {
    return (control: AbstractControl): ValidationErrors | null => {
        if (!control.parent) {
            return null;
        }

        const startDateControl = control.parent.get(startDateControlName);
        const endDateControl = control.parent.get(endDateControlName);

        if (!startDateControl || !endDateControl) {
            throw new Error('Invalid control names passed to form validator');
        } else if (
            (startDateControl && !startDateControl.value) ||
            (endDateControl && !endDateControl.value)
        ) {
            return null;
        }

        const startDate: Date = startDateControl.value;
        const endDate: Date = endDateControl.value;

        if (startDate > endDate) {
            return { deadlineTimeAhead: true };
        }

        return null;
    };
};
