import {
    AfterViewInit,
    Component,
    DestroyRef,
    ElementRef,
    inject,
    OnDestroy,
    ViewChild,
} from '@angular/core';
import { Chart } from 'chart.js/auto';
import { HttpErrorResponse } from '@angular/common/http';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import {
    MonthlyTasksData,
    TaskStatisticsService,
} from '../../services/task-statistics.service';

@Component({
    selector: 'app-task-statistics-page',
    imports: [],
    templateUrl: './task-statistics-page.html',
    styleUrl: './task-statistics-page.scss',
    standalone: true,
})
export class TaskStatisticsPage implements AfterViewInit, OnDestroy {
    protected isChartLoading: boolean = true;

    @ViewChild('taskLineChart')
    set chartCanvas(chartCanvas: ElementRef<HTMLCanvasElement>) {
        if (chartCanvas) {
            this._chartCanvas = chartCanvas;
            this._createChart(this._monthlyTasksData);
        }
    }
    private _chartCanvas!: ElementRef<HTMLCanvasElement>;
    private _chart?: Chart;
    private _monthlyTasksData!: MonthlyTasksData;
    private readonly _destroyRef = inject(DestroyRef);
    private readonly _taskStatisticsService = inject(TaskStatisticsService);

    public ngAfterViewInit(): void {
        this._loadMonthlyTaskData();
    }

    public ngOnDestroy(): void {
        this._chart?.destroy();
    }

    private _loadMonthlyTaskData(): void {
        this._taskStatisticsService
            .getMonthlyTaskData()
            .pipe(takeUntilDestroyed(this._destroyRef))
            .subscribe({
                next: (monthlyTasksData: MonthlyTasksData) => {
                    this.isChartLoading = false;
                    this._monthlyTasksData = monthlyTasksData;
                },
                error: (error: HttpErrorResponse) =>
                    console.error(
                        `Error when fetching tasks for chart -> ${error.message}`,
                    ),
            });
    }

    private _createChart(monthlyTasksData: MonthlyTasksData): void {
        this._chart?.destroy();

        this._chart = new Chart(this._chartCanvas.nativeElement, {
            type: 'line',
            data: {
                labels: monthlyTasksData.monthsLabels as string[],
                datasets: [
                    {
                        label: 'Task Creations',
                        data: monthlyTasksData.taskCountPerMonth,
                        fill: true,
                        tension: 0.5,
                    },
                ],
            },
        });
    }
}
