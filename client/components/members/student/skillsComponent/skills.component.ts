import { Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';

import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { SkillsModel } from '../../../../models/student/skills.model';
import { StudentService } from '../../../../services/student.service';
import { StudentBaseForm } from '../studentBaseForm';

@Component({
    selector: 'jobchin-student-skills',
    templateUrl: 'skills.component.html',
})
export class SkillsComponent extends StudentBaseForm<SkillsModel> {
    jobchinSettings = jobchinSettings;
    maxPrimarySoftSkills = 3;
    possiblePrimarySoftSkills = [];

    get propertyName(): string {
        return 'skills';
    }

    constructor(protected override studentService: StudentService, private config: JobChINAngularConfig, protected override profile: JobChINProfileConfig) {
        super(studentService, profile);

        this.initSoftSkills();
    }

    softSkills = this.formConfig.form?.controls?.softSkills as FormArray;
    formDescriptions = this.profile?.student.formDescriptions;

    initForm(): FormGroup {
        return new FormGroup({
            hardSkills: new FormControl([]),
            educationCertificate: new FormControl([]),
            softSkills: new FormArray([]),
            primarySoftSkills: new FormControl([], Validators.maxLength(this.maxPrimarySoftSkills)),
            languages: new FormControl(null),
            languageCertificate: new FormControl([]),
        });
    }

    mapModel(form: FormGroup): SkillsModel {
        const primarySoftSkills = form.controls.primarySoftSkills.value;
        return {
            hardSkills: form.controls.hardSkills.value,
            educationCertificate: form.controls.educationCertificate.value?.find((_, index) => index === 0),
            softSkills: form.controls.softSkills.value.filter(x => x.selected).map(x => {
                return {
                    softSkillId: x.softSkillId,
                    isPrimary: primarySoftSkills?.findIndex((y: number) => y === x.softSkillId) >= 0,
                    description: x.description,
                };
            }),
            languages: form.controls.languages.value,
            languageCertificate: form.controls.languageCertificate.value?.find((_, index) => index === 0),
        };
    }

    initModel(model: SkillsModel): void {
        this.formConfig.form.controls.hardSkills.setValue(model.hardSkills);
        this.formConfig.form.controls.educationCertificate.setValue(model.educationCertificate == null ? [] : [model.educationCertificate]);

        let primarySoftSkills = [];
        this.possiblePrimarySoftSkills = [];
        this.softSkills.controls.forEach((formGroup: FormGroup) => {
            let softSkillId = formGroup.controls.softSkillId.value;
            let softSkill = model.softSkills?.find(x => x.softSkillId === softSkillId);
            if (softSkill != null) {
                formGroup.controls.selected.setValue(true);
                formGroup.controls.description.setValue(softSkill.description);
                if (softSkill.isPrimary === true) {
                    primarySoftSkills.push(softSkill.softSkillId);
                }
            } else {
                formGroup.controls.selected.setValue(false);
                formGroup.controls.description.setValue(null);
            }
        });
        this.formConfig.form.controls.primarySoftSkills.setValue(primarySoftSkills);
        this.formConfig.form.controls.languages.setValue(model.languages);
        this.formConfig.form.controls.languageCertificate.setValue(model.languageCertificate == null ? [] : [model.languageCertificate]);
    }

    initSoftSkills() {
        for (let softSkill of this.config.softSkills) {
            let group = new FormGroup({
                softSkillId: new FormControl(softSkill.softSkillId),
                selected: new FormControl(false),
                description: new FormControl(null, Validators.maxLength(jobchinSettings.shortRteMaxLength)),
            });
            group.controls.selected.valueChanges.subscribe(x => {
                if (x) {
                    group.controls.description.addValidators(Validators.required);
                    this.possiblePrimarySoftSkills.push(softSkill);
                    this.possiblePrimarySoftSkills = [...this.possiblePrimarySoftSkills];
                } else {
                    group.controls.description.removeValidators(Validators.required);
                    this.possiblePrimarySoftSkills = this.possiblePrimarySoftSkills.filter(possibleValue => possibleValue.softSkillId !== softSkill.softSkillId);
                    this.formConfig.form.controls.primarySoftSkills.setValue(this.formConfig.form.controls.primarySoftSkills.value.filter(val => val !== softSkill.softSkillId));
                }
            })
            this.softSkills.push(group);
        }
    }

    getPossiblePrimaySoftSkills() {
        return this.possiblePrimarySoftSkills;
    }

    getSoftSkillName(index: number): string {
        return this.config.softSkills[index].name;
    }

    showDescription(index: number): boolean {
        let group = this.softSkills.at(index) as FormGroup;
        return group.controls.selected.value === true;
    }
}
