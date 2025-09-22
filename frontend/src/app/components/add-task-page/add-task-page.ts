import { Component } from '@angular/core';
import { CreateTaskForm } from '../create-task-form/create-task-form';

@Component({
    selector: 'app-add-task-page',
    imports: [CreateTaskForm],
    templateUrl: './add-task-page.html',
    styleUrl: './add-task-page.scss',
    standalone: true,
})
export class AddTaskPage {}
