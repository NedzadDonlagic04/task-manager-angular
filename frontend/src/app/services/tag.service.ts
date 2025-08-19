import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs";
import TagDTO from "../../dtos/tag.dto";
import { environment } from "../../environments/environment";

@Injectable({
    providedIn: "root",
})
export class TagService {
    tagApiUrl = `${environment.apiUrl}/tag`;

    constructor(private http: HttpClient) {}

    getTags(): Observable<TagDTO[]> {
        return this.http.get<TagDTO[]>(this.tagApiUrl);
    }
}
