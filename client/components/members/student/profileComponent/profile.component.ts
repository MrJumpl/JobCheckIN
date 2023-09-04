import { Component } from '@angular/core';

import { EntyMenuState } from '../../../../../../members/models/entry-menu-state.enum';
import { jobchinRoutes } from '../../../../../../sites/JobChIN/jobchin.routes';

@Component({
    selector: 'jobchin-student-profile',
    templateUrl: 'profile.component.html',
})
export class StudentProfileComponent {
    EntyMenuState = EntyMenuState;
    jobchinRoutes = jobchinRoutes;

    constructor() { }
}
