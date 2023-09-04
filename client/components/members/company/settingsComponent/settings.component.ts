import { Component } from '@angular/core';

import { EntyMenuState } from '../../../../../../members/models/entry-menu-state.enum';
import { jobchinRoutes } from '../../../../jobchin.routes';
import { JobChINProfileConfig } from './../../../../config/profileConfig';
import { Role } from './../../../../models/company/role.enum';

@Component({
    selector: 'jobchin-company-settings',
    templateUrl: 'settings.component.html',
})
export class CompanySettingsComponent {
    EntyMenuState = EntyMenuState;
    jobchinRoutes = jobchinRoutes;

    constructor(private profile: JobChINProfileConfig) { }

    getName(): string {
        return `${this.profile.company.user.contactPerson.firstname} ${this.profile.company.user.contactPerson.surname}`;
    }

    isCompanyAdmin(): boolean {
        return this.profile.company.role === Role.CompanyAdmin;
    }
}
