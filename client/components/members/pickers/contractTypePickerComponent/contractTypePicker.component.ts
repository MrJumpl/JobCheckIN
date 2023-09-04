import { Component, Input } from '@angular/core';
import { ControlContainer } from '@angular/forms';

import { JobChINAngularConfig } from '../../../../config/angularConfig';

@Component({
    selector: 'jobchin-contractType-picker',
    templateUrl: 'contractTypePicker.component.html',
    styleUrls: [ '../../../../assets/error.scss' ],
})
export class ContractTypePickerComponent {

    @Input() controlName: string;

    constructor(private config: JobChINAngularConfig, private parent: ControlContainer) {
    }

    getFormControl() {
        return this.parent.control.get(this.controlName);
    }

    getContractTypes()  {
        return  this.config.contractTypes;
    }
}
