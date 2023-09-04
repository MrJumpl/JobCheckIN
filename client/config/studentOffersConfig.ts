import { Injectable } from '@angular/core';

import { Language } from './../../MU_Sign/models/languageModel';
import { AreaOfInterest } from './../models/areaOfInterest';
import { ContractType } from './../models/contractType';
import { EnumerablePickerValue } from './../models/enumerablePickerValue';

@Injectable()
export class StudentOffersConfig {
    companyNoLogoLink: string;
    areaOfInterests: AreaOfInterest[];
    localAdministrativeUnits: EnumerablePickerValue<string>[];
    languages: Language[];
    contractTypes: ContractType[];
}


