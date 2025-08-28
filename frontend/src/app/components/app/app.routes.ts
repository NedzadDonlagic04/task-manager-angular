import { Routes } from '@angular/router';
import { ViewTasksPage } from '../view-tasks-page/view-tasks-page';
import { AddTaskPage } from '../add-task-page/add-task-page';
import { TaskStatisticsPage } from '../task-statistics-page/task-statistics-page';
import { HomePage } from '../home-page/home-page';
import { UpdateTaskPage } from '../update-task-page/update-task-page';

export const routes: Routes = [
    { path: '', title: 'Home', component: HomePage },
    { path: 'add-task', title: 'Add Task', component: AddTaskPage },
    { path: 'view-tasks', title: 'View Tasks', component: ViewTasksPage },
    { path: 'update/:id', title: 'Update Task', component: UpdateTaskPage },
    {
        path: 'task-statistics',
        title: 'Task Statistics',
        component: TaskStatisticsPage,
    },
];
