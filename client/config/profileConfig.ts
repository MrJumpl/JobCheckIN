import { Injectable } from '@angular/core';

import { CompanyAngularConfig } from './companyConfig';
import { StudentAngularConfig } from './studentConfig';

@Injectable()
export class JobChINProfileConfig {
    company: CompanyAngularConfig;
    student: StudentAngularConfig;
}
