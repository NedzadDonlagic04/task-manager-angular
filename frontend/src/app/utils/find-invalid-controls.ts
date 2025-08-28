import { UntypedFormGroup } from '@angular/forms';

// Useful fucker for debugging
export const findInvalidControls = (formGroup: UntypedFormGroup) => {
    const invalidControls = [];
    const controls = formGroup.controls;

    for (const name in controls) {
        if (controls[name].invalid) {
            invalidControls.push(name);
        }
    }

    return invalidControls;
};
