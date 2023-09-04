import { Component } from '@angular/core';
import { FormArray, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { MuniWebButtonSize } from '../../../../../../_templates/muni-web/models/button-size';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinRoutes } from '../../../../jobchin.routes';
import { WorkPositionStudentsPaged } from '../../../../models/workPosition/workPositionStudentsPaged.model';
import { CompanyService } from '../../../../services/company.service';

@Component({
    selector: 'jobchin-company-workPositionOverview',
    templateUrl: 'workPositionOverview.component.html',
    styleUrls: [ './workPositionOverview.component.scss' ],
})
export class WorkPositionOverviewComponent {
    MuniWebButtonSize = MuniWebButtonSize;
    jobchinRoutes = jobchinRoutes;

    workPositionId: number;
    studentsLoading = true;
    studentsError = false;
    students: WorkPositionStudentsPaged;
    selected: FormArray;
    all = false;

    constructor(protected router: Router, private route: ActivatedRoute, private profile: JobChINProfileConfig, private companyService: CompanyService) {
        this.workPositionId = Number(this.route.snapshot.paramMap.get('id'));
        this.selected = new FormArray([]);
        this.loadStudentsPage(1);
    }

    canEdit() {
        return this.profile.company.workPosition.basicInfo.expiration > new Date();
    }

    getName() {
        return this.profile.company.workPosition.basicInfo.name;
    }

    redirectList() {
        this.router.navigate([ '../../../' ], { relativeTo: this.route });
    }

    redirectUpdate() {
        this.router.navigate([ `../${jobchinRoutes.update}` ], { relativeTo: this.route });
    }

    redirectDuplicate() {
        this.router.navigate([ `../${jobchinRoutes.duplicate}` ], { relativeTo: this.route });
    }

    loadStudentsPage(page: number) {
        this.studentsLoading = true;
        this.companyService.getWorkPositionStudents(this.workPositionId, page).subscribe(
            result => {
                this.students = result;

                while (this.selected.length > 0) {
                    this.selected.removeAt(0);
                }
                this.students.students.forEach(_ => {
                    let control = new FormControl(false);
                    control.valueChanges.subscribe(x => this.onStudentSelected(x))
                    this.selected.push(control);
                });
            },
            _ => {
                this.studentsError = true;
            },
            () => {
                this.studentsLoading = false;
            }
        );
    }

    selectAll() {
        const val = !this.all;
        this.selected.controls.forEach(control => {
            control.setValue(val);
        });
    }

    selectedAny() {
        return this.selected.controls.some(x => x.value);
    }

    redirectStudentDetail() {
        window.open('https://www.w3schools.com');
    }

    onStudentSelected(checked: boolean) {
        this.all = this.selected.controls.every(x => x.value);
    }
}
