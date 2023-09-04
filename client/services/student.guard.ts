import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';

import { membersRoutes } from '../../../members/members.routes';
import { JobChINProfileConfig } from '../config/profileConfig';

@Injectable()
export class StudentGuard implements CanActivate {

    constructor(protected router: Router, protected profile: JobChINProfileConfig) { }

    canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        if (this.profile.company) {
            this.router.navigate([membersRoutes.profile]);
        }
        if (this.profile.student) {
            return true;
        }
    }
}
