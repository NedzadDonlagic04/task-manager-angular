import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { map, Observable } from 'rxjs';
import TagDTO from '../dtos/tag.dto';
import { environment } from '../environments/environment';
import { plainToClass, plainToInstance } from 'class-transformer';

@Injectable({
    providedIn: 'root',
})
export class TagService {
    private readonly _tagApiUrl = `${environment.apiUrl}/tag`;
    private readonly _http = inject(HttpClient);

    public getTags(): Observable<TagDTO[]> {
        return this._http
            .get<TagDTO[]>(this._tagApiUrl)
            .pipe(map((tags: TagDTO[]) => plainToInstance(TagDTO, tags)));
    }
}
