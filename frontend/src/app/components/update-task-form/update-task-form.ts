import { Component, OnInit } from "@angular/core";
import { TaskFormBase } from "../task-form-base/task-form-base";
import { TagService } from "../../services/tag.service";
import { TaskService } from "../../services/task.service";
import {
    ReactiveFormsModule,
    FormsModule,
    FormControl,
    Validators,
} from "@angular/forms";
import { MatCard, MatCardTitle } from "@angular/material/card";
import { MatCheckboxModule } from "@angular/material/checkbox";
import {
    MatDatepicker,
    MatDatepickerModule,
} from "@angular/material/datepicker";
import { MatFormField, MatLabel } from "@angular/material/form-field";
import { MatInputModule } from "@angular/material/input";
import { MatSnackBar } from "@angular/material/snack-bar";
import TaskStateDTO from "../../dtos/task-state.dto";
import { TaskStateService } from "../../services/task-state.service";
import { ActivatedRoute, Router } from "@angular/router";
import TaskReadDTO from "../../dtos/task-read.dto";
import TaskUpdateDTO from "../../dtos/task-update.dto";
import { MatTimepickerModule } from "@angular/material/timepicker";
import { MatOption } from "@angular/material/core";
import { MatSelect } from "@angular/material/select";
import { findInvalidControls } from "../../utils/find-invalid-controls";

@Component({
    selector: "update-task-form",
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
        MatOption,
        MatSelect,
        FormsModule,
    ],
    templateUrl: "./update-task-form.html",
    styleUrl: "./update-task-form.css",
})
export class UpdateTaskForm extends TaskFormBase implements OnInit {
    taskStates: TaskStateDTO[] = [];
    taskToUpdate!: TaskReadDTO;

    constructor(
        tagService: TagService,
        taskService: TaskService,
        matSnackBar: MatSnackBar,
        private taskStateService: TaskStateService,
        private route: ActivatedRoute,
        private router: Router,
    ) {
        super(tagService, taskService, matSnackBar);

        this.taskFormGroup.addControl(
            "taskStates",
            new FormControl("", {
                validators: [Validators.required],
                nonNullable: true,
            }),
        );
    }

    override ngOnInit() {
        super.ngOnInit();
    }

    override onTagsLoaded(): void {
        const taskId = this.route.snapshot.paramMap.get("id");

        if (taskId === null) {
            throw new Error("taskId not set in update form");
        }

        this.taskService.getTask(taskId).subscribe({
            next: (task) => {
                this.taskToUpdate = task;

                this.taskStateService.getTaskStates().subscribe({
                    next: (taskStates) => {
                        this.taskStates = taskStates;

                        this.loadTaskDataIntoForm();
                    },
                    error: (error) =>
                        console.error(
                            `Error while loading task states -> ${error}`,
                        ),
                });
            },
            error: (error) =>
                console.error(`Error while loading task -> ${error}`),
        });
    }

    loadTaskDataIntoForm(): void {
        this.taskFormGroup.controls["title"].setValue(this.taskToUpdate.title);
        this.taskFormGroup.controls["description"].setValue(
            this.taskToUpdate.description,
        );

        if (this.taskToUpdate.deadline !== null) {
            this.taskFormGroup.controls["hasDeadline"].setValue(true);
            this.taskFormGroup.controls["deadlineTime"].setValue(
                this.taskToUpdate.deadline,
            );
            this.taskFormGroup.controls["deadlineDate"].setValue(
                this.taskToUpdate.deadline,
            );
        }

        const tagsCheckboxValues: boolean[] = new Array(this.tags.length).fill(
            false,
        );

        for (let i = 0; i < this.tags.length; ++i) {
            if (this.taskToUpdate.tagNames.includes(this.tags[i].name)) {
                tagsCheckboxValues[i] = true;
            }
        }

        const taskState = this.taskStates.find(
            (taskState) => taskState.name === this.taskToUpdate.taskStateName,
        );

        if (taskState === undefined) {
            throw new Error("Task state doesn't exist");
        }

        this.taskFormGroup.controls["tags"].setValue(tagsCheckboxValues);
        this.taskFormGroup.controls["taskStates"].setValue(taskState.id);
    }

    override onSubmit(): void {
        if (this.taskFormGroup.invalid) {
            console.log(findInvalidControls(this.taskFormGroup));
            return;
        }

        const formData = this.taskFormGroup.value;
        const tagIds = this.getSelectedTags();

        const taskData: TaskUpdateDTO = {
            title: formData.title ?? "",
            description: formData.description ?? "",
            deadline: formData.hasDeadline
                ? (this.getDeadlineFromControls()?.toISOString() ?? "")
                : null,
            taskStateId: formData.taskStates,
            tagIds: tagIds,
        };

        this.taskService.updateTask(this.taskToUpdate.id, taskData).subscribe({
            next: () => {
                this.matSnackBar.open("Task updated successfully!", "Dismiss", {
                    duration: 3000,
                    panelClass: ["success-snackbar"],
                });

                this.router.navigate(["/view-tasks"]);
            },
            error: (error) =>
                this.matSnackBar.open("Failed to update task.", "Dismiss", {
                    duration: 5000,
                    panelClass: ["error-snackbar"],
                }),
        });
    }
}
