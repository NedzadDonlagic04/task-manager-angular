import { Component, Input, OnInit } from "@angular/core";
import {
    FormArray,
    FormControl,
    FormGroup,
    FormsModule,
    ReactiveFormsModule,
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
import { TagService } from "../../services/tag.service";
import TagDTO from "../../dtos/tag.dto";
import TaskCreateDTO from "../../dtos/task-create.dto";
import { TaskService } from "../../services/task.service";

@Component({
    selector: "task-form",
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
    templateUrl: "./task-form.html",
    styleUrl: "./task-form.css",
})
export class TaskForm implements OnInit {
    tags: TagDTO[] = [];
    hasDeadline: boolean = false;

    addTaskForm = new FormGroup({
        title: new FormControl("", {
            validators: [Validators.required],
            nonNullable: true,
        }),
        description: new FormControl("", {
            validators: [Validators.required],
            nonNullable: true,
        }),
        deadline: new FormControl(new Date().toISOString()),
        tags: new FormArray([] as FormControl<boolean>[]),
    });

    constructor(
        private tagService: TagService,
        private taskService: TaskService,
    ) {}

    ngOnInit(): void {
        this.tagService.getTags().subscribe({
            next: (tags) => {
                this.tags = tags;

                const tagControls = tags.map(
                    () => new FormControl(false, { nonNullable: true }),
                );

                this.addTaskForm.setControl("tags", new FormArray(tagControls));
            },
            error: (error) =>
                console.error(`Error while loading tags -> ${error}`),
        });
    }

    getSelectedTags(): number[] {
        const tagCheckBoxValues = this.addTaskForm.value.tags!;
        const tagIds: number[] = [];

        for (let i = 0; i < tagCheckBoxValues.length; ++i) {
            if (tagCheckBoxValues[i]) {
                tagIds.push(this.tags[i].id);
            }
        }

        return tagIds;
    }

    onSubmit(): void {
        const formData = this.addTaskForm.value;
        const tagIds: number[] = this.getSelectedTags();

        const taskData: TaskCreateDTO = {
            title: formData.title ?? "",
            description: formData.description ?? "",
            deadline: this.hasDeadline ? (formData.deadline ?? "") : null,
            tagIds: tagIds,
        };

        this.taskService.createTask(taskData).subscribe({
            next: (obj) => console.log(obj),
            error: (error) => console.error(error),
        });
    }
}
