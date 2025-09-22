import { Component } from '@angular/core';
import { UpdateTaskForm } from '../update-task-form/update-task-form';

@Component({
    selector: 'app-update-task-page',
    imports: [UpdateTaskForm],
    templateUrl: './update-task-page.html',
    styleUrl: './update-task-page.scss',
    standalone: true,
})
export class UpdateTaskPage {}
