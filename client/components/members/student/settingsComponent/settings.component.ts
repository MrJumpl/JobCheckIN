import { Component } from '@angular/core';

import { EntyMenuState } from '../../../../../../members/models/entry-menu-state.enum';
import { jobchinRoutes } from '../../../../jobchin.routes';

@Component({
    selector: 'jobchin-student-settings',
    templateUrl: 'settings.component.html',
})
export class StudentSettingsComponent {
    EntyMenuState = EntyMenuState;
    jobchinRoutes = jobchinRoutes;

    constructor() { }
}
