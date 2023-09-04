import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { CandidateRequestModel } from '../../../../models/workPosition/candidateRequest.model';
import { CompanyService } from '../../../../services/company.service';
import { WorkPositionBaseForm } from '../workPositionBaseForm';

@Component({
    selector: 'jobchin-workPosition-candidateRequest',
    templateUrl: 'candidateRequest.component.html',
})
export class WorkPositionCandidateRequestComponent extends WorkPositionBaseForm<CandidateRequestModel> {
    jobchinSettings = jobchinSettings;

    showAdditionalQuestions: boolean;

    get propertyName(): string {
        return 'candidateRequest';
    }

    constructor(protected override readonly route: ActivatedRoute, protected override readonly companyService: CompanyService, protected override readonly profile: JobChINProfileConfig) {
        super(route, companyService, profile);
    }

    initForm(): FormGroup {
        this.showAdditionalQuestions = false;
        let form = new FormGroup({
            coverLetter: new FormControl(false),
            hasAdditionalQuestions: new FormControl(false),
            additionalQuestions: new FormControl(null, Validators.maxLength(jobchinSettings.shortRteMaxLength)),
        });
        form.controls.hasAdditionalQuestions.valueChanges.subscribe(x => this.onHasAdditionalQuestionsChange(x))
        return form;
    }

    mapModel(form: FormGroup): CandidateRequestModel {
        return {
            coverLetter: form.controls.coverLetter.value,
            additionalQuestions: form.controls.hasAdditionalQuestions.value ? form.controls.additionalQuestions.value : null,
        }
    }

    initModel(model: CandidateRequestModel): void {
        this.formConfig.form.controls.coverLetter.setValue(model.coverLetter);
        this.formConfig.form.controls.hasAdditionalQuestions.setValue(model.additionalQuestions != null);
        this.formConfig.form.controls.additionalQuestions.setValue(model.additionalQuestions);
    }

    private onHasAdditionalQuestionsChange(val: boolean): void {
        this.showAdditionalQuestions = val;
        if (val) {
            this.formConfig.form.controls.additionalQuestions.addValidators(Validators.required);
        } else {
            this.formConfig.form.controls.additionalQuestions.removeValidators(Validators.required);
        }
        this.formConfig.form.controls.additionalQuestions.updateValueAndValidity();
    }
}
