import { Component } from '@angular/core';
import { TaskForm } from '../task-form/task-form';

@Component({
    selector: 'app-create-task-page',
    imports: [TaskForm],
    templateUrl: './create-task-page.html',
    styleUrl: './create-task-page.scss',
    standalone: true,
})
export class CreateTaskPage {}
