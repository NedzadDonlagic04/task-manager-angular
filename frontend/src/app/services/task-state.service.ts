import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { environment } from "../environments/environment";
import TaskStateDTO from "../dtos/task-state.dto";

@Injectable({
    providedIn: "root",
})
export class TaskStateService {
    tagApiUrl = `${environment.apiUrl}/task-state`;

    constructor(private http: HttpClient) {}

    getTaskStates(): Observable<TaskStateDTO[]> {
        return this.http.get<TaskStateDTO[]>(this.tagApiUrl);
    }
}
