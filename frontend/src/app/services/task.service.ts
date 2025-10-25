import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import TaskCreateDTO from '../dtos/task/task-create-update.dto';
import TaskReadDTO from '../dtos/task/task-read.dto';
import { environment } from '../environments/environment';
import { plainToInstance } from 'class-transformer';
import TaskCreateUpdateDTO from '../dtos/task/task-create-update.dto';

@Injectable({
    providedIn: 'root',
})
export class TaskService {
    private readonly _taskApiUrl = `${environment.apiUrl}/task`;
    private readonly _httpClient = inject(HttpClient);

    public getTask(taskId: string): Observable<TaskReadDTO> {
        return this._httpClient.get<TaskReadDTO>(
            `${this._taskApiUrl}/${taskId}`,
        );
    }

    public getTasks(): Observable<TaskReadDTO[]> {
        return this._httpClient
            .get<TaskReadDTO[]>(this._taskApiUrl)
            .pipe(
                map((tasks: TaskReadDTO[]) =>
                    plainToInstance(TaskReadDTO, tasks),
                ),
            );
    }

    public createTask(taskData: TaskCreateDTO): Observable<TaskReadDTO> {
        return this._httpClient.post<TaskReadDTO>(this._taskApiUrl, taskData);
    }

    public updateTask(
        taskId: string,
        taskData: TaskCreateUpdateDTO,
    ): Observable<unknown> {
        return this._httpClient.put(`${this._taskApiUrl}/${taskId}`, taskData);
    }

    public deleteTask(taskId: string): Observable<unknown> {
        return this._httpClient.delete(`${this._taskApiUrl}/${taskId}`);
    }

    public deleteMultipleTasks(taskIdList: string[]): Observable<unknown> {
        return this._httpClient.put(this._taskApiUrl, taskIdList);
    }
}
