import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { TinyMceModuleType } from '../../../../../../../core/services/tinymce-config.service';
import { TinyMceConfigService } from '../../../../../../_templates/muni-web/tinymce/services/tinymce-config.service';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { PresentationModel } from '../../../../models/company/presentation.model';
import { CompanyUpdateModel } from '../../../../models/company/update.model';
import { CompanyService } from '../../../../services/company.service';
import { BaseForm } from '../../baseForm/baseForm';


@Component({
    selector: 'jobchin-company-presentation',
    templateUrl: 'presentation.component.html',
    styleUrls: [ '../../../../assets/socialMedia.scss' ],
})
export class PresentationComponent extends BaseForm<PresentationModel, CompanyUpdateModel> {
    jobchinSettings = jobchinSettings;
    tinyConfig: Record<string, any>;

    constructor(private profile: JobChINProfileConfig, private companyService: CompanyService, private tinyMceConfig: TinyMceConfigService) {
        super();

        this.tinyConfig = this.tinyMceConfig.get(TinyMceModuleType.all);
        if (this.standAlone && this.profile.company) {
            this.model = this.profile.company.model.presentation;
            this.onCallServer = (model) => {
                let dto = new CompanyUpdateModel();
                dto.presentation = model;
                return this.companyService.updateCompany(dto);
            };
        }
    }

    initForm(): FormGroup {
        return new FormGroup({
            logo: new FormControl([]),
            backgroundImage: new FormControl([]),
            shortDescription: new FormControl(null, Validators.maxLength(jobchinSettings.descMaxLength)),
            description: new FormControl(null, Validators.maxLength(jobchinSettings.rteMaxLength)),
            differences: new FormControl(null, Validators.maxLength(jobchinSettings.rteMaxLength)),
            interviewDescription: new FormControl(null, Validators.maxLength(jobchinSettings.rteMaxLength)),
            web: new FormControl(null, Validators.maxLength(jobchinSettings.socialMediaMaxLength)),
            linkedin: new FormControl(null, Validators.maxLength(jobchinSettings.socialMediaMaxLength)),
            facebook: new FormControl(null, Validators.maxLength(jobchinSettings.socialMediaMaxLength)),
        });
    }

    mapModel(form: FormGroup): PresentationModel {
        return {
            logo : form.controls.logo.value.find((_, index) => index === 0),
            backgroundImage : form.controls.backgroundImage.value.find((_, index) => index === 0),
            shortDescription: form.controls.shortDescription.value,
            description: form.controls.description.value,
            differences: form.controls.differences.value,
            interviewDescription: form.controls.interviewDescription.value,
            web: form.controls.web.value,
            linkedin: form.controls.linkedin.value,
            facebook: form.controls.facebook.value,
        };
    }

    initModel(model: PresentationModel): void {
        this.formConfig.form.controls.logo.setValue(model.logo == null ? [] : [model.logo]);
        this.formConfig.form.controls.backgroundImage.setValue(model.backgroundImage == null ? [] : [model.backgroundImage]);
        this.formConfig.form.controls.shortDescription.setValue(model.shortDescription);
        this.formConfig.form.controls.description.setValue(model.description);
        this.formConfig.form.controls.differences.setValue(model.differences);
        this.formConfig.form.controls.interviewDescription.setValue(model.interviewDescription);
        this.formConfig.form.controls.web.setValue(model.web);
        this.formConfig.form.controls.linkedin.setValue(model.linkedin);
        this.formConfig.form.controls.facebook.setValue(model.facebook);
    }
}
