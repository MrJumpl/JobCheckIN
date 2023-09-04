import { Component, Input } from '@angular/core';
import { ControlContainer, FormGroupDirective } from '@angular/forms';

import { JobChINFormConfig } from '../../../config/formConfig';

/**
 * Base class for custom implementations of data overview actions (e.g. some operations with selected records which require some UI)
 */
@Component({
    selector: 'jobchin-formFooter',
    templateUrl: 'formFooter.component.html',
    viewProviders: [{ provide: ControlContainer, useExisting: FormGroupDirective }]
})
export class FormFooterComponent {
    @Input() formConfig: JobChINFormConfig<any>;

    constructor() {}
}
