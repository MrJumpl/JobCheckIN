import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { ContactModel } from '../../../../models/student/contact.model';
import { StudentService } from '../../../../services/student.service';
import { StudentBaseForm } from '../studentBaseForm';

@Component({
    selector: 'jobchin-student-contact',
    templateUrl: 'contact.component.html',
    styleUrls: [ '../../../../assets/socialMedia.scss' ],
})
export class ContactComponent extends StudentBaseForm<ContactModel> {
    jobchinSettings = jobchinSettings;

    get propertyName(): string {
        return 'contact';
    }

    constructor(protected override studentService: StudentService, protected override profile: JobChINProfileConfig) {
        super(studentService, profile);
    }

    initForm(): FormGroup {
        return new FormGroup({
            privateEmail: new FormControl(null, Validators.maxLength(jobchinSettings.notificationEmailMaxLength)),
            phone: new FormControl(null, Validators.maxLength(jobchinSettings.contactMaxLength)),
            linkedin: new FormControl(null, Validators.maxLength(jobchinSettings.socialMediaMaxLength)),
            facebook: new FormControl(null, Validators.maxLength(jobchinSettings.socialMediaMaxLength)),
            twitter: new FormControl(null, Validators.maxLength(jobchinSettings.socialMediaMaxLength)),
        });
    }

    mapModel(form: FormGroup): ContactModel {
        return ({
            privateEmail: form.controls.privateEmail.value ? form.controls.privateEmail.value : undefined,
            phone: form.controls.phone.value,
            linkedin: form.controls.linkedin.value,
            facebook: form.controls.facebook.value,
            twitter: form.controls.twitter.value,
        })
    }

    initModel(model: ContactModel): void {
        this.formConfig.form.controls.privateEmail.setValue(model.privateEmail);
        this.formConfig.form.controls.phone.setValue(model.phone);
        this.formConfig.form.controls.linkedin.setValue(model.linkedin);
        this.formConfig.form.controls.facebook.setValue(model.facebook);
        this.formConfig.form.controls.twitter.setValue(model.twitter);
    }
}
