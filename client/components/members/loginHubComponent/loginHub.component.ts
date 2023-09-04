import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { EntyMenuState } from '../../../../../members/models/entry-menu-state.enum';
import { LoggedInEvent } from '../../../../../members/models/logged-in.event';
import { JobChINProfileConfig } from '../../../config/profileConfig';
import { jobchinRoutes } from '../../../jobchin.routes';

@Component({
    selector: 'jobchin-loginHub',
    templateUrl: 'loginHub.component.html',
})
export class LoginHubComponent implements OnInit {
    EntyMenuState = EntyMenuState;

    returnUrl: string;
    returnRoute: string;

    constructor(
        private router: Router,
        protected route: ActivatedRoute,
        private profile: JobChINProfileConfig) {
    }

    ngOnInit() {
        this.route.queryParamMap.subscribe(queryParams => {
            if (queryParams.get('returnUrl')) {
                this.returnUrl = queryParams.get('returnUrl');
            }
            this.returnRoute = queryParams.get('return');

            if (queryParams.get('passwordReset')) {
                this.router.navigate([jobchinRoutes.company], { relativeTo: this.route, queryParamsHandling: 'merge'  });
            }
        });
    }

    loginStudent() {
        this.router.navigate([jobchinRoutes.student], { relativeTo: this.route, queryParamsHandling: 'merge' });
    }

    loginCompany() {
        this.router.navigate([jobchinRoutes.company], { relativeTo: this.route, queryParamsHandling: 'merge'  });
    }

    login(status: LoggedInEvent) {
        this.onLogin(status);
    }

    loginExternal(status: LoggedInEvent) {
        this.onLogin(status);
    }

    onLogin(status: LoggedInEvent) {
        status.autoLoginRedirect = false;
        this.profile = status.profileConfig;
        if (this.returnRoute) {
            this.router.navigate([this.returnRoute]);
        } else {
            window.location.href = this.returnUrl;
        }
    }
}
