import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { TinyMceModuleType } from '../../../../../../../core/services/tinymce-config.service';
import { TinyMceConfigService } from '../../../../../../_templates/muni-web/tinymce/services/tinymce-config.service';
import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { CandidatesModel } from '../../../../models/company/candidates.model';
import { CompanyUpdateModel } from '../../../../models/company/update.model';
import { CompanyService } from '../../../../services/company.service';
import { BaseForm } from '../../baseForm/baseForm';


@Component({
    selector: 'jobchin-company-candidates',
    templateUrl: 'candidates.component.html',
})
export class CandidatesComponent extends BaseForm<CandidatesModel, CompanyUpdateModel> {
    jobchinSettings = jobchinSettings;
    tinyConfig: Record<string, any>;


    constructor(private profile: JobChINProfileConfig, public config: JobChINAngularConfig, private companyService: CompanyService, private tinyMceConfig: TinyMceConfigService) {
        super();

        this.tinyConfig = this.tinyMceConfig.get(TinyMceModuleType.all);
        if (this.standAlone && this.profile.company) {
            this.model = this.profile.company.model.candidates;
            this.onCallServer = (model) => {
                let dto = new CompanyUpdateModel();
                dto.candidates = model;
                return this.companyService.updateCompany(dto);
            };
        }
    }

    initForm(): FormGroup {
        return new FormGroup({
            areaOfInterests: new FormControl(null),
            faculties: new FormControl(null),
            languageSkillPrefered: new FormControl(null),
            peopleTypesSought: new FormControl(null, Validators.maxLength(jobchinSettings.rteMaxLength)),
        });
    }

    mapModel(form: FormGroup): CandidatesModel {
        return {
            areaOfInterests: form.controls.areaOfInterests.value,
            faculties: form.controls.faculties.value,
            languageSkillPrefered: form.controls.languageSkillPrefered.value,
            peopleTypesSought: form.controls.peopleTypesSought.value,
        };
    }

    initModel(model: CandidatesModel): void {
        this.formConfig.form.controls.areaOfInterests.setValue(model.areaOfInterests);
        this.formConfig.form.controls.faculties.setValue(model.faculties);
        this.formConfig.form.controls.languageSkillPrefered.setValue(model.languageSkillPrefered);
        this.formConfig.form.controls.peopleTypesSought.setValue(model.peopleTypesSought);
    }
}
