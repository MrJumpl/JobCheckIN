import { UploadFileModel } from '../../../../../core/models/upload-file-model';

export interface PresentationModel {
    logo: UploadFileModel;
    backgroundImage: UploadFileModel;
    shortDescription: string;
    description: string;
    differences: string;
    interviewDescription: string;
    web: string;
    linkedin: string;
    facebook: string;
}
