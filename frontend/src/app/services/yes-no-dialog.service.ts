import { inject, Injectable } from '@angular/core';
import { MatDialog, MatDialogRef } from '@angular/material/dialog';
import { YesNoDialog } from '../components/yes-no-dialog/yes-no-dialog';

export interface YesNoDialogData {
    title: string;
    message: string;
    cancelText: string;
    confirmText: string;
}

@Injectable()
export class YesNoDialogService {
    private readonly _matDialog = inject(MatDialog);

    public open(
        yesNoDialogData?: Partial<YesNoDialogData>,
    ): MatDialogRef<YesNoDialog, any> {
        return this._matDialog.open(YesNoDialog, {
            width: '350px',
            data: {
                title: yesNoDialogData?.title ?? 'Title',
                message: yesNoDialogData?.message ?? 'Message',
                cancelText: yesNoDialogData?.cancelText ?? 'Cancel',
                confirmText: yesNoDialogData?.confirmText ?? 'Confirm',
            },
            disableClose: true,
        });
    }
}
