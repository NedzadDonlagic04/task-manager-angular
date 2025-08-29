import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import TaskCreateDTO from '../dtos/task-create.dto';
import TaskReadDTO from '../dtos/task-read.dto';
import TaskUpdateDTO from '../dtos/task-update.dto';
import { environment } from '../environments/environment';

@Injectable({
    providedIn: 'root',
})
export class TaskService {
    private taskApiUrl = `${environment.apiUrl}/task`;

    public constructor(private http: HttpClient) {}

    public getTask(taskId: string): Observable<TaskReadDTO> {
        return this.http.get<TaskReadDTO>(`${this.taskApiUrl}/${taskId}`);
    }

    public getTasks(): Observable<TaskReadDTO[]> {
        return this.http.get<TaskReadDTO[]>(this.taskApiUrl);
    }

    public createTask(taskData: TaskCreateDTO): Observable<TaskReadDTO> {
        return this.http.post<TaskReadDTO>(this.taskApiUrl, taskData);
    }

    public updateTask(
        taskId: string,
        taskData: TaskUpdateDTO,
    ): Observable<any> {
        return this.http.put(`${this.taskApiUrl}/${taskId}`, taskData);
    }

	public deleteTask(taskId: string): Observable<any> {
		return this.http.delete(`${this.taskApiUrl}/${taskId}`);
	}

    public deleteMultipleTasks(taskIdList: string[]): Observable<any> {
        return this.http.put(this.taskApiUrl, taskIdList)
    }
}
