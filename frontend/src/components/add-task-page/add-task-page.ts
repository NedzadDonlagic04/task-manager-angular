import { Component } from "@angular/core";
import { TaskForm } from "../task-form/task-form";

@Component({
    selector: "app-add-task-page",
    imports: [TaskForm],
    templateUrl: "./add-task-page.html",
    styleUrl: "./add-task-page.css",
})
export class AddTaskPage {}
