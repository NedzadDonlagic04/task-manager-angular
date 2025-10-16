import { inject, Injectable } from '@angular/core';
import { TaskService } from './task.service';
import { map, Observable } from 'rxjs';
import TaskReadDTO from '../dtos/task/task-read.dto';

const MONTH_NAMES = [
    'Jan',
    'Feb',
    'Mar',
    'Apr',
    'May',
    'Jun',
    'Jul',
    'Aug',
    'Sep',
    'Oct',
    'Nov',
    'Dec',
] as const;

export interface MonthlyTasksData {
    monthsLabels: readonly string[];
    taskCountPerMonth: number[];
}

@Injectable({
    providedIn: 'root',
})
export class TaskStatisticsService {
    private readonly _taskService = inject(TaskService);

    public getMonthlyTaskData(): Observable<MonthlyTasksData> {
        return this._taskService
            .getTasks()
            .pipe(
                map((tasks: TaskReadDTO[]) =>
                    this._calculateMonthlyData(tasks),
                ),
            );
    }

    private _calculateMonthlyData(tasks: TaskReadDTO[]): MonthlyTasksData {
        const taskCountPerMonth: number[] = Array(MONTH_NAMES.length).fill(0);

        tasks
            .filter((task: TaskReadDTO) => {
                const dateTimeNow = new Date();

                return (
                    dateTimeNow.getFullYear() === task.createdAt.getFullYear()
                );
            })
            .forEach((task: TaskReadDTO) => {
                const month = task.createdAt.getMonth();

                ++taskCountPerMonth[month];
            });

        return {
            monthsLabels: MONTH_NAMES,
            taskCountPerMonth: taskCountPerMonth,
        };
    }
}
