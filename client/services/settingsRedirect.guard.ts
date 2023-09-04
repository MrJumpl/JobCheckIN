import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

import { JobChINProfileConfig } from '../config/profileConfig';
import { jobchinRoutes } from '../jobchin.routes';
import { Role } from '../models/company/role.enum';

@Injectable()
export class SettingsRedirectGuard implements CanActivate {

    constructor(protected router: Router, protected profile: JobChINProfileConfig) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        if (this.profile.student) {
            return this.router.navigate([state.url, jobchinRoutes.visibility], { queryParamsHandling: 'merge' });
        }
        if (this.profile.company) {
            if (this.profile.company.role == Role.CompanyAdmin) {
                return this.router.navigate([state.url, jobchinRoutes.companyType], { queryParamsHandling: 'merge' });
            } else {
                return this.router.navigate([state.url, jobchinRoutes.contactPerson], { queryParamsHandling: 'merge' });
            }
        }
        return true;
    }
}


