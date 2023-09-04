import { BasicInfoModel } from './basicInfo.model';
import { ContactModel } from './contact.model';
import { StudentNotificationSettings } from './notificationSettings.model';
import { PhotoModel } from './photo.model';
import { SkillsModel } from './skills.model';
import { StudiesModel } from './studies.model';
import { VisibilityModel } from './visibility.model';
import { WorkExperincesModel } from './workExperiences.model';

export class StudentUpdateModel {
    basicInfo: BasicInfoModel;
    photo: PhotoModel;
    workExperiences: WorkExperincesModel
    studies: StudiesModel;
    skills: SkillsModel;
    contact: ContactModel;
    notificationSettings: StudentNotificationSettings;
    visibility: VisibilityModel;
}
