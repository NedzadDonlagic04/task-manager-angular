import { Component } from '@angular/core';
import { TaskFormBase } from '../task-form-base/task-form-base';
import TaskCreateDTO from '../../dtos/task-create.dto';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { MatCard, MatCardTitle } from '@angular/material/card';
import { MatCheckboxModule } from '@angular/material/checkbox';
import {
    MatDatepicker,
    MatDatepickerModule,
} from '@angular/material/datepicker';
import { MatFormField, MatLabel } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatTimepickerModule } from '@angular/material/timepicker';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
    selector: 'app-create-task-form',
    imports: [
        ReactiveFormsModule,
        MatFormField,
        MatLabel,
        MatCard,
        MatCardTitle,
        MatInputModule,
        MatDatepicker,
        MatDatepickerModule,
        MatCheckboxModule,
        MatTimepickerModule,
        FormsModule,
    ],
    templateUrl: './create-task-form.html',
    styleUrl: './create-task-form.scss',
    standalone: true,
})
export class CreateTaskForm extends TaskFormBase {
    protected override onTagsLoaded(): void {}

    protected override onSubmit(): void {
        const formData = this.taskFormGroup.value;
        const tagIds = this.getSelectedTags();

        const taskData: TaskCreateDTO = {
            title: formData.title?.trim() ?? '',
            description: formData.description?.trim() ?? '',
            deadline: formData.hasDeadline
                ? (this.getDeadlineFromControls()?.toISOString() ?? '')
                : null,
            tagIds: tagIds,
        };

        this.taskService.createTask(taskData).subscribe({
            next: () => {
                this.matSnackBar.open('Task saved successfully!', 'Dismiss', {
                    duration: 3000,
                    panelClass: ['success-snackbar'],
                });
                this.taskFormGroup.reset(this.initialFormValues);
            },
            error: (error: HttpErrorResponse) => {
                this.matSnackBar.open('Failed to save task.', 'Dismiss', {
                    duration: 5000,
                    panelClass: ['error-snackbar'],
                });
                console.error(`Error while creating task -> ${error}`);
            },
        });
    }
}
