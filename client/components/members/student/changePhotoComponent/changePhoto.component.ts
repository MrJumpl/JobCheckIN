import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { PhotoModel } from '../../../../models/student/photo.model';
import { StudentService } from '../../../../services/student.service';
import { StudentBaseForm } from '../studentBaseForm';

@Component({
    selector: 'jobchin-student-changePhoto',
    templateUrl: 'changePhoto.component.html',
})
export class ChangePhotoComponent extends StudentBaseForm<PhotoModel>  {
    jobchinSettings = jobchinSettings;

    get propertyName(): string {
        return 'photo';
    }

    constructor(protected override studentService: StudentService, protected override profile: JobChINProfileConfig) {
        super(studentService, profile);
    }

    initForm(): FormGroup {
        let formGroup = new FormGroup({
            photo: new FormControl([]),
        });
        return formGroup;
    }

    mapModel(form: FormGroup): PhotoModel {
        return ({
            photo: form.controls.photo.value?.find((_, index) => index === 0),
        })
    }

    initModel(model: PhotoModel): void {
        this.formConfig.form.controls.photo.setValue(model.photo == null ? [] : [model.photo]);
    }
}
