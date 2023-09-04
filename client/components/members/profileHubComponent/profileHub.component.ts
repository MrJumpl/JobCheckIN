import { Component, OnInit } from '@angular/core';

import { EntyMenuState } from '../../../../../members/models/entry-menu-state.enum';
import { JobChINProfileConfig } from '../../../config/profileConfig';
import { jobchinRoutes } from '../../../jobchin.routes';

@Component({
    selector: 'jobchin-profileHub',
    templateUrl: 'profileHub.component.html',
})
export class ProfileHubComponent implements OnInit {
    EntyMenuState = EntyMenuState;
    jobchinRoutes = jobchinRoutes;

    returnUrl: string;
    returnRoute: string;

    constructor(public profile: JobChINProfileConfig) { }

    ngOnInit() { }

    getCompanyRoute(route: string) {
        return `${jobchinRoutes.company}/${route}`;
    }

    getStudentRoute(route: string) {
        return `${jobchinRoutes.student}/${route}`;
    }
}
