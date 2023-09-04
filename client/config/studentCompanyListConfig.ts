import { Injectable } from '@angular/core';

import { AreaOfInterest } from './../models/areaOfInterest';


@Injectable()
export class StudentCompanyListConfig {
    companyNoLogoLink: string;
    areaOfInterests: AreaOfInterest[];
}
