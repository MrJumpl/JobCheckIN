import { Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { MuniWebButtonSize } from '../../../../../../_templates/muni-web/models/button-size';
import { TinyMceConfigService } from '../../../../../../_templates/muni-web/tinymce/services/tinymce-config.service';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { DetailModel } from '../../../../models/workPosition/detail.model';
import { CompanyService } from '../../../../services/company.service';
import { WorkPositionBaseForm } from '../workPositionBaseForm';

@Component({
    selector: 'jobchin-workPosition-detail',
    templateUrl: 'detail.component.html',
    styleUrls: [ '../../../../assets/error.scss' ],
})
export class WorkPositionDetailComponent extends WorkPositionBaseForm<DetailModel> {
    MuniWebButtonSize = MuniWebButtonSize;
    jobchinSettings = jobchinSettings;

    tinyConfig: Record<string, any>;

    get propertyName(): string {
        return 'detail';
    }

    constructor(protected override readonly route: ActivatedRoute, protected override readonly companyService: CompanyService, protected override readonly profile: JobChINProfileConfig, private tinyMceConfig: TinyMceConfigService) {
        super(route, companyService, profile);

        this.tinyConfig = this.tinyMceConfig.get();
    }

    override ngOnInit() {
        super.ngOnInit();

        if (!this.standAlone) {
            this.formConfig.form.controls.companyDescription.setValue(this.profile.company.model.presentation.shortDescription);
        }
    }

    customFields = this.formConfig.form?.controls?.customFields as FormArray;

    initForm(): FormGroup {
        return new FormGroup({
            companyDescription: new FormControl(null, Validators.maxLength(jobchinSettings.descMaxLength)),
            description: new FormControl(null, [Validators.maxLength(jobchinSettings.rteMaxLength), Validators.required]),
            requesting: new FormControl(null, [Validators.maxLength(jobchinSettings.rteMaxLength), Validators.required]),
            offering: new FormControl(null, Validators.maxLength(jobchinSettings.rteMaxLength)),
            customFields: new FormArray([]),
        });
    }

    mapModel(form: FormGroup): DetailModel {
        let customFields = (form.controls.customFields as FormArray).controls as FormGroup[];
        return {
            companyDescription: form.controls.companyDescription.value,
            description: form.controls.description.value,
            requesting: form.controls.requesting.value,
            offering: form.controls.offering.value,
            customField1Name: customFields[0]?.controls.name.value,
            customField2Name: customFields[1]?.controls.name.value,
            customField3Name: customFields[2]?.controls.name.value,
            customField1Value: customFields[0]?.controls.value.value,
            customField2Value: customFields[1]?.controls.value.value,
            customField3Value: customFields[2]?.controls.value.value,
        }
    }

    initModel(model: DetailModel): void {
        this.formConfig.form.controls.companyDescription.setValue(model.companyDescription);
        this.formConfig.form.controls.description.setValue(model.description);
        this.formConfig.form.controls.requesting.setValue(model.requesting);
        this.formConfig.form.controls.offering.setValue(model.offering);

        while (this.customFields.length !== 0) {
            this.customFields.removeAt(0)
        }
        if (model.customField1Name) {
            this.addField(model.customField1Name, model.customField1Value);
        }
        if (model.customField2Name) {
            this.addField(model.customField2Name, model.customField2Value);
        }
        if (model.customField3Name) {
            this.addField(model.customField3Name, model.customField3Value);
        }
    }

    addField(name = undefined, value = undefined) {
        let group = new FormGroup({
            name: new FormControl(name, [Validators.maxLength(jobchinSettings.customFieldNameMaxLength), Validators.required]),
            value: new FormControl(value, [Validators.maxLength(jobchinSettings.rteMaxLength), Validators.required]),
        });
        this.customFields.push(group);
    }

    removeField(index: number) {
        this.customFields.removeAt(index);
    }

    getTitle(customField: FormGroup): string {
        return customField.controls.name.value;
    }
}
