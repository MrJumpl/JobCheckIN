import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { CompanyNotificationSettings } from '../../../../models/company/notificaitonSettings.model';
import { CompanyService } from '../../../../services/company.service';
import { CompanyUserBaseForm } from '../companyUserBaseForm';

@Component({
    selector: 'jobchin-company-notificationSettings',
    templateUrl: 'notificationSettings.component.html',
})
export class CompanyNotificationSettingsComponent extends CompanyUserBaseForm<CompanyNotificationSettings> {
    jobchinSettings = jobchinSettings;

    override get propertyName(): string {
        return 'notificationSettings'
    }

    constructor(protected override companyService: CompanyService, protected override profile: JobChINProfileConfig) {
        super(companyService, profile);
    }

    initForm(): FormGroup {
        return new FormGroup({
            frequency: new FormControl(null),
            notificationEmail: new FormControl(null, Validators.maxLength(jobchinSettings.notificationEmailMaxLength)),
        });
    }

    mapModel(form: FormGroup): CompanyNotificationSettings {
        return {
            notificationFrequency: form.controls.frequency.value,
            notificationEmail: form.controls.notificationEmail.value,
        };
    }

    initModel(model: CompanyNotificationSettings): void {
        this.formConfig.form.controls.frequency.setValue(model.notificationFrequency);
        this.formConfig.form.controls.notificationEmail.setValue(model.notificationEmail);
    }
}
