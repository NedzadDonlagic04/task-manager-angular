import { FormArray, FormControl, FormGroup } from '@angular/forms';

export type UnwrapFormControl<T> = T extends FormControl<infer U> ? U : never;

export type UnwrapFormArray<T> =
    T extends FormArray<FormControl<infer U>> ? U[] : never;

export type FormControlsToRawValue<T> = {
    [K in keyof T]: T[K] extends FormControl<any>
        ? UnwrapFormControl<T[K]>
        : T[K] extends FormArray<FormControl<any>>
          ? UnwrapFormArray<T[K]>
          : T[K] extends FormGroup<infer U>
            ? FormControlsToRawValue<U>
            : never;
};
