import { Component } from '@angular/core';
import { ViewTasksTable } from '../view-tasks-table/view-tasks-table';

@Component({
    selector: 'app-view-tasks-page',
    imports: [ViewTasksTable],
    templateUrl: './view-tasks-page.html',
    styleUrl: './view-tasks-page.css',
})
export class ViewTasksPage {}
