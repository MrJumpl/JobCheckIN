import { Component } from '@angular/core';

import { JobChINProfileConfig } from '../../../../config/profileConfig';

@Component({
    selector: 'jobchin-student-offers',
    templateUrl: 'offers.component.html',
})
export class StudentOffersComponent {

    constructor(private profile: JobChINProfileConfig) {

    }
}
