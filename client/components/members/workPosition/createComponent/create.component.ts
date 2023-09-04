import { Component, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';

import { MuniWebButtonSize } from '../../../../../../_templates/muni-web/models/button-size';
import { jobchinRoutes } from '../../../../jobchin.routes';
import { BasicInfoModel } from '../../../../models/workPosition/basicInfo.model';
import { CandidateRequestModel } from '../../../../models/workPosition/candidateRequest.model';
import { CandidatesModel } from '../../../../models/workPosition/candidates.model';
import { WorkPositionCreateModel } from '../../../../models/workPosition/create.model';
import { CreateSteps } from '../../../../models/workPosition/createSteps.enum';
import { DetailModel } from '../../../../models/workPosition/detail.model';
import { WorkPositionUpdateModel } from '../../../../models/workPosition/update.model';
import { CompanyService } from '../../../../services/company.service';
import { ModelUtils } from '../../../../utils/modelUtils';
import { WorkPositionBasicInfoComponent } from '../basicInfoComponent/basicInfo.component';
import { WorkPositionCandidateRequestComponent } from '../candidateRequestComponent/candidateRequest.component';
import { WorkPositionCandidatesComponent } from '../candidatesComponent/candidates.component';
import { WorkPositionDetailComponent } from '../detailComponent/detail.component';

@Component({
    selector: 'jobchin-workPosition-create',
    templateUrl: 'create.component.html',
})
export class WorkPositionCreateComponent {
    CreateSteps = CreateSteps;
    MuniWebButtonSize = MuniWebButtonSize;

    step = CreateSteps.BasicInfo;
    createModel: WorkPositionCreateModel = new WorkPositionCreateModel();

    @ViewChild(WorkPositionBasicInfoComponent) basicInfoComponent: WorkPositionBasicInfoComponent;
    @ViewChild(WorkPositionDetailComponent) detailComponent: WorkPositionDetailComponent;
    @ViewChild(WorkPositionCandidatesComponent) candidatesComponent: WorkPositionCandidatesComponent;
    @ViewChild(WorkPositionCandidateRequestComponent) candidateRequestComponent: WorkPositionCandidateRequestComponent;

    constructor(protected companyService: CompanyService, protected router: Router, protected route: ActivatedRoute) { }

    protected getListRoute() {
        return  `../../${jobchinRoutes.myOffres}`;
    }

    redirectList() {
        this.router.navigate([ this.getListRoute() ], { relativeTo: this.route });
    }

    nextBasicInfo(model: BasicInfoModel) {
        this.createModel.basicInfo = model;
        this.step = CreateSteps.Detail;
    }

    nextDetail(model: DetailModel) {
        this.createModel.detail = model;
        this.step = CreateSteps.Candidates;
    }

    nextCandidates(model: CandidatesModel) {
        this.createModel.candidates = model;
        this.step = CreateSteps.CandidateRequest;
    }

    protected getUpdateRoute(workPositionId: number) {
        return `../${workPositionId}/${jobchinRoutes.update}`;
    }

    nextCandidateRequest(result: WorkPositionUpdateModel) {
        this.step = CreateSteps.Success;
        setTimeout(() => {
            this.router.navigate([ this.getUpdateRoute(result.workPositionId) ], { relativeTo: this.route });
        }, 3000);
    }

    createWorkPosition = (model: CandidateRequestModel): Observable<WorkPositionUpdateModel> => {
        this.createModel.candidateRequest = model;
        return this.companyService.createWorkPosition(this.createModel);
    }

    backDetail = (model: DetailModel): void => {
        this.createModel.detail = model;
        this.step = CreateSteps.BasicInfo;
    }

    backCandidates = (model: CandidatesModel): void => {
        this.createModel.candidates = model;
        this.step = CreateSteps.Detail;
    }

    backCandidateRequest = (model: CandidateRequestModel): void => {
        this.createModel.candidateRequest = model;
        this.step = CreateSteps.Candidates;
    }

    getModel = () => {
        let model = ModelUtils.deepCopy(this.createModel);
        switch (this.step) {
            case CreateSteps.BasicInfo:
                model.basicInfo = this.basicInfoComponent.getCurrentModel();
                if (!model.basicInfo) {
                    return null;
                }
                break;
            case CreateSteps.Detail:
                model.detail = this.detailComponent.getCurrentModel();
                if (!model.detail) {
                    return null;
                }
                break;
            case CreateSteps.Candidates:
                model.candidates = this.candidatesComponent.getCurrentModel();
                if (!model.candidates) {
                    return null;
                }
                break;
            case CreateSteps.CandidateRequest:
                model.candidateRequest = this.candidateRequestComponent.getCurrentModel();
                if (!model.candidateRequest) {
                    return null;
                }
                break;
        }
        return model;
    }
}
