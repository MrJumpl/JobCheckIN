import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';

import { JobChINProfileConfig } from '../config/profileConfig';

@Injectable()
export class JobChINZoneRootGuard implements CanActivate {
    constructor(
        public router: Router,
        public profile: JobChINProfileConfig
    ) { }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
        if (this.profile.company || this.profile.student) {
            return true;
        }
    }
}
