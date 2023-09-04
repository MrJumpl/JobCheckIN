import { FormDescriptionsDto } from '../models/student/formDescriptions.dto.model';
import { StudyDto } from '../models/student/study.dto.model';
import { StudentUpdateModel } from '../models/student/update.model';

export interface StudentAngularConfig {
    studentId: number;
    uco: number;
    fullName: string;
    muniStudies: StudyDto[];
    lastTimeUpdatedByStudent: Date;
    formDescriptions: FormDescriptionsDto;
    model: StudentUpdateModel;
}
