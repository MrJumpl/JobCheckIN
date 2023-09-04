import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { CandidatesModel } from '../../../../models/workPosition/candidates.model';
import { CompanyService } from '../../../../services/company.service';
import { WorkPositionBaseForm } from '../workPositionBaseForm';

@Component({
    selector: 'jobchin-workPosition-candidates',
    templateUrl: 'candidates.component.html',
})
export class WorkPositionCandidatesComponent extends WorkPositionBaseForm<CandidatesModel> {
    showActiveDriver = false;

    get propertyName(): string {
        return 'candidates';
    }

    constructor(public config: JobChINAngularConfig, protected override readonly route: ActivatedRoute, protected override readonly companyService: CompanyService, protected override readonly profile: JobChINProfileConfig) {
        super(route, companyService, profile);
    }

    initForm(): FormGroup {
        let form = new FormGroup({
            areaOfInterests: new FormControl([]),
            faculties: new FormControl([]),
            hardSkills: new FormControl([]),
            softSkills: new FormControl([]),
            languages: new FormControl([]),
            drivingLicense: new FormControl(false),
            activeDriver: new FormControl(false),
        });
        form.controls.drivingLicense.valueChanges.subscribe(x => this.onDrivingLicenseChange(x));
        return form;
    }

    mapModel(form: FormGroup): CandidatesModel {
        return {
            areaOfInterests: form.controls.areaOfInterests.value,
            faculties: form.controls.faculties.value,
            hardSkills: form.controls.hardSkills.value,
            softSkills: form.controls.softSkills.value,
            languages: form.controls.languages.value,
            drivingLicense: form.controls.drivingLicense.value,
            activeDriver: form.controls.drivingLicense.value && form.controls.activeDriver.value,
        }
    }

    initModel(model: CandidatesModel): void {
        this.formConfig.form.controls.areaOfInterests.setValue(model.areaOfInterests);
        this.formConfig.form.controls.faculties.setValue(model.faculties);
        this.formConfig.form.controls.hardSkills.setValue(model.hardSkills);
        this.formConfig.form.controls.softSkills.setValue(model.softSkills);
        this.formConfig.form.controls.languages.setValue(model.languages);
        this.formConfig.form.controls.drivingLicense.setValue(model.drivingLicense);
        this.formConfig.form.controls.activeDriver.setValue(model.drivingLicense && model.activeDriver);
    }

    onDrivingLicenseChange(val: boolean) {
        this.showActiveDriver = val;
    }
}
