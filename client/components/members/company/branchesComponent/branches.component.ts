import { Component } from '@angular/core';
import { FormArray, FormControl, FormGroup, Validators } from '@angular/forms';

import { TinyMceModuleType } from '../../../../../../../core/services/tinymce-config.service';
import { TinyMceConfigService } from '../../../../../../_templates/muni-web/tinymce/services/tinymce-config.service';
import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { Branch } from '../../../../models/company/branch.model';
import { CompanyBranches } from '../../../../models/company/companyBranches.model';
import { CompanyService } from '../../../../services/company.service';
import { CompanyBaseForm } from '../companyBaseForm';


@Component({
    selector: 'jobchin-company-branches',
    templateUrl: 'branches.component.html',
    styleUrls: [ '../../../../assets/accordion.scss', '../../../../assets/error.scss' ],
})
export class BranchesComponent extends CompanyBaseForm<CompanyBranches> {
    jobchinSettings = jobchinSettings;
    tinyConfig: Record<string, any>;

    branches = this.formConfig.form?.controls?.branches as FormArray;

    get propertyName(): string {
        return 'branches';
    }

    constructor(protected override companyService: CompanyService, protected override profile: JobChINProfileConfig, public config: JobChINAngularConfig, private tinyMceConfig: TinyMceConfigService) {
        super(companyService, profile);

        this.tinyConfig = this.tinyMceConfig.get(TinyMceModuleType.all);
    }

    initForm(): FormGroup {
        return new FormGroup({
            branches: new FormArray([]),
        });
    }

    mapModel(form: FormGroup): CompanyBranches {
        let brancehs = form.controls.branches as FormArray;
        return {
            branches: brancehs.controls.map((x: FormGroup) => this.mapBranch(x))
        };
    }

    initModel(model: CompanyBranches): void {
        setTimeout(() => {
            this.removeAllBrancehs();
            model.branches?.forEach(element => {
                this.addBranch(element);
            });
        });
    }

    addBranch(branch: Branch = undefined): void {
        let group = new FormGroup({
            branchId: new FormControl(branch?.branchId ?? 0, Validators.required),
            city: new FormControl(branch?.city, [Validators.maxLength(jobchinSettings.cityMaxLength), Validators.required]),
            street: new FormControl(branch?.street, [Validators.maxLength(jobchinSettings.streetMaxLength), Validators.required]),
            zipCode: new FormControl(branch?.zipCode, [Validators.maxLength(jobchinSettings.zipCodeMaxLength), Validators.required]),
            locationId: new FormControl(branch?.locationId, Validators.required),
        });
        this.branches.push(group);
    }

    removeBranch(index: number): void {
        this.branches.removeAt(index);
    }

    getLocalAdministrativeUnits() {
        return this.config.localAdministrativeUnits;
    }

    getTitle(workExperience: FormGroup): string {
        const street = workExperience.controls.street.value || '';
        const city = workExperience.controls.city.value || '';
        if (!street && !city) {
            return '';
        }
        return street
            + ((street && city) ? ' - ' : '')
            + city;
    }

    isNew(workExperience: FormGroup): boolean {
        return workExperience.controls.branchId.value === 0;
    }

    private removeAllBrancehs() {
        while (this.branches.length !== 0) {
            this.branches.removeAt(0)
        }
    }

    private mapBranch(form: FormGroup): Branch {
        return {
            branchId: form.controls.branchId.value,
            city: form.controls.city.value,
            street: form.controls.street.value,
            zipCode: form.controls.zipCode.value,
            locationId: form.controls.locationId.value,
        }
    }
}
