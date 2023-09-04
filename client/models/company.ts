import { CandidatesModel } from './company/candidates.model';
import { ContactPersonModel } from './company/contactPerson.model';
import { GeneralInfoModel } from './company/generalInfo.model';
import { CompanyNotificationSettings } from './company/notificaitonSettings.model';
import { PresentationModel } from './company/presentation.model';

export interface CompanyModel {
    companyId: number;
    memberId: number;
    generalInfo: GeneralInfoModel;
    contactPerson: ContactPersonModel;
    candidates: CandidatesModel;
    presentation: PresentationModel;
    notificationSettings: CompanyNotificationSettings;
}
