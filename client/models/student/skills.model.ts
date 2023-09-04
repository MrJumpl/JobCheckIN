import { UploadFileModel } from '../../../../../core/models/upload-file-model';
import { LanguageModel } from '../language.model';
import { StudentSoftSkill } from './studentSoftSkill';

export interface SkillsModel {
    hardSkills: number[];
    educationCertificate: UploadFileModel;
    softSkills: StudentSoftSkill[];
    languages: LanguageModel[];
    languageCertificate: UploadFileModel;
}
