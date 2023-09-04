import { Component } from '@angular/core';

import { JobChINProfileConfig } from '../../../config/profileConfig';


@Component({
    selector: 'jobchin-notifiaciton',
    templateUrl: 'notification.component.html',
})
export class NotificationComponent /*extends BaseForm<CandidatesModel>*/ {

    constructor(private profile: JobChINProfileConfig) {
        //super();
    }
/*
    initForm(): FormGroup {
        return new FormGroup({
            areaOfInterests: new FormControl(null),
            faculties: new FormControl(null),
            languageSkillPrefered: new FormControl(null),
            peopleTypesSought: new FormControl(null),
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
*/
}
