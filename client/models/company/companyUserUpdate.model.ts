import { ContactPersonModel } from './contactPerson.model';
import { CompanyNotificationSettings } from './notificaitonSettings.model';

export class CompanyUserUpdateModel {
    contactPerson: ContactPersonModel;
    notificationSettings: CompanyNotificationSettings;
}
