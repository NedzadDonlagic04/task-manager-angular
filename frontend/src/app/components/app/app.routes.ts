import { Route } from '@angular/router';
import { HomePage } from '../home-page/home-page';

export interface AppRoute extends Route {
    showInNav: boolean;
}

export const routes: AppRoute[] = [
    { path: '', title: 'Home', component: HomePage, showInNav: true },
    {
        path: 'add-task',
        title: 'Add Task',
        loadComponent: () =>
            import('../create-task-page/create-task-page').then(
                (module) => module.CreateTaskPage,
            ),
        showInNav: true,
    },
    {
        path: 'view-tasks',
        title: 'View Tasks',
        loadComponent: () =>
            import('../view-tasks-page/view-tasks-page').then(
                (module) => module.ViewTasksPage,
            ),
        showInNav: true,
    },
    {
        path: 'update/:task-id',
        title: 'Update Task',
        loadComponent: () =>
            import('../update-task-page/update-task-page').then(
                (module) => module.UpdateTaskPage,
            ),
        showInNav: false,
    },
    {
        path: 'task-statistics',
        title: 'Task Statistics',
        loadComponent: () =>
            import('../task-statistics-page/task-statistics-page').then(
                (module) => module.TaskStatisticsPage,
            ),
        showInNav: true,
    },
];
