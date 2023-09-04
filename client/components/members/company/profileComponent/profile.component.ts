import { Component } from '@angular/core';

import { EntyMenuState } from '../../../../../../members/models/entry-menu-state.enum';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinRoutes } from '../../../../jobchin.routes';

@Component({
    selector: 'jobchin-company-profile',
    templateUrl: 'profile.component.html',
})
export class CompanyProfileComponent {
    EntyMenuState = EntyMenuState;
    jobchinRoutes = jobchinRoutes;

    constructor(public profile: JobChINProfileConfig) { }
}
