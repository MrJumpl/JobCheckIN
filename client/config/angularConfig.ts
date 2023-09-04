import { Injectable } from '@angular/core';

import { AreaOfInterest } from '../models/areaOfInterest';
import { ContractType } from '../models/contractType';
import { EnumerablePickerValue } from '../models/enumerablePickerValue';
import { HardSkill } from '../models/hardSkill';
import { Language } from '../models/language';
import { SoftSkill } from '../models/softSkill';
import { WorkerCountRange } from '../models/workerCountRange';

@Injectable()
export class JobChINAngularConfig {
    studentAfterLogin: string;
    areaOfInterests: AreaOfInterest[];
    hardSkills: HardSkill[];
    softSkills: SoftSkill[];
    workerCountRanges: WorkerCountRange[];
    countries: EnumerablePickerValue<number>[];
    localAdministrativeUnits: EnumerablePickerValue<string>[];
    faculties: EnumerablePickerValue<string>[];
    languages: Language[];
    contractTypes: ContractType[];
}
