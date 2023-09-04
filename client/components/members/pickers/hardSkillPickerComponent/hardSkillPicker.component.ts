import { ChangeDetectorRef, Component, forwardRef, Input, ViewChild } from '@angular/core';
import {
    ControlValueAccessor,
    FormControl,
    FormGroup,
    FormGroupDirective,
    NG_VALUE_ACCESSOR,
    Validators,
} from '@angular/forms';

import { MuniWebForm2Component } from '../../../../../../_templates/muni-web/components/form/form2.component';
import {
    MuniWebPopUpButtonComponent,
} from '../../../../../../_templates/muni-web/components/pop-up-button/pop-up-button.component';
import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINFormConfig } from '../../../../config/formConfig';
import { jobchinLocalize } from '../../../../jobchin.localize';
import { UserService } from '../../../../services/user.service';

@Component({
    selector: 'jobchin-hardSkillPicker',
    templateUrl: 'hardSkillPicker.component.html',
    styleUrls: [ './hardSkillPicker.component.scss' ],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => HardSkillPickerComponent),
            multi: true
        },
    ],
})
export class HardSkillPickerComponent implements ControlValueAccessor {
    fullText = [];
    areaOfInterest: any;
    hardSkills = [];

    formConfig: JobChINFormConfig<string, any>;

    @Input() label: string;

    _value: number[];
    get value(): number[] {
        return this._value;
    }

    @Input() set value(val: number[]) {
        this._value = val;

        this.onChange(val);
        this.onTouched();
    }

    @Input() disabled = false;

    @ViewChild(MuniWebPopUpButtonComponent) popUp: MuniWebPopUpButtonComponent;
    @ViewChild(MuniWebForm2Component) form: MuniWebForm2Component;

    onChange = (_: number[]) => { };
    onTouched = () => { };

    constructor(private parent: FormGroupDirective, private config: JobChINAngularConfig, private ref: ChangeDetectorRef, private userService: UserService) {
        this.hardSkills = JSON.parse(JSON.stringify(this.config.hardSkills));

        this.formConfig = {
            form: new FormGroup({
                name: new FormControl(null, [ Validators.required,  Validators.maxLength(64)])
            }),
            onGetFormModel: form => form.controls.name.value,
            onCallServer: (model) => this.userService.suggestHardSkill(model),
            onServerCallback: (result, model) => this.onHardSkillSuggested(),
            onGetResultMessage: (_) => jobchinLocalize.hardSkillSuggestSuccess,
            submitButtonLabel: jobchinLocalize.send,
        }
    }

    getHardSkills() {
        return this.config.hardSkills;
    }

    getAreaOfInterests() {
        return this.config.areaOfInterests;
    }

    getTitle(hardSkillId: number) {
        return this.config.hardSkills.find(x => x.hardSkillId === hardSkillId)?.name;
    }

    removeHardSkill(hardSkillId: number) {
        this.value = this.value.filter(x => x !== hardSkillId);
    }

    fullTextChange(val: number) {
        let hardSkill = this.hardSkills.find(x => x.hardSkillId === val);
        if (hardSkill) {
            hardSkill.disabled = true;
        }
        this.value = this.value.concat(val);
        this.fullText = [];
    }

    onAreaOfInterestChanged(val: number) {
        if (!val) {
            this.hardSkills = this.config.hardSkills;
        } else {
            this.hardSkills = this.config.hardSkills.filter(x => x.areaOfInterestId === val);
        }
        this.ref.detectChanges();
    }

    onHardSkillSuggested(): boolean {
        setTimeout(() => {
            this.popUp.closePopUp();
            this.formConfig.form.reset();
            this.form.reset();
        }, 2000);
        return false;
    }

    // Implementation of the ControlValueAccessor
    writeValue(obj: any): void {
        if (obj == null) {
            obj = [];
        }
        this._value = obj;
    }

    registerOnChange(fn: any): void {
        this.onChange = fn;
    }

    registerOnTouched(fn: any): void {
        this.onTouched = fn;
    }

    setDisabledState?(isDisabled: boolean): void {
        this.disabled = isDisabled;
    }

    getFormGroup(): FormGroup {
        return this.parent.control;
    }
}
