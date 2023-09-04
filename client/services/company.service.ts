import { HttpClient, HttpParams, HttpUrlEncodingCodec } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { catchError, map, tap } from 'rxjs/operators';

import { DateUtils } from '../../../../core/utils/date-utils';
import { mergeObjects } from '../../../../core/utils/object-utils';
import { UmbracoService } from '../../../_shared/services/umbraco.service';
import { WebCentrumBaseService } from '../../../_shared/services/webcentrum-base.service';
import { CompanyRegistrationConfig } from '../config/companyRegistrationConfig';
import { CompanyUserViewConfig } from '../config/companyUserViewConfig';
import { JobChINProfileConfig } from '../config/profileConfig';
import { BuyCompanyTypeDto } from '../models/company/buyCompanyType.dto.model';
import { ChangeRoleModel } from '../models/company/changeRole.model';
import { CompanyUserUpdateModel } from '../models/company/companyUserUpdate.model';
import { CompanyCreateModel } from '../models/company/create.model';
import { GeneralInfoDTO } from '../models/company/generalInfo.dto.model';
import { TakeOverAccountModel as NewCompanyUserModel } from '../models/company/newCompanyUser.model';
import { SendUserInvitation } from '../models/company/sendUserInvitation';
import { CompanyUpdateModel } from '../models/company/update.model';
import { WorkPositionCreateModel } from '../models/workPosition/create.model';
import { WorkPositionUpdateModel } from '../models/workPosition/update.model';
import { MembersService } from './../../../members/services/members.service';
import { SearchStudentFilterDto } from './../models/company/searchStudentFilter.model';
import { CompanyWorkPositionsPaged } from './../models/workPosition/companyWorkPositionPaged.model';
import { WorkPositionStudentsPaged } from './../models/workPosition/workPositionStudentsPaged.model';

@Injectable()
export class CompanyService extends WebCentrumBaseService {

    constructor(private http: HttpClient, private membersService: MembersService, protected override readonly umbraco: UmbracoService, private profile: JobChINProfileConfig) {
        super(umbraco, 'CompanyApi/');
    }

    validateCompanyIco(ico: string): Observable<GeneralInfoDTO> {
        const params = new HttpParams().set('ico', ico);

        return this.http.get<GeneralInfoDTO>(this.apiUrl + 'ValidateCompanyIco', { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError)
            );
    }

    getRegistrationGonfig(): Observable<CompanyRegistrationConfig> {
        return this.http.get<CompanyRegistrationConfig>(this.apiUrl + 'GetRegistrationGonfig', { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError)
            );
    }

    registerCompany(model: CompanyCreateModel): Observable<string> {
        return this.http.post<string>(this.apiUrl + 'RegisterCompany', model,  { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError)
            );
    }

    updateCompany(model: CompanyUpdateModel): Observable<CompanyUpdateModel> {
        return this.http.post<CompanyUpdateModel>(this.apiUrl + 'UpdateCompany', model,  { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError),
                tap(cfg => {
                    mergeObjects(this.profile.company.model, cfg, ['generalInfo', 'candidates', 'presentation', 'branches']);
                })
            );
    }

    updateCompanyUser(model: CompanyUserUpdateModel): Observable<CompanyUserUpdateModel> {
        return this.http.post<CompanyUserUpdateModel>(this.apiUrl + 'UpdateUser', model,  { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError),
                tap(cfg => {
                    mergeObjects(this.profile.company.user, cfg, ['contactPerson', 'notificationSettings']);
                    this.membersService.refreshUserMenu();
                })
            );
    }

    sendInvitation(model: SendUserInvitation): Observable<any> {
        return this.http.post<any>(this.apiUrl + 'SendInvitaion', model,  { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError)
            );
    }

    createNewUser(model: NewCompanyUserModel): Observable<any> {
        return this.http.post<any>(this.apiUrl + 'CreateNewUser', model,  { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError)
            );
    }

    buyCompanyType(model: BuyCompanyTypeDto): Observable<string> {
        return this.http.post<string>(this.apiUrl + 'BuyCompanyType', model, { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError)
            );
    }

    downloadInvoice(requestId: number): Observable<any> {
        return this.http.post(this.apiUrl + 'DownloadInvoice', requestId, {
            responseType: 'arraybuffer',
            observe: 'response',
            headers: this.httpHeaders()
        })
        .pipe(
            catchError(this.handleError)
        );
    }

    createWorkPosition(model: WorkPositionCreateModel): Observable<WorkPositionUpdateModel> {
        return this.http.post<WorkPositionUpdateModel>(this.apiUrl + 'CreateWorkPosition', model,  { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError),
                map(cfg => DateUtils.convertDates(cfg)),
                tap((cfg: WorkPositionUpdateModel) => {
                    this.profile.company.workPosition = cfg;
                })
            );
    }

    updateWorkPosition(model: WorkPositionUpdateModel): Observable<WorkPositionUpdateModel> {
        return this.http.post<WorkPositionUpdateModel>(this.apiUrl + 'UpdateWorkPosition', model,  { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError),
                map(cfg => DateUtils.convertDates(cfg)),
                tap((cfg: WorkPositionUpdateModel) => {
                    if (this.profile.company.workPosition?.workPositionId === cfg.workPositionId) {
                        this.profile.company.workPosition = mergeObjects(this.profile.company.workPosition, cfg, ['workPositionId', 'basicInfo', 'detail', 'candidates', 'candidateRequest']);
                    }
                })
            );
    }

    getWorkPositionPreview(model: WorkPositionUpdateModel): Observable<string> {
        return this.http.post<string>(this.apiUrl + 'GetWorkPositionPreview', model, { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError),
            );
    }

    getCurrentWorkPositionPaged(page: number): Observable<CompanyWorkPositionsPaged> {
        const params = new HttpParams().set('page', page);
        return this.http.get<CompanyWorkPositionsPaged>(this.apiUrl + 'GetCurrentWorkPositionPaged', { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError),
                map(cfg => DateUtils.convertDates(cfg))
            );
    }

    getArchivedWorkPositionPaged(page: number): Observable<CompanyWorkPositionsPaged> {
        const params = new HttpParams().set('page', page);
        return this.http.get<CompanyWorkPositionsPaged>(this.apiUrl + 'GetArchivedWorkPositionPaged', { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError),
                map(cfg => DateUtils.convertDates(cfg))
            );
    }

    getWorkPositionDetail(workPositionId: number): Observable<WorkPositionUpdateModel> {
        const params = new HttpParams().set('workPositionId', workPositionId);
        return this.http.get<WorkPositionUpdateModel>(this.apiUrl + 'GetWorkPositionDetail', { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError),
                map(cfg => DateUtils.convertDates(cfg))
            );
    }

    getCompanyFreeSlots(workPositionId: number): Observable<Interval[]> {
        let params = new HttpParams();
        if (workPositionId) {
            params = params.set('workPositionId', workPositionId);
        }
        return this.http.get<Interval[]>(this.apiUrl + 'GetCompanyFreeSlots', { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError),
                map(cfg => DateUtils.convertDates(cfg))
            );
    }

    getWorkPositionStudents(workPositionId: number, page: number): Observable<WorkPositionStudentsPaged> {
        const params = new HttpParams().set('workPositionId', workPositionId)
            .set('page', page);
        return this.http.get<WorkPositionStudentsPaged>(this.apiUrl + 'GetWorkPositionStudents', { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError),
                map(cfg => DateUtils.convertDates(cfg))
            );
    }

    deleteUser(memberId: number): Observable<any>  {
        return this.http.post(this.apiUrl + 'DeleteUser', memberId, { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError),
                tap(_ => {
                    this.profile.company.users = this.profile.company.users.filter(x => x.memberId !== memberId);
                }),
            );
    }

    changeRole(model: ChangeRoleModel) {
        return this.http.post<CompanyUserViewConfig>(this.apiUrl + 'ChangeRole', model, { headers: this.httpHeaders() })
            .pipe(
                catchError(this.handleError),
                tap((cfg: CompanyUserViewConfig) => {
                    var index = this.profile.company.users.findIndex(x => x.memberId === cfg.memberId);
                    if (index >= 0) {
                        this.profile.company.users[index] = cfg;
                    }
                }),
            );
    }

    searchStudents(filter: SearchStudentFilterDto): Observable<WorkPositionStudentsPaged> {
        const params = new HttpParams({
            fromObject: { ...filter },
            encoder: new HttpUrlEncodingCodec()
          })
        return this.http.get<WorkPositionStudentsPaged>(this.apiUrl + 'SearchStudentPaged', { headers: this.httpHeaders(), params: params })
            .pipe(
                catchError(this.handleError),
                map(cfg => DateUtils.convertDates(cfg))
            );
    }
}
