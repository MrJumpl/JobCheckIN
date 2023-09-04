import { Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';

import { TinyMceModuleType } from '../../../../../../../core/services/tinymce-config.service';
import { MuniWebButtonSize } from '../../../../../../_templates/muni-web/models/button-size';
import { TinyMceConfigService } from '../../../../../../_templates/muni-web/tinymce/services/tinymce-config.service';
import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { StudentUpdateModel } from '../../../../models/student/update.model';
import { WorkExperinceModel } from '../../../../models/student/workExperience.model';
import { WorkExperincesModel } from '../../../../models/student/workExperiences.model';
import { StudentService } from '../../../../services/student.service';
import { StudentBaseForm } from '../studentBaseForm';

@Component({
    selector: 'jobchin-student-workExperiences',
    templateUrl: 'workExperiences.component.html',
    styleUrls: [ '../../../../assets/error.scss' ],
})
export class WorkExperiencesComponent extends StudentBaseForm<WorkExperincesModel> {
    MuniWebButtonSize = MuniWebButtonSize;
    jobchinSettings = jobchinSettings;

    tinyConfig: Record<string, any>;
    show: boolean;

    workExperiences = this.formConfig.form?.controls?.workExperiences as FormArray;
    formDescriptions = this.profile?.student.formDescriptions;

    get propertyName(): string {
        return 'workExperiences';
    }

    constructor(protected override studentService: StudentService, private config: JobChINAngularConfig, protected override profile: JobChINProfileConfig, private tinyMceConfig: TinyMceConfigService) {
        super(studentService, profile);

        this.tinyConfig = this.tinyMceConfig.get(TinyMceModuleType.default);
        this.formConfig.form.controls.hasWorkExperinces.valueChanges.subscribe((x: string) => this.onWorkExperiencesShow(x));
    }

    initForm(): FormGroup {
        return new FormGroup({
            hasWorkExperinces: new FormControl(false.toString()),
            workExperiences: new FormArray([]),
            careerPortfolio: new FormControl(null, Validators.maxLength(jobchinSettings.rteMaxLength)),
        });
    }

    mapModel(form: FormGroup): WorkExperincesModel {
        let experiences = form.controls.workExperiences as FormArray;
        return ({
            experiences: form.controls.hasWorkExperinces.value
                ?  experiences.controls.map((x: FormGroup) => this.mapWorkExperience(x))
                : [],
            careerPortfolio: form.controls.careerPortfolio.value,
        });
    }

    initModel(model: WorkExperincesModel): void {
        setTimeout(() => {
            this.removeAllExperiences();
            const hasExperiences = model.experiences?.length > 0;
            model.experiences?.forEach(element => {
                this.addWorkExperience(element);
            });
            this.formConfig.form.controls.hasWorkExperinces.setValue(hasExperiences.toString());
            this.formConfig.form.controls.careerPortfolio.setValue(model.careerPortfolio);
        });
    }

    override onServerCallback(result: StudentUpdateModel, _: WorkExperincesModel): void {
        this.formConfig.form.reset();
        this.profile.student.model.workExperiences = result.workExperiences;
        this.model = this.profile.student.model.workExperiences;
    }

    addWorkExperience(workExperience: WorkExperinceModel = undefined): void {
        const hasContact = workExperience ? (workExperience?.contactPerson != null) : true;
        const stillWorking = workExperience ? (workExperience?.to ? false : true) : false;
        let group = new FormGroup({
            workExperienceId: new FormControl(workExperience?.workExperienceId ?? 0),
            position: new FormControl(workExperience?.position, [Validators.maxLength(jobchinSettings.workExperienceMaxLength), Validators.required]),
            companyName: new FormControl(workExperience?.companyName, [Validators.maxLength(jobchinSettings.workExperienceMaxLength), Validators.required]),
            city: new FormControl(workExperience?.city, [Validators.maxLength(jobchinSettings.studentCityMaxLength), Validators.required]),
            areaOfInterest: new FormControl(workExperience?.areaOfInterestId, Validators.required),
            from: new FormControl(workExperience?.from, Validators.required),
            to: new FormControl(workExperience?.to, stillWorking ? [] : Validators.required),
            stillWorking: new FormControl(stillWorking),
            paid: new FormControl(workExperience?.paid || false),
            contractType: new FormControl(workExperience?.contractTypeId, Validators.required),
            hasContact: new FormControl(hasContact.toString()),
            contactPerson: new FormControl(workExperience?.contactPerson, Validators.maxLength(jobchinSettings.workExperienceMaxLength)),
            contact: new FormControl(workExperience?.contact, Validators.maxLength(jobchinSettings.workExperienceMaxLength)),
            description: new FormControl(workExperience?.description, Validators.maxLength(jobchinSettings.shortRteMaxLength)),
        });
        if (hasContact) {
            group.controls.contactPerson.setValidators(Validators.required);
            group.controls.contact.setValidators(Validators.required);
        }
        group.controls.hasContact.valueChanges.subscribe(x => this.onContactChange(x === 'true', group));
        group.controls.stillWorking.valueChanges.subscribe(x => this.onStillWorkingChange(x, group));
        this.workExperiences.push(group);
    }

    removeWorkExperience(index: number): void {
        this.workExperiences.removeAt(index);
    }

    showTo(workExperience: FormGroup): boolean {
        return !workExperience.controls.stillWorking.value;
    }

    showContact(workExperience: FormGroup): boolean {
        return workExperience.controls.hasContact.value === 'true';
    }

    getAreaOfInterests() {
        return this.config.areaOfInterests;
    }

    getContractTypes() {
        return this.config.contractTypes;
    }

    getWorkExperienceTitle(workExperience: FormGroup): string {
        const position = workExperience.controls.position.value || '';
        const companyName = workExperience.controls.companyName.value || '';
        if (!position && !companyName) {
            return '';
        }
        return position
            + ((position && companyName) ? ' - ' : '')
            + companyName;
    }

    isAccordionOpen(workExperience: FormGroup): boolean {
        return workExperience.controls.workExperienceId.value === 0;
    }

    private onWorkExperiencesShow(show: string) {
        this.show = show === 'true';
        if (this.show && this.workExperiences.length === 0) {
            this.workExperiences.addValidators([Validators.minLength(1), Validators.required]);
            this.workExperiences.updateValueAndValidity();
            this.addWorkExperience();
        }
        if (!this.show) {
            this.workExperiences.clearValidators();
            this.workExperiences.updateValueAndValidity();
            this.removeAllExperiences();
        }
    }

    private onStillWorkingChange(stillWorking: boolean, group: FormGroup) {
        if (stillWorking) {
            group.controls.to.clearValidators();
            group.controls.to.updateValueAndValidity();
        } else {
            group.controls.to.setValidators(Validators.required);
            group.controls.to.updateValueAndValidity();
        }
    }

    private onContactChange(hasContact: boolean, group: FormGroup) {
        if (hasContact) {
            group.controls.contactPerson.setValidators(Validators.required);
            group.controls.contactPerson.updateValueAndValidity();
            group.controls.contact.setValidators(Validators.required);
            group.controls.contact.updateValueAndValidity();
        } else {
            group.controls.contactPerson.removeValidators(Validators.required);
            group.controls.contactPerson.updateValueAndValidity();
            group.controls.contact.removeValidators(Validators.required);
            group.controls.contact.updateValueAndValidity();
        }
    }

    private mapWorkExperience(group: FormGroup): WorkExperinceModel {
        const hasContact = group.controls.hasContact.value === 'true';
        return ({
            workExperienceId: group.controls.workExperienceId.value,
            position: group.controls.position.value,
            companyName: group.controls.companyName.value,
            city: group.controls.city.value,
            areaOfInterestId: group.controls.areaOfInterest.value,
            from: group.controls.from.value,
            to: group.controls.stillWorking.value ? null : group.controls.to.value,
            paid: group.controls.paid.value,
            contractTypeId: group.controls.contractType.value,
            contactPerson: hasContact ? group.controls.contactPerson.value : null,
            contact: hasContact ? group.controls.contact.value : null,
            description: group.controls.description.value,
        });
    }

    private removeAllExperiences() {
        while (this.workExperiences.length !== 0) {
            this.workExperiences.removeAt(0)
        }
    }
}
