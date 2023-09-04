import { Component, HostListener } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';

import { TinyMceModuleType } from '../../../../../../../core/services/tinymce-config.service';
import { MuniWebButtonSize } from '../../../../../../_templates/muni-web/models/button-size';
import { TinyMceConfigService } from '../../../../../../_templates/muni-web/tinymce/services/tinymce-config.service';
import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { Language } from '../../../../models/language';
import { OtherStudyModel } from '../../../../models/student/otherStudy.model';
import { StudiesModel } from '../../../../models/student/studies.model';
import { StudyDto } from '../../../../models/student/study.dto.model';
import { StudentUpdateModel } from '../../../../models/student/update.model';
import { StudentService } from '../../../../services/student.service';
import { StudentBaseForm } from '../studentBaseForm';


@Component({
    selector: 'jobchin-student-study',
    templateUrl: 'study.component.html',
    styleUrls: [ '../../../../assets/accordion.scss', '../../../../assets/error.scss' ],
})
export class StudyComponent extends StudentBaseForm<StudiesModel> {
    MuniWebButtonSize = MuniWebButtonSize;
    jobchinSettings = jobchinSettings;

    tinyConfig: Record<string, any>;
    width: number;
    initiallyOpen: false;

    formDescriptions = this.profile?.student.formDescriptions;

    @HostListener('window:resize', ['$event'])
    onResize(event) {
        this.width = window.innerWidth;
    }

    get propertyName(): string {
        return 'studies';
    }

    constructor(protected override studentService: StudentService, private config: JobChINAngularConfig, protected override profile: JobChINProfileConfig, private tinyMceConfig: TinyMceConfigService) {
        super(studentService, profile);

        this.tinyConfig = this.tinyMceConfig.get(TinyMceModuleType.default);
    }

    override ngOnInit(): void {
        super.ngOnInit();

        this.width = window.innerWidth;
    }

    // studies = this.profile.student.muniStudies;
    otherStudies = this.formConfig.form?.controls?.otherStudies as FormArray;

    initForm(): FormGroup {
        return new FormGroup({
            otherStudies: new FormArray([]),
            additionalEducation: new FormControl(null, Validators.maxLength(jobchinSettings.rteMaxLength)),
        });
    }

    mapModel(form: FormGroup): StudiesModel {
        let studies = form.controls.otherStudies as FormArray;
        let models = studies.controls.map((x: FormGroup) => {
            return {
                otherUniversityStudyId: x.controls.otherUniversityStudyId.value,
                university: x.controls.university.value,
                faculty: x.controls.faculty.value,
                specialization: x.controls.specialization.value,
                city: x.controls.city.value,
                countryId: x.controls.countryId.value,
                from: x.controls.from.value,
                to: x.controls.stillStudy.value ? null : x.controls.to.value,
                languageId: x.controls.languageId.value,
            };
        });
        return {
            others: models,
            additionalEducation: form.controls.additionalEducation.value,
        };
    }

    initModel(model: StudiesModel): void {
        while (this.otherStudies.length !== 0) {
            this.otherStudies.removeAt(0)
        }
        model.others?.forEach(element => {
            this.addStudy(element);
        });
        this.formConfig.form.controls.additionalEducation.setValue(model.additionalEducation);
    }

    override onServerCallback(result: StudentUpdateModel, _: StudiesModel): void {
        this.formConfig.form.reset();
        this.profile.student.model.studies.others = result.studies.others;
        this.model = this.profile.student.model.studies;
    }

    addStudy(otherStudy: OtherStudyModel = undefined): void {
        const stillStudy = otherStudy ? (otherStudy?.to ? false : true) : false;
        let group = new FormGroup({
            otherUniversityStudyId: new FormControl(otherStudy?.otherUniversityStudyId ?? 0),
            university: new FormControl(otherStudy?.university, [Validators.maxLength(jobchinSettings.otherStudyMaxLength), Validators.required]),
            faculty: new FormControl(otherStudy?.faculty, Validators.maxLength(jobchinSettings.otherStudyMaxLength)),
            specialization: new FormControl(otherStudy?.specialization, [Validators.maxLength(jobchinSettings.otherStudyMaxLength), Validators.required]),
            city: new FormControl(otherStudy?.city, [Validators.maxLength(jobchinSettings.studentCityMaxLength), Validators.required]),
            countryId: new FormControl(otherStudy?.countryId, Validators.required),
            from: new FormControl(otherStudy?.from, Validators.required),
            to: new FormControl(otherStudy?.to, stillStudy ? [] : Validators.required),
            stillStudy: new FormControl(stillStudy),
            languageId: new FormControl(otherStudy?.languageId),
        });
        group.controls.stillStudy.valueChanges.subscribe(x => this.onStillStudyChange(x, group));
        this.otherStudies.push(group);
    }

    createOpen(otherStudy: FormGroup): boolean {
        return otherStudy.controls.otherUniversityStudyId.value === 0;
    }

    removeStudy(index: number): void {
        this.otherStudies.removeAt(index);
    }

    getStudyFields(study: StudyDto): string {
        return study.fields.join(', ');
    }

    getOtherStudyTitle(otherStudy: FormGroup): string {
        let university = otherStudy.controls.university.value;
        let specialization = otherStudy.controls.specialization.value
        if (!university && !specialization) {
            return '';
        }
        return (university ?? '')
            + (university && specialization ? ' - ' : '')
            + (specialization ?? '');
    }

    showTo(otherStudy: FormGroup): boolean {
        return !otherStudy.controls.stillStudy.value;
    }

    getFirstRowOrder(): number[] {
        return this.width >= 1024 ? [1, 2] : [2, 1];
    }

    getLanguages(): Language[] {
        return this.config.languages;
    }

    private onStillStudyChange(stillStudy: boolean, group: FormGroup) {
        if (stillStudy) {
            group.controls.to.clearValidators();
            group.controls.to.updateValueAndValidity();
        } else {
            group.controls.to.setValidators(Validators.required);
            group.controls.to.updateValueAndValidity();
        }
    }
}
