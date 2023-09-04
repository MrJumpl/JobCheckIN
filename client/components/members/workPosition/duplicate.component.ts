import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { JobChINProfileConfig } from '../../../config/profileConfig';
import { CompanyService } from '../../../services/company.service';
import { ModelUtils } from '../../../utils/modelUtils';
import { WorkPositionCreateComponent } from './createComponent/create.component';

@Component({
    selector: 'jobchin-workPosition-duplicate',
    templateUrl: './createComponent/create.component.html',
})
export class WorkPositionDuplicateComponent extends WorkPositionCreateComponent {

    constructor(protected override companyService: CompanyService, protected override router: Router, protected override route: ActivatedRoute, private profile: JobChINProfileConfig) {
        super(companyService, router, route);

        this.createModel = ModelUtils.deepCopy(this.profile.company.workPosition);
        this.createModel.basicInfo.publication = undefined;
        this.createModel.basicInfo.expiration = undefined;
     }

    override getListRoute() {
        return `../${super.getListRoute()}`;
    }

    override getUpdateRoute(workPositionId: number) {
        return `../${super.getUpdateRoute(workPositionId)}`;
    }
}
