import { Routes } from '@angular/router';
import { HomePage } from '../home-page/home-page';

export const routes: Routes = [
    { path: '', title: 'Home', component: HomePage },
    {
        path: 'add-task',
        title: 'Add Task',
        loadComponent: () =>
            import('../add-task-page/add-task-page').then(
                (module) => module.AddTaskPage,
            ),
    },
    {
        path: 'view-tasks',
        title: 'View Tasks',
        loadComponent: () =>
            import('../view-tasks-page/view-tasks-page').then(
                (module) => module.ViewTasksPage,
            ),
    },
    {
        path: 'update/:id',
        title: 'Update Task',
        loadComponent: () =>
            import('../update-task-page/update-task-page').then(
                (module) => module.UpdateTaskPage,
            ),
    },
    {
        path: 'task-statistics',
        title: 'Task Statistics',
        loadComponent: () =>
            import('../task-statistics-page/task-statistics-page').then(
                (module) => module.TaskStatisticsPage,
            ),
    },
];
