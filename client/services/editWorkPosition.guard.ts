import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router } from '@angular/router';

import { JobChINAngularConfig } from '../config/angularConfig';
import { JobChINProfileConfig } from '../config/profileConfig';
import { jobchinRoutes } from '../jobchin.routes';
import { CompanyService } from './company.service';
import { WorkPositionGuard } from './workPosition.guard';


@Injectable()
export class EditWorkPositionGuard extends WorkPositionGuard {

    constructor(protected override router: Router, protected override config: JobChINAngularConfig, protected override companyService: CompanyService, protected override profile: JobChINProfileConfig) {
        super(router, config, companyService, profile);
    }

    protected override onWorkPositionLoaded(route: ActivatedRouteSnapshot): Promise<boolean> {
        if (this.profile.company.workPosition?.basicInfo.expiration < new Date()) {
            let url = route.parent.pathFromRoot.map(x => x.url.map(y => y.path).join('/'));
            url.push(jobchinRoutes.expired);
            return this.router.navigate([ url.join('/') ]);
        }
        return super.onWorkPositionLoaded(route);
    }

    protected override getWorkPositionId(route: ActivatedRouteSnapshot): number {
        return super.getWorkPositionId(route.parent);
    }
}
