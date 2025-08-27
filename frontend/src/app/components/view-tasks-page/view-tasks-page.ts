import { Component, OnInit } from "@angular/core";
import TaskReadDTO from "../../dtos/task-read.dto";
import { TaskService } from "../../services/task.service";
import { DatePipe } from "@angular/common";
import { Router } from "@angular/router";
import { YesNoDialog } from "../yes-no-dialog/yes-no-dialog";
import { MatDialog } from "@angular/material/dialog";

@Component({
    selector: "app-view-tasks-page",
    imports: [DatePipe],
    templateUrl: "./view-tasks-page.html",
    styleUrl: "./view-tasks-page.css",
})
export class ViewTasksPage implements OnInit {
    tasks: TaskReadDTO[] = [];

    constructor(
        private taskService: TaskService,
        private dialog: MatDialog,
        private router: Router,
    ) {}

    ngOnInit(): void {
        this.getAllTasks();
    }

    navigatoToUpdatePage(taskId: string): void {
        // this.router.navigate(["/update", taskId]);
    }

    getAllTasks(): void {
        this.taskService.getTasks().subscribe({
            next: (tasks: TaskReadDTO[]) => {
                this.tasks = tasks;
            },
            error: (error: any) => {
                console.error("Error fetching tasks:", error);
            },
        });
    }

    deleteInstance(taskId: string) {
        this.taskService.deleteTask(taskId).subscribe({
            next: () => this.getAllTasks(),
            error: (error: any) => {
                console.error("Error deleting task:", error);
            }
        });
    }

    showDeleteDialog(taskId: string): void {
        let dialogRef = this.dialog.open(YesNoDialog, {
            width: '350px',
            data: {
                title: 'Delete',
                message: 'Are you sure?',
                confirmText: 'Delete',
                cancelText: 'Cancel',
            },
            disableClose: true,
        });

        dialogRef.afterClosed().subscribe(result => {
            if (result)
                this.deleteInstance(taskId);
        })
    }
}
