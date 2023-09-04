import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

import { JobChINProfileConfig } from '../config/profileConfig';
import { JobChINAngularConfig } from './../config/angularConfig';

@Injectable()
export class CompanyGuard implements CanActivate {

    constructor(protected config: JobChINAngularConfig, protected profile: JobChINProfileConfig) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        if (this.profile.student) {
            window.location.href = this.config.studentAfterLogin;
        }
        if (this.profile.company) {
            return true;
        }
    }
}


