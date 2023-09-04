import { ChangeDetectorRef, Component, forwardRef, Input, ViewEncapsulation } from '@angular/core';
import { ControlValueAccessor, NG_VALIDATORS, NG_VALUE_ACCESSOR, Validator } from '@angular/forms';

import { EntyMenuState } from '../../../../../../members/models/entry-menu-state.enum';
import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { jobchinLocalize } from '../../../../jobchin.localize';
import { LanguageModel } from '../../../../models/language.model';
import { LanguageSkill } from '../../../../models/languageSkill.enum';

@Component({
    selector: 'jobchin-languagePicker',
    templateUrl: 'languagePicker.component.html',
    styleUrls: ['./languagePicker.component.scss' ],
    providers: [
        {
            provide: NG_VALUE_ACCESSOR,
            useExisting: forwardRef(() => LanguagePickerComponent),
            multi: true
        },
        {
            provide: NG_VALIDATORS,
            useExisting: LanguagePickerComponent,
            multi: true
        }
    ],
    encapsulation: ViewEncapsulation.None,
})
export class LanguagePickerComponent implements ControlValueAccessor, Validator {
    EntyMenuState = EntyMenuState;
    languageSkills = [
        { id: LanguageSkill.A1, name: 'A1' },
        { id: LanguageSkill.A2, name: 'A2' },
        { id: LanguageSkill.B1, name: 'B1' },
        { id: LanguageSkill.B2, name: 'B2' },
        { id: LanguageSkill.C1, name: 'C1' },
        { id: LanguageSkill.C2, name: 'C2' }
    ];

    optional = [
        { value: true, displayValue: jobchinLocalize.languagePickerOptional },
        { value: false, displayValue: jobchinLocalize.languagePickerMandatory }
    ];

    languages: any[] = [];

    _value: LanguageModel[];
    get value(): LanguageModel[] {
        return this._value.filter(x => x.languageId != null && x.skill != null);
    }

    @Input() set value(val: LanguageModel[]) {
        this._value = val;

        this.onChange(val);
        this.onTouched();
    }

    @Input() disabled = false;
    @Input() useOptional = false;

    onChange = (_: LanguageModel[]) => { };
    onTouched = () => { };

    constructor(private config: JobChINAngularConfig, private ref: ChangeDetectorRef) {
            this.languages = JSON.parse(JSON.stringify(this.config.languages));
    }

    addLang(): void {
        this._value.push({ languageId: undefined, skill: undefined, optional: this.useOptional ? true : undefined });
        this.onChange(this._value);
    }

    removeLang(lang: LanguageModel) {
        this._value = this._value.filter(x => x !== lang);
        this.enableSelectedLanguage(lang);
        this.onLanguagesChanged();
    }

    setLanguageValue(lang: LanguageModel, newValue: number) {
        if (lang.languageId === newValue) {
            return;
        }
        this.enableSelectedLanguage(lang);
        let newLang = this.languages.find(x => x.languageId === newValue);
        newLang.disabled = true;
        lang.languageId = newValue;
        this.onLanguagesChanged();
    }

    setSkillValue(lang: LanguageModel, newValue: LanguageSkill) {
        if (lang.skill === newValue) {
            return;
        }
        lang.skill = newValue;
        this.onChange(this.value);
    }

    clearSelectedLanguage(lang: LanguageModel) {
        this.enableSelectedLanguage(lang);
        this.onLanguagesChanged();
    }

    clearSelectedSkill(lang: LanguageModel) {
        lang.skill = null;
        this.onChange(this.value);
    }

    private enableSelectedLanguage(lang: LanguageModel) {
        if (lang.languageId == null) {
            return;
        }
        let oldLang = this.languages.find(x => x.languageId === lang.languageId);
        oldLang.disabled = false;
        lang.languageId = null;
    }

    private onLanguagesChanged() {
        this.languages = [...this.languages];
        this.ref.detectChanges();
        this.onChange(this.value);
    }

    // Implementation of the ControlValueAccessor
    writeValue(value: LanguageModel[]): void {
        this._value = [];
        if (value == null) {
            return;
        }
        for (let lang of value) {
            let newLang = this.languages.find(x => x.languageId === lang.languageId);
            if (newLang) {
                this._value.push(lang);
                newLang.disabled = true;
            }
        }
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

    // Implementation of the Validator
    validate(_: any) {
        const isNotValid = this._value.find(x => x.languageId == null || x.skill == null) != null;
        return isNotValid ? { jobchin_lang: true } : null;
    }
}
