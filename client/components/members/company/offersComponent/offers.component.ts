import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { MuniWebButtonSize } from '../../../../../../_templates/muni-web/models/button-size';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinRoutes } from '../../../../jobchin.routes';
import { CompanyWorkPositionsPaged } from '../../../../models/workPosition/companyWorkPositionPaged.model';
import { CompanyService } from '../../../../services/company.service';

@Component({
    selector: 'jobchin-company-offers',
    templateUrl: 'offers.component.html',
    styleUrls: [ './offers.component.scss' ],
})
export class CompanyOffersComponent {
    MuniWebButtonSize = MuniWebButtonSize;
    jobchinRoutes = jobchinRoutes;

    currentLoading = true;
    currentError = false;
    current: CompanyWorkPositionsPaged;
    archivedLoading = true;
    archivedError = false;
    archived: CompanyWorkPositionsPaged;

    intervalsLoading = true;

    constructor(protected router: Router, private route: ActivatedRoute, private profile: JobChINProfileConfig, private companyService: CompanyService) {
        this.loadCurrentPage(1);
        this.loadArchivedPage(1);

        this.companyService.getCompanyFreeSlots(null).subscribe(
            result => {
                this.profile.company.intervals = result;
                this.intervalsLoading = false;
            },
        );
    }

    loadCurrentPage(page: number) {
        this.currentLoading = true;
        this.companyService.getCurrentWorkPositionPaged(page).subscribe(
            result => {
                this.current = result;
            },
            _ => {
                this.currentError = true;
            },
            () => {
                this.currentLoading = false;
            }
        );
    }

    loadArchivedPage(page: number) {
        this.archivedLoading = true;
        this.companyService.getArchivedWorkPositionPaged(page).subscribe(
            result => {
                this.archived = result;
            },
            _ => {
                this.archivedError = true;
            },
            () => {
                this.archivedLoading = false;
            }
        );
    }

    canCreate(): boolean {
        return this.profile.company.confirmed && this.profile.company.intervals && this.profile.company.intervals.length > 0;
    }

    redirectCreate() {
        this.router.navigate([ jobchinRoutes.create ], { relativeTo: this.route });
    }

    redirectUpdate(workPositionId: number) {
        this.router.navigate([ workPositionId, jobchinRoutes.update ], { relativeTo: this.route });
    }

    archiveActionClicked() {
        this.archivedLoading = true;
    }

    currentActionClicked() {
        this.currentLoading = true;
    }
}
