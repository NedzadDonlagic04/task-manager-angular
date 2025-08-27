import {
    AfterViewInit,
    ChangeDetectionStrategy,
    Component,
    OnInit,
    ViewChild,
} from "@angular/core";
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
import { TaskTableRowData } from "../../services/task-table-row-data.service";
import { DatePipe } from "@angular/common";
import { MatIcon } from "@angular/material/icon";
import { CdkTableModule } from "@angular/cdk/table";
import { MatPaginator, MatPaginatorModule } from "@angular/material/paginator";
import { MatSort, MatSortModule } from "@angular/material/sort";
import { Router } from "@angular/router";

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
        MatPaginatorModule,
        MatSortModule,
    ],
    templateUrl: "./view-tasks-table.html",
    styleUrl: "./view-tasks-table.css",
    changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ViewTasksTable implements OnInit, AfterViewInit {
    readonly NOT_SET_MESSAGE = "<Not set>";

    readonly displayedTasksColumns: string[] = [
        "rowNum",
        "title",
        "description",
        "deadline",
        "createdAt",
        "taskStateName",
        "tagNames",
        "actions",
    ];
    readonly tasksDataSource: MatTableDataSource<TaskTableRowData>;

    @ViewChild(MatSort) sort!: MatSort;
    @ViewChild(MatPaginator) paginator!: MatPaginator;

    constructor(
        private taskService: TaskService,
        private router: Router,
    ) {
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

    ngAfterViewInit(): void {
        this.tasksDataSource.sort = this.sort;
        this.tasksDataSource.paginator = this.paginator;
    }

    navigatoToUpdatePage(taskId: string): void {
        this.router.navigate(["/update", taskId]);
    }
}
