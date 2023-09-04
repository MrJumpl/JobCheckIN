import { LanguageSkill } from './languageSkill.enum';

export interface LanguageModel {
    languageId: number;
    skill: LanguageSkill;
    optional?: boolean;
}
