import { CompanyCompanyTypeModel } from '../models/company/companyCompanyType.model';
import { CompanyTypeModel } from './../models/company/companyType.model';

export interface CompanyTypeConfig {
    afterLogin: string;
    currentCompanyTypes: CompanyCompanyTypeModel[];
    companyTypes: CompanyTypeModel[];
}
