import { ChangeDetectorRef, Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { addDays, addMonths, max, min, startOfDay } from 'date-fns';

import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinLocalize } from '../../../../jobchin.localize';
import { jobchinSettings } from '../../../../jobchin.settings';
import { Branch } from '../../../../models/company/branch.model';
import { Remote } from '../../../../models/remote.enum';
import { BasicInfoModel } from '../../../../models/workPosition/basicInfo.model';
import { CompanyService } from '../../../../services/company.service';
import { JobChINValidators } from '../../../../utils/validators';
import { WorkPositionBaseForm } from '../workPositionBaseForm';
import { CompanyUserViewConfig } from './../../../../config/companyUserViewConfig';
import { Role } from './../../../../models/company/role.enum';

@Component({
    selector: 'jobchin-workPosition-basicInfo',
    templateUrl: 'basicInfo.component.html',
    styleUrls: [ '../../../../assets/error.scss' ],
})
export class WorkPositionBasicInfoComponent extends WorkPositionBaseForm<BasicInfoModel> {
    jobchinSettings = jobchinSettings;

    maxExpiration = 3;
    showJobBeginning = false;
    branches = [];
    published = false;
    hasPublication = false;
    intervals: Interval[];
    currentInterval: Interval = {
        start: null,
        end: null
    };
    intervalsLoading = true;
    filterDate = undefined;
    filterExprirationDate = undefined;
    users: CompanyUserViewConfig[];

    location = this.formConfig.form.controls.location as FormGroup;

    languages = [
        { value: 'cs', displayValue: jobchinLocalize.cs },
        { value: 'en', displayValue: jobchinLocalize.en }
    ];

    get propertyName(): string {
        return 'basicInfo';
    }

    constructor(protected override readonly route: ActivatedRoute, protected override readonly companyService: CompanyService, protected override readonly profile: JobChINProfileConfig, private config: JobChINAngularConfig, private ref: ChangeDetectorRef) {
        super(route, companyService, profile);

        this.users = this.profile.company.users.filter(x => x.role === Role.WorkPositionAdmin && x.memberId !== this.profile.company.memberId);
        this.maxExpiration = this.profile.company.maxDuration;
        if (this.profile.company.model.branches.branches) {
            this.branches = this.profile.company.model.branches.branches.map(x => this.mapBranch(x));
        } else {
            this.location.controls.branchId.disable({ emitEvent: false});
        }
    }

    override ngOnInit(): void {
        super.ngOnInit();

        if (this.standAlone) {
            this.companyService.getCompanyFreeSlots(this.profile.company.workPosition?.workPositionId).subscribe(
                result => {
                    this.intervals = result;
                    this.intervalsLoading = false;
                    this.filterDate = this.filterDateFunc(this.intervals);
                    this.onPublicationChange(this.formConfig.form, this.formConfig.form.controls.publication.value);
                },
            );
        } else if (!this.profile.company.intervals) {
            this.companyService.getCompanyFreeSlots(null).subscribe(
                result => {
                    this.intervals = result;
                    this.intervalsLoading = false;
                    this.filterDate = this.filterDateFunc(this.intervals);
                    this.formConfig.form.controls.publication.setValue(this.getPublicationMinDate());
                },
            );
        } else {
            this.intervals = this.profile.company.intervals;
            this.intervalsLoading = false;
            this.filterDate = this.filterDateFunc(this.intervals);
            this.formConfig.form.controls.publication.setValue(this.getPublicationMinDate());
        }
    }

    initForm(): FormGroup {
        let locationGroup =  new FormGroup({
            locationId: new FormControl(null),
            branchId: new FormControl(null),
        }, JobChINValidators.atLeastOneValidator());
        locationGroup.controls.locationId.valueChanges.subscribe(x => this.onLocationChanged(locationGroup, x));
        locationGroup.controls.branchId.valueChanges.subscribe(x => this.onBranchChanged(locationGroup, x));

        let form = new FormGroup({
            language: new FormControl(null, Validators.required),
            name: new FormControl(null, [Validators.maxLength(jobchinSettings.workPositionNameMaxLength), Validators.required]),
            publication: new FormControl(startOfDay(new Date()), Validators.required),
            expiration: new FormControl(null, Validators.required),
            workFromNow: new FormControl(true),
            jobBeginning: new FormControl(null),
            remote: new FormControl(null, Validators.required),
            location: locationGroup,
            contractTypes: new FormControl([], [Validators.required, Validators.minLength(1)]),
            users: new FormControl([]),
            public: new FormControl(false),
        });
        form.controls.publication.valueChanges.subscribe(x => this.onPublicationChange(form, x));
        form.controls.workFromNow.valueChanges.subscribe(x => this.onWorkFromNowChange(form, x));
        form.controls.remote.valueChanges.subscribe(x => this.onRemoteChanged(form, x));
        return form;
    }

    mapModel(form: FormGroup): BasicInfoModel {
        let loc = form.controls.location as FormGroup;
        let users = form.controls.users.value;
        if (this.profile.company.role !== Role.CompanyAdmin) {
            users.push(this.profile.company.memberId);
        }
        return {
            language: form.controls.language.value,
            name: form.controls.name.value,
            publication: form.controls.publication.value,
            expiration: form.controls.expiration.value,
            jobBeginning: form.controls.workFromNow.value ? null : form.controls.jobBeginning.value,
            remote: form.controls.remote.value,
            locationId: loc.controls.locationId.value,
            branchId: loc.controls.branchId.value,
            contractTypes: form.controls.contractTypes.value,
            users: users,
            public: form.controls.public.value,
        }
    }

    initModel(model: BasicInfoModel): void {
        this.formConfig.form.controls.language.setValue(model.language);
        this.formConfig.form.controls.name.setValue(model.name);
        this.formConfig.form.controls.publication.setValue(model.publication || this.getPublicationMinDate());
        this.formConfig.form.controls.expiration.setValue(model.expiration);
        this.formConfig.form.controls.workFromNow.setValue(model.jobBeginning == null);
        this.formConfig.form.controls.jobBeginning.setValue(model.jobBeginning);
        this.formConfig.form.controls.remote.setValue(model.remote);
        this.location.controls.locationId.setValue(model.locationId);
        this.location.controls.branchId.setValue(model.branchId);
        this.formConfig.form.controls.contractTypes.setValue(model.contractTypes || []);
        this.formConfig.form.controls.users.setValue(model.users?.filter(x => x !== this.profile.company.memberId) || []);
        this.formConfig.form.controls.public.setValue(model.public);

        if (model.publication < new Date()) {
            this.formConfig.form.controls.publication.disable();
            this.published = true;
        }
    }

    hasBranches(): boolean {
        return this.branches.length > 0;
    }

    getPublicationMinDate() {
        if (this.intervals?.length > 0) {
            return this.intervals[0].start;
        }
        return new Date();
    }

    getPublicationMaxDate() {
        if (this.intervals?.length > 0) {
            return this.intervals[this.intervals.length - 1].end;
        }
        return new Date();
    }

    getExpirationMinDate() {
        return this.currentInterval?.start || new Date();
    }

    getExpirationMaxDate() {
        return this.currentInterval?.end || new Date();
    }

    getLocalAdministrativeUnits() {
        return this.config.localAdministrativeUnits;
    }

    filterDateFunc(intervals: Interval[]) {
        return (d: Date): boolean => {
            const find = intervals?.find(x => x.start <= d && x.end > d);
            return find !== undefined;
        }
    }

    filterExprirationDateFunc(intervals: Interval[], publication: Date, maxExpiration) {
        let interval = intervals?.find(x => x.start <= publication && x.end > publication);
        if (!interval && publication <= new Date()) {
            interval = intervals?.find(x => x.start <= new Date());
        }
        if (!publication || !interval) {
            return (d: Date): boolean => false;
        }
        const maxDate = min([interval.end, addMonths(publication, maxExpiration)]);
        return (d: Date): boolean => interval.start < d && maxDate >= d && d > publication;
    }

    private onPublicationChange(form: FormGroup, publication: Date) {
        if (!this.intervals) {
            return;
        }
        let expiration = form.controls.expiration.value;
        this.filterExprirationDate = this.filterExprirationDateFunc(this.intervals, publication, this.maxExpiration);
        if (publication) {
            this.currentInterval.start = max([addDays(publication, 1), startOfDay(new Date())]);
            let interval = this.intervals?.find(x => x.start <= publication || x.end >= publication) || this.intervals[0]
            this.currentInterval.end = min([interval.end, addMonths(publication, this.maxExpiration)]);
            if (expiration < this.currentInterval.start || expiration > this.currentInterval.end) {
                form.controls.expiration.setValue(null);
            }
        } else {
            form.controls.expiration.setValue(null);
            this.currentInterval = null;
        }
        form.controls.expiration.updateValueAndValidity();
        this.ref.markForCheck();
    }

    private onWorkFromNowChange(form: FormGroup, val: boolean) {
        this.showJobBeginning = !val;
        if (val) {
            form.controls.jobBeginning.removeValidators(Validators.required);
        } else {
            form.controls.jobBeginning.addValidators(Validators.required);
        }
        form.controls.jobBeginning.updateValueAndValidity();
    }

    private onRemoteChanged(form: FormGroup, val: Remote) {
        if (val === Remote.Full) {
            form.controls.location.clearValidators();
        } else {
            form.controls.location.addValidators(JobChINValidators.atLeastOneValidator());
        }
        form.controls.location.updateValueAndValidity();
    }

    private onLocationChanged(form: FormGroup, val: string) {
        if (val) {
            form.controls.branchId.disable({ emitEvent: false});
            form.controls.branchId.setValue(null);
        } else if (this.hasBranches()) {
            form.controls.branchId.enable({ emitEvent: false});
        }
    }

    private onBranchChanged(form: FormGroup, val: number) {
        if (val) {
            form.controls.locationId.disable({ emitEvent: false});
            form.controls.locationId.setValue(null);
        } else {
            form.controls.locationId.enable({ emitEvent: false});
        }
    }

    private mapBranch(branch: Branch) {
        let displayValue = branch.street;
        if (branch.street && branch.city) {
            displayValue += ' - ';
        }
        displayValue += branch.city;
        return {
            value: branch.branchId,
            displayValue: displayValue,
        };
    }
}
