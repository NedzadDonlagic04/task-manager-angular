import { Component } from '@angular/core';
import { TaskForm } from '../task-form/task-form';

@Component({
    selector: 'app-update-task-page',
    imports: [TaskForm],
    templateUrl: './update-task-page.html',
    styleUrl: './update-task-page.scss',
    standalone: true,
})
export class UpdateTaskPage {}
