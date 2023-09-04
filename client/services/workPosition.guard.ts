import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, of } from 'rxjs';
import { catchError, mergeMap } from 'rxjs/operators';

import { JobChINAngularConfig } from '../config/angularConfig';
import { JobChINProfileConfig } from '../config/profileConfig';
import { RouterUtils } from '../utils/routerUtils';
import { CompanyGuard } from './company.guard';
import { CompanyService } from './company.service';


@Injectable()
export class WorkPositionGuard extends CompanyGuard {

    constructor(protected router: Router, protected override config: JobChINAngularConfig, protected companyService: CompanyService, protected override profile: JobChINProfileConfig) {
        super(config, profile)
     }

    override canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        super.canActivate(route, state);

        const id = this.getWorkPositionId(route);
        if (this.profile.company.workPosition?.workPositionId === id) {
            return this.onWorkPositionLoaded(route);
        }
        this.profile.company.workPosition = null;
        return this.companyService.getWorkPositionDetail(id).pipe(
            mergeMap(wp => {
                this.profile.company.workPosition = wp;
                return this.onWorkPositionLoaded(route);
            }),
            catchError(err => this.router.navigate(RouterUtils.GetWorkPositionNotFoundRouterLink()))
        );
    }

    protected onWorkPositionLoaded(route: ActivatedRouteSnapshot): Promise<boolean> {
        return of(true).toPromise();
    }

    protected getWorkPositionId(route: ActivatedRouteSnapshot): number {
        return +route.paramMap.get('id');
    }
}
