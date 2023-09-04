import { Component, Input } from '@angular/core';

import { StudentCompanyListView } from '../../../../models/company/studentCompanyListView.model';

@Component({
    selector: 'jobchin-student-companyListView',
    templateUrl: 'companyListView.component.html',
    styleUrls: [ './companyListView.component.scss' ],
})
export class StudentCompanyListViewComponent {
    groups: Record<string, StudentCompanyListView[]> = {};
    empty: boolean = false;

    @Input() set companies(val: any[]) {
        this.empty = false;
        if (!val) {
            this.groups = {};
            return;
        }
        if (val.length === 0) {
            this.empty = true;
            this.groups = {};
            return;
        }
        this.groups = val.reduce((groups, item) => {
            (groups[item.group] ||= []).push(item);
            return groups;
          }, {} as Record<string, StudentCompanyListView[]>);
    }
    
    @Input() noLogo: string;

    constructor() {
    }

    getGroups(): string[] {
        return Object.keys(this.groups);
    }

    getLogo(companyLogo: string) {
        return companyLogo || this.noLogo;
    }
}
