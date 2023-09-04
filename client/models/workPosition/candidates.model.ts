import { LanguageModel } from '../language.model';

export interface CandidatesModel {
    activeDriver: boolean;
    drivingLicense: boolean;
    areaOfInterests: number[];
    hardSkills: number[];
    softSkills: number[];
    languages: LanguageModel[];
    faculties: string[];
}
