import { Component } from '@angular/core';

import { JobChINProfileConfig } from '../../../../config/profileConfig';

@Component({
    selector: 'jobchin-student-dashboard',
    templateUrl: 'dashboard.component.html',
})
export class DashboardComponent {

    constructor(private profile: JobChINProfileConfig) {

    }
}
