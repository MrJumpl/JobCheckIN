import { LanguageModel } from './../language.model';

export interface CandidatesModel {
    areaOfInterests: number[],
    faculties: string[],
    languageSkillPrefered: LanguageModel[],
    peopleTypesSought: string,
}
