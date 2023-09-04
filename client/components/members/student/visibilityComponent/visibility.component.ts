import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { VisibilityModel } from '../../../../models/student/visibility.model';
import { StudentService } from '../../../../services/student.service';
import { StudentBaseForm } from '../studentBaseForm';

@Component({
    selector: 'jobchin-student-visibility',
    templateUrl: 'visibility.component.html',
})
export class StudentVisibilityComponent extends StudentBaseForm<VisibilityModel>  {

    override get propertyName(): string {
        return 'visibility';
    }

    constructor(protected override profile: JobChINProfileConfig, protected override studentService: StudentService) {
        super(studentService, profile);
    }

    getVisibilityDescription(): string {
        return this.profile.student.formDescriptions?.visibility;
    }

    initForm(): FormGroup {
        let formGroup = new FormGroup({
            provideContact: new FormControl(false),
        });
        return formGroup;
    }

    mapModel(form: FormGroup): VisibilityModel {
        return ({
            provideContact: form.controls.provideContact.value,
        })
    }

    initModel(model: VisibilityModel): void {
        this.formConfig.form.controls.provideContact.setValue(model.provideContact);
    }
}
