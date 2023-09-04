import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

import { JobChINAngularConfig } from '../config/angularConfig';
import { JobChINProfileConfig } from '../config/profileConfig';
import { Role } from '../models/company/role.enum';
import { CompanyGuard } from './company.guard';


@Injectable()
export class CompanyAdminGuard extends CompanyGuard {

    constructor(private router: Router, protected override config: JobChINAngularConfig, protected override profile: JobChINProfileConfig) {
        super(config, profile);
    }

    override canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        super.canActivate(route, state);

        if (this.profile.company.role === Role.CompanyAdmin) {
            return true;
        }
        this.router.navigate(['..']);
    }
}
