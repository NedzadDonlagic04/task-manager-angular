import { Component, OnInit } from "@angular/core";
import {
    FormArray,
    FormControl,
    UntypedFormGroup,
    Validators,
} from "@angular/forms";
import TagDTO from "../../../dtos/tag.dto";
import { TagService } from "../../../services/tag.service";
import { TaskService } from "../../../services/task.service";
import { MatSnackBar } from "@angular/material/snack-bar";

@Component({
    selector: "task-form-base",
    imports: [],
    templateUrl: "./task-form-base.html",
    styleUrl: "./task-form-base.css",
})
export abstract class TaskFormBase implements OnInit {
    tags: TagDTO[] = [];
    hasDeadline: boolean = false;

    taskFormGroup = new UntypedFormGroup({
        title: new FormControl("", {
            validators: [Validators.required],
            nonNullable: true,
        }),
        description: new FormControl("", {
            validators: [Validators.required],
            nonNullable: true,
        }),
        deadline: new FormControl(new Date().toISOString()),
    });

    constructor(
        protected tagService: TagService,
        protected taskService: TaskService,
        protected matSnackBar: MatSnackBar,
    ) {}

    ngOnInit(): void {
        this.tagService.getTags().subscribe({
            next: (tags) => {
                this.tags = tags;

                const tagControls = tags.map(
                    () => new FormControl(false, { nonNullable: true }),
                );

                this.taskFormGroup.addControl(
                    "tags",
                    new FormArray(tagControls),
                );

                this.onTagsLoaded();
            },
            error: (error) =>
                console.error(`Error while loading tags -> ${error}`),
        });
    }

    getSelectedTags(): number[] {
        const tagCheckBoxValues = this.taskFormGroup.value.tags!;
        const tagIds: number[] = [];

        for (let i = 0; i < tagCheckBoxValues.length; ++i) {
            if (tagCheckBoxValues[i]) {
                tagIds.push(this.tags[i].id);
            }
        }

        return tagIds;
    }

    abstract onTagsLoaded(): void;
    abstract onSubmit(): void;
}
