import { CompanyUserUpdateModel } from '../models/company/companyUserUpdate.model';
import { CompanyUpdateModel } from '../models/company/update.model';
import { WorkPositionUpdateModel } from '../models/workPosition/update.model';
import { Role } from './../models/company/role.enum';
import { CompanyTypeConfig } from './companyTypeConfig';
import { CompanyUserViewConfig } from './companyUserViewConfig';

export interface CompanyAngularConfig {
    companyId: number;
    confirmed: boolean;
    maxDuration: number;
    companyTypeConfig: CompanyTypeConfig;
    model: CompanyUpdateModel;
    users: CompanyUserViewConfig[];
    user: CompanyUserUpdateModel;
    memberId: number;
    role: Role;
    workPosition: WorkPositionUpdateModel;
    intervals: Interval[];
}
