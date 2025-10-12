import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import { environment } from '../environments/environment';
import TaskStateDTO from '../dtos/task-state.dto';
import { plainToInstance } from 'class-transformer';

@Injectable({
    providedIn: 'root',
})
export class TaskStateService {
    private readonly _tagApiUrl = `${environment.apiUrl}/task-state`;
    private readonly _httpClient = inject(HttpClient);

    public getTaskStates(): Observable<TaskStateDTO[]> {
        return this._httpClient
            .get<TaskStateDTO[]>(this._tagApiUrl)
            .pipe(
                map((taskStates: TaskStateDTO[]) =>
                    plainToInstance(TaskStateDTO, taskStates),
                ),
            );
    }
}
