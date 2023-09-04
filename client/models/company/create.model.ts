import { MemberDataModel } from './../../../../members/models/member-data.model';
import { ContactPersonModel } from './contactPerson.model';
import { GeneralInfoModel } from './generalInfo.model';

export interface CompanyCreateModel {
    signUpModel: MemberDataModel;
    contactPerson: ContactPersonModel;
    generalInfo: GeneralInfoModel;
}
