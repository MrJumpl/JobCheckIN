import { MemberDataModel } from './../../../../members/models/member-data.model';
import { ContactPersonModel } from './contactPerson.model';

export interface TakeOverAccountModel {
    token: string;
    signUpModel: MemberDataModel;
    contactPerson: ContactPersonModel;
}
