import {
    ChangeDetectionStrategy,
    Component,
    inject,
    Inject,
} from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { YesNoDialogData } from '../../services/yes-no-dialog.service';

@Component({
    selector: 'app-yes-no-dialog',
    imports: [],
    templateUrl: './yes-no-dialog.html',
    styleUrl: './yes-no-dialog.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
})
export class YesNoDialog {
    protected readonly dialogRef: MatDialogRef<YesNoDialog> =
        inject(MatDialogRef);
    protected readonly yesNoDialogData =
        inject<YesNoDialogData>(MAT_DIALOG_DATA);
}
