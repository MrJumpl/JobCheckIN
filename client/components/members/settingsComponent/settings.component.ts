import { Component } from '@angular/core';

import { JobChINProfileConfig } from '../../../config/profileConfig';

@Component({
    selector: 'jobchin-settings',
    templateUrl: 'settings.component.html',
})
export class SettingsComponent {
    constructor(private profile: JobChINProfileConfig) { }

    isStudent(): boolean {
        return this.profile.student != null;
    }
}
