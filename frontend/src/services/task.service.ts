import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import TaskCreateDTO from "../dtos/task-create.dto";
import TaskReadDTO from "../dtos/task-read.dto";
import { environment } from "../environments/environment";

@Injectable({
    providedIn: "root",
})
export class TaskService {
    taskApiUrl = `${environment.apiUrl}/task`;

    constructor(private http: HttpClient) {}

    createTask(taskData: TaskCreateDTO): Observable<TaskReadDTO> {
        return this.http.post<TaskReadDTO>(this.taskApiUrl, taskData);
    }
}
