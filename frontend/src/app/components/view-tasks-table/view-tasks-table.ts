import {
    AfterViewInit,
    ChangeDetectionStrategy,
    Component,
    DestroyRef,
    inject,
    OnInit,
    ViewChild,
} from '@angular/core';
import {
    MatHeaderCell,
    MatHeaderRow,
    MatCell,
    MatRow,
    MatTable,
    MatTableModule,
    MatTableDataSource,
} from '@angular/material/table';
import { TaskService } from '../../services/task.service';
import TaskReadDTO from '../../dtos/task/task-read.dto';
import { TaskTableRow as TaskTableRow } from '../../utils/task-table-row';
import { DatePipe, NgClass } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { Router } from '@angular/router';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SelectionModel } from '@angular/cdk/collections';
import { HttpErrorResponse } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { YesNoDialogService } from '../../services/yes-no-dialog.service';
import { ProcessedFilterTaskFormValue } from '../../services/filter-task-form.service';
import { TaskFilterService } from '../../services/task-filter.service';

@Component({
    selector: 'app-view-tasks-table',
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
        MatCheckboxModule,
        NgClass,
    ],
    templateUrl: './view-tasks-table.html',
    styleUrl: './view-tasks-table.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
    providers: [YesNoDialogService],
})
export class ViewTasksTable implements OnInit, AfterViewInit {
    protected readonly not_set_message = '<Not set>';
    protected readonly displayedTasksColumns: string[] = [
        'select',
        'title',
        'deadline',
        'createdAt',
        'state',
        'tags',
        'actions',
    ];
    protected readonly tasksDataSource = new MatTableDataSource<TaskTableRow>();
    protected readonly selectedTasks = new SelectionModel<TaskTableRow>(
        true,
        [],
    );

    @ViewChild(MatSort) protected sort!: MatSort;
    @ViewChild(MatPaginator) protected paginator!: MatPaginator;

    private readonly _taskService = inject(TaskService);
    private readonly _taskFilterService = inject(TaskFilterService);
    private readonly _router = inject(Router);
    private readonly _destroyRef = inject(DestroyRef);
    private readonly _yesNoDialogService = inject(YesNoDialogService);
    private _processedFilterFormValue: ProcessedFilterTaskFormValue | null =
        null;

    public constructor() {
        this.tasksDataSource.filterPredicate =
            this._taskTableFilterPredicate.bind(this);
    }

    public ngOnInit(): void {
        this._refreshTasksTable();
    }

    public ngAfterViewInit(): void {
        this.tasksDataSource.sort = this.sort;
        this.tasksDataSource.paginator = this.paginator;
    }

    public filterTasks(filterObjStr: string): void {
        this._processedFilterFormValue = JSON.parse(filterObjStr);
        this.tasksDataSource.filter = filterObjStr;
    }

    protected navigateToUpdatePage(taskId: string): void {
        this._router.navigate(['/update', taskId]);
    }

    protected openBatchDeleteDialog(): void {
        if (!this.isAnyRowSelected()) {
            return;
        }

        const dialogRef = this._yesNoDialogService.open({
            title: 'Batch Delete Tasks',
            message: `Are you sure you want to delete ${this.selectedTasks.selected.length} tasks?`,
            confirmText: 'Delete',
            cancelText: 'Cancel',
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this._batchDeleteSelectedTasks();
            }
        });
    }

    protected openDeleteTaskDialog(taskId: string): void {
        const dialogRef = this._yesNoDialogService.open({
            title: 'Delete Task',
            message: `Are you sure you want to delete task?`,
            confirmText: 'Delete',
            cancelText: 'Cancel',
        });

        dialogRef.afterClosed().subscribe((result) => {
            if (result) {
                this._deleteTask(taskId);
            }
        });
    }

    protected isAnyRowSelected(): boolean {
        return this.selectedTasks.selected.length > 0;
    }

    protected areAllRowsSelected(): boolean {
        const numberOfRowsSelected = this.selectedTasks.selected.length;
        const numberOfRowsInTable = this.tasksDataSource.data.length;

        return numberOfRowsInTable === numberOfRowsSelected;
    }

    protected toggleAllRows(): void {
        if (this.areAllRowsSelected()) {
            this.selectedTasks.clear();
            return;
        }

        this.selectedTasks.select(...this.tasksDataSource.data);
    }

    private _batchDeleteSelectedTasks(): void {
        const tasksToDelete: string[] = this.selectedTasks.selected.map(
            (task: TaskTableRow) => task.id,
        );

        this._taskService
            .deleteMultipleTasks(tasksToDelete)
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe({
                next: () => {
                    /*
                        TODO: Consider what would happen if some tasks failed to delete
                        Maybe return the ids of the ones that failed or some shit like that
                        Currently that case is not handled, everything is just deselected after
                        the batch delete operation
                    */
                    this.selectedTasks.clear();
                    this._refreshTasksTable();
                },
                error: (error: HttpErrorResponse) =>
                    console.error(
                        `Error while deleting tasks -> ${error.message}`,
                    ),
            });
    }

    private _taskTableFilterPredicate(row: TaskTableRow, _: string): boolean {
        if (!this._processedFilterFormValue) {
            return true;
        }

        return this._taskFilterService.doesRowMatchFilter(
            row,
            this._processedFilterFormValue,
        );
    }

    private _refreshTasksTable(): void {
        this._taskService
            .getTasks()
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe({
                next: (tasks: TaskReadDTO[]) => {
                    const tasksTableRowData: TaskTableRow[] = tasks.map(
                        (task: TaskReadDTO, index: number) =>
                            new TaskTableRow(index + 1, task),
                    );

                    this.tasksDataSource.data = tasksTableRowData;
                },
                error: (error: HttpErrorResponse) =>
                    console.error(
                        `Error while loading tasks -> ${error.message}`,
                    ),
            });
    }

    private _deleteTask(taskId: string): void {
        this._taskService
            .deleteTask(taskId)
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe({
                next: () => {
                    this._refreshTasksTable();
                },
                error: (error: HttpErrorResponse) =>
                    console.error(
                        `Error while deleting single task -> ${error.message}`,
                    ),
            });
    }
}
