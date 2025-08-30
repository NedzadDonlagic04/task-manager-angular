import { Component } from '@angular/core';
import { ViewTasksTable } from '../view-tasks-table/view-tasks-table';
import { FilterTasksForm } from '../filter-tasks-form/filter-tasks-form';

@Component({
    selector: 'app-view-tasks-page',
    imports: [ViewTasksTable, FilterTasksForm],
    templateUrl: './view-tasks-page.html',
    styleUrl: './view-tasks-page.scss',
})
export class ViewTasksPage {}
