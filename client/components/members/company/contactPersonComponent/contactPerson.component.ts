import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { ContactPersonModel } from '../../../../models/company/contactPerson.model';
import { CompanyService } from '../../../../services/company.service';
import { CompanyUserBaseForm } from '../companyUserBaseForm';

@Component({
    selector: 'jobchin-company-contactPerson',
    templateUrl: 'contactPerson.component.html',
})
export class ContactPersonComponent extends CompanyUserBaseForm<ContactPersonModel> {
    jobchinSettings = jobchinSettings;
    
    override get propertyName(): string {
        return 'contactPerson'
    }

    constructor(protected override companyService: CompanyService, protected override profile: JobChINProfileConfig) {
        super(companyService, profile);
    }

    initForm(): FormGroup {
        return new FormGroup({
            firstname: new FormControl(null, [Validators.required, Validators.maxLength(jobchinSettings.contactMaxLength)]),
            surname: new FormControl(null, [Validators.required, Validators.maxLength(jobchinSettings.contactMaxLength)]),
            phone: new FormControl(null, [Validators.required, Validators.maxLength(jobchinSettings.contactMaxLength)]),
        });
    }

    mapModel(form: FormGroup): ContactPersonModel {
        return {
            firstname: form.controls.firstname.value,
            surname: form.controls.surname.value,
            phone: form.controls.phone.value,
        }
    }

    initModel(model: ContactPersonModel): void {
        this.formConfig.form.controls.firstname.setValue(model.firstname);
        this.formConfig.form.controls.surname.setValue(model.surname);
        this.formConfig.form.controls.phone.setValue(model.phone);
    }
}
