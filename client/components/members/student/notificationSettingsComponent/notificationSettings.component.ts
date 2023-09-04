import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { StudentNotificationSettings } from '../../../../models/student/notificationSettings.model';
import { StudentUpdateModel } from '../../../../models/student/update.model';
import { StudentService } from '../../../../services/student.service';
import { BaseForm } from '../../baseForm/baseForm';


@Component({
    selector: 'jobchin-student-notificationSettings',
    templateUrl: 'notificationSettings.component.html',
})
export class StudentNotificationSettingsComponent extends BaseForm<StudentNotificationSettings, StudentUpdateModel> {
    jobchinSettings = jobchinSettings;

    constructor(private profile: JobChINProfileConfig, private studentService: StudentService) {
        super();

        this.model = this.profile.student.model.notificationSettings;
        this.onCallServer = (model) => {
            let dto = new StudentUpdateModel();
            dto.notificationSettings = model;
            return this.studentService.updateStudent(dto);
        };
    }

    initForm(): FormGroup {
        return new FormGroup({
            notificationFrequency: new FormControl(null),
            notificationEmail: new FormControl(null, Validators.maxLength(jobchinSettings.notificationEmailMaxLength)),
            workPositionNotificationFrequency: new FormControl(null),
        });
    }

    mapModel(form: FormGroup): StudentNotificationSettings {
        return {
            notificationFrequency: form.controls.notificationFrequency.value,
            notificationEmail: form.controls.notificationEmail.value,
            workPositionNotificationFrequency: form.controls.workPositionNotificationFrequency.value,
        };
    }

    initModel(model: StudentNotificationSettings): void {
        this.formConfig.form.controls.notificationFrequency.setValue(model.notificationFrequency);
        this.formConfig.form.controls.notificationEmail.setValue(model.notificationEmail);
        this.formConfig.form.controls.workPositionNotificationFrequency.setValue(model.workPositionNotificationFrequency);
    }
}
