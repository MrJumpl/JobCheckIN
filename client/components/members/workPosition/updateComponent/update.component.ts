import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { MuniWebButtonSize } from '../../../../../../_templates/muni-web/models/button-size';
import { jobchinRoutes } from '../../../../../../sites/JobChIN/jobchin.routes';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { CompanyService } from '../../../../services/company.service';
import { ModelUtils } from '../../../../utils/modelUtils';
import { WorkPositionBasicInfoComponent } from '../basicInfoComponent/basicInfo.component';
import { WorkPositionCandidateRequestComponent } from '../candidateRequestComponent/candidateRequest.component';
import { WorkPositionCandidatesComponent } from '../candidatesComponent/candidates.component';
import { WorkPositionDetailComponent } from '../detailComponent/detail.component';

@Component({
    selector: 'jobchin-workPosition-update',
    templateUrl: 'update.component.html',
})
export class WorkPositionUpdateComponent implements OnInit {
    MuniWebButtonSize = MuniWebButtonSize;
    jobchinRoutes = jobchinRoutes;

    opened: WorkPositionBasicInfoComponent | WorkPositionDetailComponent | WorkPositionCandidateRequestComponent | WorkPositionCandidatesComponent;
    id: number;

    constructor(protected router: Router, private route: ActivatedRoute, private profile: JobChINProfileConfig, private companyService: CompanyService) { }

    ngOnInit(): void {
        this.id = Number(this.route.snapshot.paramMap.get('id'));
    }

    redirectOverview() {
        this.router.navigate([ `../${jobchinRoutes.overview}` ], { relativeTo: this.route });
    }

    getName(): string {
        return this.profile.company.workPosition?.basicInfo.name;
    }

    canEdit(): boolean {
        return this.profile.company.workPosition?.basicInfo.expiration > new Date();
    }

    onActivate(componentRef) {
        if (componentRef instanceof WorkPositionBasicInfoComponent
            || componentRef instanceof WorkPositionDetailComponent
            || componentRef instanceof WorkPositionCandidatesComponent
            || componentRef instanceof WorkPositionCandidateRequestComponent) {
                this.opened = componentRef;
        }
    }

    getModel = () => {
        let model = ModelUtils.deepCopy(this.profile.company.workPosition);
        if (this.opened instanceof WorkPositionBasicInfoComponent) {
            model.basicInfo = this.opened.getCurrentModel();
            if (!model.basicInfo) {
                return null;
            }
        } else if (this.opened instanceof WorkPositionDetailComponent) {
            model.detail = this.opened.getCurrentModel();
            if (!model.detail) {
                return null;
            }
        } else if (this.opened instanceof WorkPositionCandidatesComponent) {
            model.candidates = this.opened.getCurrentModel();
            if (!model.candidates) {
                return null;
            }
        } else if (this.opened instanceof WorkPositionCandidateRequestComponent) {
            model.candidateRequest = this.opened.getCurrentModel();
            if (!model.candidateRequest) {
                return null;
            }
        }
        return model;
    }
}
