import { Component } from '@angular/core';

import { JobChINProfileConfig } from '../../../../config/profileConfig';

@Component({
    selector: 'jobchin-student-cv',
    templateUrl: 'cv.component.html',
})
export class CvComponent {

    constructor(private profile: JobChINProfileConfig) {

    }
}
