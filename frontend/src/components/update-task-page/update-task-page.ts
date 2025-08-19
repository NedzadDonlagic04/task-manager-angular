import { Component } from "@angular/core";
import { UpdateTaskForm } from "../update-task-form/update-task-form";

@Component({
    selector: "update-task-page",
    imports: [UpdateTaskForm],
    templateUrl: "./update-task-page.html",
    styleUrl: "./update-task-page.css",
})
export class UpdateTaskPage {}
