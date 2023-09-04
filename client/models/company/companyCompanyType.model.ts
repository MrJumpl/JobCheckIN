import { OcOrderStateModel } from '../../../../_templates/muni-web/models/ocOrderState-model';

export interface CompanyCompanyTypeModel {
    companyTypeId: number;
    numberOfWorkPosition: number | null;
    numberOfStudentsRevealed: number | null;
    activeFrom: Date;
    activeTo: Date;
    confirmed: boolean;
    paid: boolean;
    orderState: OcOrderStateModel;
}
