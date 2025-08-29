import {
  AfterViewInit,
  ChangeDetectionStrategy,
  Component,
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
import TaskReadDTO from '../../dtos/task-read.dto';
import { TaskTableRowData } from '../../services/task-table-row-data.service';
import { DatePipe } from '@angular/common';
import { MatIcon } from '@angular/material/icon';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatSort, MatSortModule } from '@angular/material/sort';
import { Router } from '@angular/router';
import {
  FilterData,
  FilterTasksForm,
} from '../filter-tasks-form/filter-tasks-form';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { SelectionModel } from '@angular/cdk/collections';
import { MatDialog } from '@angular/material/dialog';
import { YesNoDialog } from '../yes-no-dialog/yes-no-dialog';

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
    FilterTasksForm,
  ],
  templateUrl: './view-tasks-table.html',
  styleUrl: './view-tasks-table.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ViewTasksTable implements OnInit, AfterViewInit {
  protected readonly NOT_SET_MESSAGE = '<Not set>';

  protected readonly displayedTasksColumns: string[] = [
    'select',
    'rowNum',
    'title',
    'deadline',
    'createdAt',
    'taskStateName',
    'tagNames',
    'actions',
  ];

  protected readonly tasksDataSource =
    new MatTableDataSource<TaskTableRowData>();
  protected readonly selectedTasks = new SelectionModel<TaskTableRowData>(
    true,
    [],
  );

  @ViewChild(MatSort) sort!: MatSort;
  @ViewChild(MatPaginator) paginator!: MatPaginator;

  protected taskService = inject(TaskService);
  protected dialog = inject(MatDialog);
  protected router = inject(Router);

  public constructor() {
    this.tasksDataSource.filterPredicate = this.taskTableFilterPredicate;
  }

  public ngOnInit(): void {
    this.refreshTasksTable();
  }

  public ngAfterViewInit(): void {
    this.tasksDataSource.sort = this.sort;
    this.tasksDataSource.paginator = this.paginator;
  }

  protected navigatoToUpdatePage(taskId: string): void {
    this.router.navigate(['/update', taskId]);
  }

  private refreshTasksTable(): void {
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

  private deleteInstance(taskId: string) {
    this.taskService.deleteTask(taskId).subscribe({
      next: () => this.refreshTasksTable(),
      error: (error: any) => {
        console.error('Error deleting task:', error);
      },
    });
  }

  protected showDeleteDialog(taskId?: string): void {
    const isBatchDelete: boolean = this.isAnyRowSelected();

    const dialogRef = this.dialog.open(YesNoDialog, {
      width: '350px',
      data: {
        title: 'Delete',
        message: isBatchDelete
          ? `Are you sure you want to delete ${this.selectedTasks.selected.length} ${this.selectedTasks.selected.length === 1 ? 'task' : 'tasks'}?`
          : 'Are you sure you want to delete selected task?',
        confirmText: 'Delete',
        cancelText: 'Cancel',
      },
      disableClose: true,
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result && isBatchDelete) this.batchDelete();
      else if (result) this.deleteInstance(taskId!);
    });
  }

  private taskTableFilterPredicate(
    rowData: TaskTableRowData,
    filter: string,
  ): boolean {
    const filterData: FilterData = JSON.parse(filter);

    const searchTerm = filterData.searchTerm.trim().toLocaleLowerCase();
    const deadlineStartDate = new Date(filterData.deadlineStart);
    const deadlineEndDate = new Date(filterData.deadlineEnd);
    const createdAtStartDate = new Date(filterData.createdAtStart);
    const createdAtEndDate = new Date(filterData.createdAtEnd);
    const { taskStateName, tagNames } = filterData;

    const createDateWithoutHours = (dateStr: string): Date => {
      const date = new Date(dateStr);
      date.setHours(0, 0, 0, 0);
      return date;
    };

    const areTagNamesSame = (
      tagNames1: string[],
      tagNames2: string[],
    ): boolean => {
      if (tagNames1.length !== tagNames2.length) {
        return false;
      }

      tagNames1.sort();
      tagNames2.sort();

      for (let i = 0; i < tagNames1.length; ++i) {
        if (tagNames1[i] !== tagNames2[i]) {
          return false;
        }
      }

      return true;
    };

    const searchTermValid =
      searchTerm === '' ||
      rowData.title.toLocaleLowerCase().includes(searchTerm) ||
      rowData.description.toLocaleLowerCase().includes(searchTerm);

    const deadlineStartValid =
      isNaN(deadlineStartDate.getTime()) ||
      (rowData.deadline !== null &&
        createDateWithoutHours(rowData.deadline) >= deadlineStartDate);

    const deadlineEndValid =
      isNaN(deadlineEndDate.getTime()) ||
      (rowData.deadline !== null &&
        createDateWithoutHours(rowData.deadline) <= deadlineEndDate);

    const createdAtStartValid =
      isNaN(createdAtStartDate.getTime()) ||
      createDateWithoutHours(rowData.createdAt) >= createdAtStartDate;

    const createdAtEndValid =
      isNaN(createdAtEndDate.getTime()) ||
      createDateWithoutHours(rowData.createdAt) <= createdAtEndDate;

    const taskStateNameValid =
      taskStateName === '' || taskStateName === rowData.taskStateName;

    const tagNamesValid =
      tagNames.length === 0 || areTagNamesSame(tagNames, rowData.tagNames);

    return (
      searchTermValid &&
      deadlineStartValid &&
      deadlineEndValid &&
      createdAtStartValid &&
      createdAtEndValid &&
      taskStateNameValid &&
      tagNamesValid
    );
  }

  protected filterTasks(filterObjStr: string): void {
    this.tasksDataSource.filter = filterObjStr;
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

  protected batchDelete(): void {
    const tasksToDelete: string[] = [];
    this.selectedTasks.selected.forEach((task) => tasksToDelete.push(task.id));

    this.taskService.deleteMultipleTasks(tasksToDelete).subscribe({
      next: () => {
        this.selectedTasks.deselect(
          ...this.selectedTasks.selected.filter((task) =>
            tasksToDelete.includes(task.id),
          ),
        );
        this.refreshTasksTable();
      },
      error: (error: any) =>
        console.error('Error while deleting tasks:', error),
    });
  }
}
