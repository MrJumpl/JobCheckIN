import { Component, Input } from '@angular/core';
import { FormGroup, FormGroupDirective } from '@angular/forms';

import { jobchinLocalize } from '../../../../jobchin.localize';
import { NotificationFrequency } from '../../../../models/notificationFrequency.enum';

@Component({
    selector: 'jobchin-notificationFrequency-picker',
    templateUrl: 'notificationFrequencyPicker.component.html',
})
export class NotificationFrequencyPickerComponent {

    @Input() controlName: string;
    @Input() label: string = jobchinLocalize.notificationFrequencyLabel;

    values: NotificationFrequency[];

    constructor(private parent: FormGroupDirective) {
        let values = Object.values(NotificationFrequency);
        this.values = values.slice(values.length / 2, values.length).map(x => x as NotificationFrequency);
    }

    getFormGroup(): FormGroup {
        return this.parent.control;
    }

    translate(value: NotificationFrequency): string {
        switch (value) {
            case NotificationFrequency.Never:
                return jobchinLocalize.notificationFrequencyNever;
            case NotificationFrequency.Immediately:
                return jobchinLocalize.notificationFrequencyImmediately;
            case NotificationFrequency.Daily:
                return jobchinLocalize.notificationFrequencyDaily;
            case NotificationFrequency.Weekly:
                return jobchinLocalize.notificationFrequencyWeekly;
            default:
                return '';
        }
    }
}
