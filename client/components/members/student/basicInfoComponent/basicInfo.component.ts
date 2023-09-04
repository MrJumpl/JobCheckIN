import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { startOfDay } from 'date-fns/esm';

import { TinyMceModuleType } from '../../../../../../../core/services/tinymce-config.service';
import { TinyMceConfigService } from '../../../../../../_templates/muni-web/tinymce/services/tinymce-config.service';
import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { BasicInfoModel } from '../../../../models/student/basicInfo.model';
import { StudentService } from '../../../../services/student.service';
import { StudentBaseForm } from '../studentBaseForm';

@Component({
    selector: 'jobchin-student-basicInfo',
    templateUrl: 'basicInfo.component.html',
})
export class BasicInfoComponent extends StudentBaseForm<BasicInfoModel>  {
    jobchinSettings = jobchinSettings;

    tinyConfig: Record<string, any>;
    hidePreferedLocationId: boolean;
    hideWorkFrom: boolean;
    secondaryAreaOfInterest = [];

    formDescriptions = this.profile?.student.formDescriptions;

    get propertyName(): string {
        return 'basicInfo';
    }

    constructor(private config: JobChINAngularConfig, protected override studentService: StudentService, protected override profile: JobChINProfileConfig, private tinyMceConfig: TinyMceConfigService) {
        super(studentService, profile);

        this.tinyConfig = this.tinyMceConfig.get(TinyMceModuleType.all);
        this.formConfig.form.controls.willingToMove.valueChanges.subscribe((x: boolean) => this.hidePreferedLocationId = x);
        this.formConfig.form.controls.workFromNow.valueChanges.subscribe((x: boolean) => this.hideWorkFrom = x);
        this.secondaryAreaOfInterest = this.config.areaOfInterests;
    }

    initForm(): FormGroup {
        let formGroup = new FormGroup({
            activeDriver: new FormControl(false),
            drivingLicense: new FormControl(false),
            careerVision: new FormControl(null, Validators.maxLength(jobchinSettings.rteMaxLength)),
            presentation: new FormControl(null, Validators.maxLength(jobchinSettings.rteMaxLength)),
            contractTypes: new FormControl([]),
            areaOfInterests: new FormControl([], Validators.maxLength(3)),
            secondaryAreaOfInterests: new FormControl([]),
            preferedLocationId: new FormControl(),
            willingToMove: new FormControl(false),
            preferredJobBeginning: new FormControl(),
            workFromNow: new FormControl(false),
        });
        formGroup.controls.areaOfInterests.valueChanges.subscribe(x => this.onPrimaryChange(x))
        return formGroup;
    }

    mapModel(form: FormGroup): BasicInfoModel {
        let preferredJobBeginning = null;
        if (form.controls.workFromNow.value) {
            preferredJobBeginning = this.getMinDate();
        } else if (form.controls.preferredJobBeginning.value) {
            preferredJobBeginning = startOfDay(form.controls.preferredJobBeginning.value)
        }
        return ({
            activeDriver: form.controls.activeDriver.value,
            drivingLicense: form.controls.drivingLicense.value,
            careerVision: form.controls.careerVision.value,
            presentation: form.controls.presentation.value,
            contractTypes: form.controls.contractTypes.value,
            areaOfInterests: form.controls.areaOfInterests.value,
            secondaryAreaOfInterests: form.controls.secondaryAreaOfInterests.value,
            preferedLocationId: form.controls.preferedLocationId.value,
            willingToMove: form.controls.willingToMove.value,
            preferredJobBeginning: preferredJobBeginning,
        })
    }

    initModel(model: BasicInfoModel): void {
        let workFromNow = model.preferredJobBeginning != null && model.preferredJobBeginning <= new Date();
        this.formConfig.form.controls.activeDriver.setValue(model.activeDriver);
        this.formConfig.form.controls.drivingLicense.setValue(model.drivingLicense);
        this.formConfig.form.controls.careerVision.setValue(model.careerVision);
        this.formConfig.form.controls.presentation.setValue(model.presentation);
        this.formConfig.form.controls.contractTypes.setValue(model.contractTypes);
        this.formConfig.form.controls.areaOfInterests.setValue(model.areaOfInterests);
        this.formConfig.form.controls.secondaryAreaOfInterests.setValue(model.secondaryAreaOfInterests);
        this.formConfig.form.controls.preferedLocationId.setValue(model.preferedLocationId);
        this.formConfig.form.controls.willingToMove.setValue(model.willingToMove);
        this.formConfig.form.controls.preferredJobBeginning.setValue(model.preferredJobBeginning);
        this.formConfig.form.controls.workFromNow.setValue(workFromNow);
    }

    getAreaOfInterests() {
        return this.config.areaOfInterests;
    }

    getContractTypes() {
        return this.config.contractTypes;
    }

    getLocalAdministrativeUnits() {
        return this.config.localAdministrativeUnits;
    }

    getMinDate() {
        return startOfDay(new Date());
    }

    private onPrimaryChange(values) {
        if (!values) {
            return;
        }
        this.secondaryAreaOfInterest = this.config.areaOfInterests.filter(x => !values.includes(x.areaOfInterestId));
        let oldValue = this.formConfig.form.controls.secondaryAreaOfInterests.value || [];
        let newValue = oldValue.filter(x => !values.includes(x)) || [];
        if (oldValue.length !== newValue.length) {
            this.formConfig.form.controls.secondaryAreaOfInterests.setValue(newValue);
        }
    }
}
