import { Component } from '@angular/core';

import { JobChINProfileConfig } from '../../../config/profileConfig';

@Component({
    selector: 'jobchin-offers',
    templateUrl: 'offers.component.html',
})
export class OffersComponent {
    constructor(private profile: JobChINProfileConfig) { }

    isStudent(): boolean {
        return this.profile.student != null;
    }
}
