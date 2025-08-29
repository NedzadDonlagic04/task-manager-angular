import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ConfirmDialog } from '../../interfaces/confirm-dialog.interface';

@Component({
    selector: 'app-yes-no-dialog',
    imports: [],
    templateUrl: './yes-no-dialog.html',
    styleUrl: './yes-no-dialog.css',
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class YesNoDialog {
    public constructor(
        protected dialogRef: MatDialogRef<YesNoDialog>,
        @Inject(MAT_DIALOG_DATA) protected data: ConfirmDialog,
    ) {}
}
