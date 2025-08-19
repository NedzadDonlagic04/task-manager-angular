import { Component, OnInit } from "@angular/core";
import { TaskFormBase } from "../task-form-base/task-form-base";
import { TagService } from "../../../services/tag.service";
import { TaskService } from "../../../services/task.service";
import TaskCreateDTO from "../../../dtos/task-create.dto";
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { MatCard, MatCardTitle } from "@angular/material/card";
import { MatCheckboxModule } from "@angular/material/checkbox";
import {
    MatDatepicker,
    MatDatepickerModule,
} from "@angular/material/datepicker";
import { MatFormField, MatLabel } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
    selector: "create-task-form",
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
        FormsModule,
    ],
    templateUrl: "./create-task-form.html",
    styleUrl: "./create-task-form.css",
})
export class CreateTaskForm extends TaskFormBase {
    constructor(
        tagService: TagService,
        taskService: TaskService,
        matSnackBar: MatSnackBar,
    ) {
        super(tagService, taskService, matSnackBar);
    }

    override onTagsLoaded(): void {}

    override onSubmit(): void {
        const formData = this.taskFormGroup.value;
        const tagIds = this.getSelectedTags();

        const taskData: TaskCreateDTO = {
            title: formData.title ?? "",
            description: formData.description ?? "",
            deadline: this.hasDeadline ? (formData.deadline ?? "") : null,
            tagIds: tagIds,
        };

        this.taskService.createTask(taskData).subscribe({
            next: () => {
                this.matSnackBar.open("Task saved successfully!", "Dismiss", {
                    duration: 3000,
                    panelClass: ["success-snackbar"],
                });
                this.taskFormGroup.reset();
            },
            error: (error) =>
                this.matSnackBar.open("Failed to save task.", "Dismiss", {
                    duration: 5000,
                    panelClass: ["error-snackbar"],
                }),
        });
    }
}
