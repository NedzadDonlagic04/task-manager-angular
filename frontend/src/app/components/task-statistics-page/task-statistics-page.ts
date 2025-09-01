import {
    AfterViewInit,
    ChangeDetectionStrategy,
    Component,
    inject,
} from '@angular/core';
import { Chart } from 'chart.js/auto';
import { TaskService } from '../../services/task.service';
import TaskReadDTO from '../../dtos/task-read.dto';

@Component({
    selector: 'app-task-statistics-page',
    imports: [],
    templateUrl: './task-statistics-page.html',
    styleUrl: './task-statistics-page.scss',
    changeDetection: ChangeDetectionStrategy.OnPush,
    standalone: true,
})
export class TaskStatisticsPage implements AfterViewInit {
    protected taskService = inject(TaskService);

    private tasks!: TaskReadDTO[];

    public ngAfterViewInit(): void {
        this.taskService.getTasks().subscribe({
            next: (tasks: TaskReadDTO[]) => {
                this.tasks = tasks;
                this.createChart();
            },
            error: (error: any) =>
                console.error(
                    `Error when fetching tasks for chart -> ${error}`,
                ),
        });
    }

    private createChart(): void {
        const monthNames = [
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
        ];
        const tasksByMonth = TaskStatisticsPage.getTasksPerMonthCount(
            this.tasks,
        );

        const chart = new Chart(
            document.getElementById('task-line-chart') as HTMLCanvasElement,
            {
                type: 'line',
                data: {
                    labels: monthNames,
                    datasets: [
                        {
                            label: 'Task Creations',
                            data: tasksByMonth,
                            fill: true,
                            tension: 0.5,
                        },
                    ],
                },
            },
        );
    }

    private static getTasksPerMonthCount(tasks: TaskReadDTO[]): number[] {
        const tasksPerMonth = new Array(12).fill(0);

        for (const task of tasks) {
            const createdAt = new Date(task.created_At);
            const month = createdAt.getMonth();

            if (month >= 0 && month < 12) {
                ++tasksPerMonth[month];
            }
        }

        return tasksPerMonth;
    }
}
