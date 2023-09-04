import { Component, HostListener, ViewChild } from '@angular/core';
import { Observable } from 'rxjs';

import { JobChINProfileConfig } from '../../../config/profileConfig';
import { ComponentCanDeactivate } from '../../../services/pendingChanges.guard';
import {
    CompanyNotificationSettingsComponent,
} from '../company/notificationSettingsComponent/notificationSettings.component';
import {
    StudentNotificationSettingsComponent,
} from '../student/notificationSettingsComponent/notificationSettings.component';

@Component({
    selector: 'jobchin-notificationSettings',
    templateUrl: 'notificationSettings.component.html',
})
export class NotificationSettingsComponent implements ComponentCanDeactivate {

    @ViewChild(StudentNotificationSettingsComponent) studentComponent: StudentNotificationSettingsComponent;
    @ViewChild(CompanyNotificationSettingsComponent) companyComponent: CompanyNotificationSettingsComponent;

    @HostListener('window:beforeunload') canDeactivate(): Observable<boolean> | boolean {
        if (this.isStudent()) {
            return this.studentComponent.canDeactivate();
        }
        return this.companyComponent.canDeactivate();
    }

    constructor(private profile: JobChINProfileConfig) { }

    isStudent(): boolean {
        return this.profile.student != null;
    }
}
