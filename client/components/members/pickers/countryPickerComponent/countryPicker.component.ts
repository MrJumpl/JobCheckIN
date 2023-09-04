import { Component, Input } from '@angular/core';
import { ControlContainer } from '@angular/forms';

import { JobChINAngularConfig } from '../../../../config/angularConfig';

@Component({
    selector: 'jobchin-coutry-picker',
    templateUrl: 'countryPicker.component.html',
    styleUrls: [ '../../../../assets/error.scss' ],
})
export class CountryPickerComponent {

    @Input() controlName: string;

    constructor(private config: JobChINAngularConfig, private parent: ControlContainer) {
    }

    getCountries() {
        return this.config.countries;
    }

    getFormControl() {
        return this.parent.control.get(this.controlName);
    }
}
