import { Component } from '@angular/core';
import { FormControl, FormGroup } from '@angular/forms';

import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { CompanyService } from '../../../../services/company.service';
import { JobChINAngularConfig } from './../../../../config/angularConfig';
import { SearchStudentFilterDto } from './../../../../models/company/searchStudentFilter.model';
import { WorkPositionStudentsPaged } from './../../../../models/workPosition/workPositionStudentsPaged.model';

@Component({
    selector: 'jobchin-company-studentSearch',
    templateUrl: 'studentSearch.component.html',
    styleUrls: [ '../../../../assets/match.scss' ],
})
export class CompanyStudentSearchComponent {
    jobchinSettings = jobchinSettings;

    showActiveDriver = false;
    studentsLoading = false;
    students: WorkPositionStudentsPaged;
    searchForm: FormGroup;
    filter: SearchStudentFilterDto;

    constructor(public config: JobChINAngularConfig, private companyService: CompanyService, private profile: JobChINProfileConfig) {
        this.filter = new SearchStudentFilterDto();
        this.searchForm = new FormGroup({
            contractTypes: new FormControl([]),
            drivingLicense: new FormControl(false),
            activeDriver: new FormControl(false),
            areaOfInterests: new FormControl([]),
            hardSkills: new FormControl([]),
            softSkills: new FormControl([]),
            languages: new FormControl([]),
            faculties: new FormControl([]),
        });
        this.searchForm.controls.drivingLicense.valueChanges.subscribe(x => this.onDrivingLicenseChange(x));

        this.search();
    }

    search() {
        this.filter.contractTypes = this.searchForm.controls.contractTypes.value;
        this.filter.drivingLicense = this.searchForm.controls.drivingLicense.value;
        this.filter.activeDriver = this.searchForm.controls.activeDriver.value;
        this.filter.areaOfInterests = this.searchForm.controls.areaOfInterests.value;
        this.filter.hardSkills = this.searchForm.controls.hardSkills.value;
        this.filter.softSkills = this.searchForm.controls.softSkills.value;
        // this.filter.languages = this.searchForm.controls.languages.value;
        this.filter.faculties = this.searchForm.controls.faculties.value;

        this.loadPage(1);
    }

    loadPage(page: number) {
        this.filter.pageNo = page;
        this.studentsLoading = true;
        this.companyService.searchStudents(this.filter).subscribe(
            result => {
                this.students = result;
                this.studentsLoading = false;
            }
        );
    }

    onDrivingLicenseChange(val: boolean) {
        this.showActiveDriver = val;
    }
}
