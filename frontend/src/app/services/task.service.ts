import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import TaskCreateDTO from "../dtos/task-create.dto";
import TaskReadDTO from "../dtos/task-read.dto";
import TaskUpdateDTO from "../dtos/task-update.dto";
import { environment } from "../environments/environment";

@Injectable({
    providedIn: "root",
})
export class TaskService {
    taskApiUrl = `${environment.apiUrl}/task`;

    constructor(private http: HttpClient) {}

    getTask(taskId: string): Observable<TaskReadDTO> {
        return this.http.get<TaskReadDTO>(`${this.taskApiUrl}/${taskId}`);
    }

    getTasks(): Observable<TaskReadDTO[]> {
        return this.http.get<TaskReadDTO[]>(this.taskApiUrl);
    }

    createTask(taskData: TaskCreateDTO): Observable<TaskReadDTO> {
        return this.http.post<TaskReadDTO>(this.taskApiUrl, taskData);
    }

    updateTask(taskId: string, taskData: TaskUpdateDTO): Observable<any> {
        return this.http.put(`${this.taskApiUrl}/${taskId}`, taskData);
    }
}
