import { HttpClient, HttpParams, HttpUrlEncodingCodec } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { DateUtils } from '../../../../core/utils/date-utils';
import { mergeObjects } from '../../../../core/utils/object-utils';
import { UmbracoService } from '../../../_shared/services/umbraco.service';
import { WebCentrumBaseService } from '../../../_shared/services/webcentrum-base.service';
import { JobChINProfileConfig } from '../config/profileConfig';
import { StudentAngularConfig } from '../config/studentConfig';
import { StudentRegistrationConfig } from '../config/studentRegistrationConfig';
import { StudentCompanyListView } from '../models/company/studentCompanyListView.model';
import { DownloadResult } from '../models/downloadResult';
import { StudentCreateModel } from '../models/student/create.model';
import { ShowInterestModel } from '../models/student/showInterest.model';
import { StudentWorkPositionsPaged } from '../models/workPosition/studentWorkPositionPaged.model';
import { MembersService } from './../../../members/services/members.service';
import { CompanyFilter } from './../models/company/companyFilter.model';
import { StudentUpdateModel } from './../models/student/update.model';
import { WorkPositionFilterDto } from './../models/workPosition/workPositionFilter.model';

@Injectable()
export class StudentService extends WebCentrumBaseService {

    constructor(private http: HttpClient, protected override readonly umbraco: UmbracoService, private membersService: MembersService, private profile: JobChINProfileConfig) {
        super(umbraco, 'StudentApi/');
    }

    getRegistrationGonfig(): Observable<StudentRegistrationConfig> {
        return this.http.get<StudentRegistrationConfig>(this.apiUrl + 'GetRegistrationGonfig', { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError)
            );
    }

    registerStudent(model: StudentCreateModel): Observable<StudentAngularConfig> {
        return this.http.post<StudentAngularConfig>(this.apiUrl + 'RegisterStudent', model, { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError),
                tap(() => {
                    this.membersService.tryIsLoggedIn().subscribe();
                })
            );
    }

    updateStudent(model: StudentUpdateModel): Observable<StudentUpdateModel> {
        return this.http.post<StudentUpdateModel>(this.apiUrl + 'UpdateStudent', model,  { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError),
                map(cfg => DateUtils.convertDates(cfg)),
                tap(cfg => {
                    this.profile.student.lastTimeUpdatedByStudent = new Date();
                    mergeObjects(this.profile.student.model, cfg, ['basicInfo', 'workExperiences', 'studies', 'skills', 'contact', 'notificationSettings']);
                })
            );
    }

    downloadDocxCv(): Observable<DownloadResult> {
        return this.http.get(this.apiUrl + 'DownloadDocxCv', { headers: this.httpHeaders(), observe: 'response', responseType: 'blob' })
            .pipe(
                map(resp => {
                    let name = undefined;
                    let header = resp.headers.get('content-disposition')?.split(';')?.find(x => x.includes('filename='));
                    if (header) {
                        name = header.replace('filename=', '').trim();
                    }
                    console.log(resp.headers)
                    return ({
                        name: name,
                        data: resp.body,
                    });
                }),
                catchError(this.handleError)
            );
    }

    showInterest(model: ShowInterestModel): Observable<any> {
        return this.http.post<any>(this.apiUrl + 'ShowInterest', model, { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError)
            );
    }

    getWorkPositionsPaged(filter: WorkPositionFilterDto): Observable<StudentWorkPositionsPaged> {
        const params = new HttpParams({
            fromObject: { ...filter },
            encoder: new HttpUrlEncodingCodec()
          })
        return this.http.get<StudentWorkPositionsPaged>(this.apiUrl + 'GetWorkPositionsPaged', { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError),
                map(cfg => DateUtils.convertDates(cfg))
            );
    }

    getCompanies(filter: CompanyFilter): Observable<StudentCompanyListView[]> {
        const params = new HttpParams({
            fromObject: { ...filter },
            encoder: new HttpUrlEncodingCodec()
          })
        return this.http.get<StudentCompanyListView[]>(this.apiUrl + 'GetCompanies', { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError)
            );
    }

    favoriteWorkPosition(workPositionId: number, active: boolean): Observable<any> {
        const params = new HttpParams()
            .set('workPositionId', workPositionId)
            .set('active', active);

        return this.http.post<any>(this.apiUrl + 'FavoriteWorkPosition', null, { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError)
            );
    }
}
