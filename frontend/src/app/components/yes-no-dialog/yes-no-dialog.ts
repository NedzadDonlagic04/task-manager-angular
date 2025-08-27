import { ChangeDetectionStrategy, Component, Inject } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { ConfirmDialog } from '../../interfaces/confirm-dialog.interface';

@Component({
  selector: 'app-yes-no-dialog',
  imports: [],
  templateUrl: './yes-no-dialog.html',
  styleUrl: './yes-no-dialog.css',
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class YesNoDialog {
  constructor(
    public dialogRef: MatDialogRef<YesNoDialog>,
    @Inject(MAT_DIALOG_DATA) public data: ConfirmDialog
  ) {}
}
