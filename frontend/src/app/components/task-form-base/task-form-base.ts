import { Component, OnInit } from "@angular/core";
import {
    FormArray,
    FormControl,
    UntypedFormGroup,
    Validators,
} from "@angular/forms";
import TagDTO from "../../dtos/tag.dto";
import { TagService } from "../../services/tag.service";
import { TaskService } from "../../services/task.service";
import { MatSnackBar } from "@angular/material/snack-bar";
import { deadlineDateAheadValidator } from "../../validators/deadline-date-validator";
import { deadlineTimeAheadAnHourValidator } from "../../validators/deadline-time-validator";
import { MatButtonModule } from "@angular/material/button";

@Component({
    selector: "task-form-base",
    imports: [MatButtonModule],
    templateUrl: "./task-form-base.html",
    styleUrl: "./task-form-base.css",
})
export abstract class TaskFormBase implements OnInit {
    private readonly HOUR_IN_MILISECONDS = 3_600_000;

    readonly initialFormValues = {
        title: "",
        description: "",
        hasDeadline: false,
        deadlineDate: new Date().toISOString(),
        deadlineTime: new Date(
            new Date().getTime() + this.HOUR_IN_MILISECONDS,
        ).toISOString(),
    };

    tags: TagDTO[] = [];

    taskFormGroup = new UntypedFormGroup({
        title: new FormControl(this.initialFormValues.title, {
            validators: [
                Validators.required,
                Validators.minLength(3),
                Validators.maxLength(50),
            ],
            nonNullable: true,
        }),
        description: new FormControl(this.initialFormValues.description, {
            validators: [Validators.maxLength(1_000)],
            nonNullable: true,
        }),
        hasDeadline: new FormControl(this.initialFormValues.hasDeadline, {
            nonNullable: true,
        }),
        deadlineDate: new FormControl(this.initialFormValues.deadlineDate, {
            validators: [deadlineDateAheadValidator("hasDeadline")],
            nonNullable: true,
        }),
        deadlineTime: new FormControl(this.initialFormValues.deadlineTime, {
            validators: [deadlineTimeAheadAnHourValidator("hasDeadline")],
            nonNullable: true,
        }),
    });

    constructor(
        protected tagService: TagService,
        protected taskService: TaskService,
        protected matSnackBar: MatSnackBar,
    ) {}

    ngOnInit(): void {
        this.startFetchingTags();
        this.attachValueChangedToHasDeadline();
    }

    startFetchingTags(): void {
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

    attachValueChangedToHasDeadline(): void {
        this.hasDeadlineControl?.valueChanges.subscribe(() => {
            this.deadlineDateControl?.updateValueAndValidity();
            this.deadlineTimeControl?.updateValueAndValidity();
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

    get titleControl() {
        return this.taskFormGroup.get("title");
    }

    get descriptionControl() {
        return this.taskFormGroup.get("description");
    }

    get hasDeadlineControl() {
        return this.taskFormGroup.get("hasDeadline");
    }

    get deadlineTimeControl() {
        return this.taskFormGroup.get("deadlineTime");
    }

    get deadlineDateControl() {
        return this.taskFormGroup.get("deadlineDate");
    }

    getDeadlineFromControls(): Date | null {
        const dateStr: Date | null = this.deadlineDateControl?.value;
        const timeStr: Date | null = this.deadlineTimeControl?.value;

        if (!dateStr || !timeStr) {
            return null;
        }

        const date = new Date(dateStr);
        const time = new Date(timeStr);

        date.setHours(time.getHours());
        date.setMinutes(time.getMinutes());

        return date;
    }
}
