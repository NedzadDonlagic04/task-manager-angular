import { ChangeDetectionStrategy, Component, OnInit } from "@angular/core";
import {
    MatHeaderCell,
    MatHeaderRow,
    MatCell,
    MatRow,
    MatTable,
    MatTableModule,
    MatTableDataSource,
    MatHeaderRowDef,
} from "@angular/material/table";
import { TaskService } from "../../services/task.service";
import TaskReadDTO from "../../dtos/task-read.dto";
import { TasksTableDataSource } from "../../services/tasks-table-data-source.service";
import { TaskTableRowData } from "../../services/task-table-row-data.service";
import { DatePipe } from "@angular/common";
import { MatIcon } from "@angular/material/icon";
import { CdkTableModule } from "@angular/cdk/table";
import { MatPaginatorModule } from "@angular/material/paginator";
import { MatSortModule } from "@angular/material/sort";

@Component({
    selector: "app-view-tasks-table",
    imports: [
        MatHeaderCell,
        MatHeaderRow,
        MatCell,
        MatRow,
        MatTable,
        MatTableModule,
        DatePipe,
        MatIcon,
    ],
    templateUrl: "./view-tasks-table.html",
    styleUrl: "./view-tasks-table.css",
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ViewTasksTable implements OnInit {
    displayedTasksColumns: string[] = [
        "rowNum",
        "title",
        "description",
        "deadline",
        "createdAt",
        "taskState",
        "tagNames",
        "actions",
    ];
    tasksDataSource: MatTableDataSource<TaskTableRowData>;

    constructor(private taskService: TaskService) {
        this.tasksDataSource = new MatTableDataSource<TaskTableRowData>();
    }

    ngOnInit(): void {
        this.taskService.getTasks().subscribe({
            next: (tasks: TaskReadDTO[]) => {
                const tasksTableRowData: TaskTableRowData[] = tasks.map(
                    (task: TaskReadDTO, index: number) =>
                        new TaskTableRowData(index + 1, task),
                );

                this.tasksDataSource.data = tasksTableRowData;
            },
            error: (error: any) =>
                console.error(`Error while loading tags -> ${error}`),
        });
    }
}
