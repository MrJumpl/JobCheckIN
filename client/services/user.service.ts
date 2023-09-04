import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';

import { UmbracoService } from '../../../_shared/services/umbraco.service';
import { WebCentrumBaseService } from '../../../_shared/services/webcentrum-base.service';

@Injectable()
export class UserService extends WebCentrumBaseService {

    constructor(private http: HttpClient, protected override readonly umbraco: UmbracoService) {
        super(umbraco, 'JobChINUserApi/');
    }

    suggestHardSkill(name: string): Observable<any> {
        const params = new HttpParams().set('name', name);

        return this.http.post<any>(this.apiUrl + 'SuggestHardSkill', null, { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError)
            );
    }
}
