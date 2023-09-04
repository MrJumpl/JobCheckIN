import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

import { ExternalLoginProvider } from '../../../../../../../core/models/external-login-provider';
import { WebApiErrorResponse } from '../../../../../../_shared/models/webapi-error-response';
import { ExternalLoginService } from '../../../../../../_shared/services/external-login.service';
import { MembersService } from '../../../../../../members/services/members.service';
import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { RouterUtils } from '../../../../utils/routerUtils';

@Component({
    selector: 'jobchin-loginCompany',
    templateUrl: './loginStudent.component.html',
})
export class LoginStudentComponent implements OnInit {
    providerId = ExternalLoginProvider.muni;
    loading = true;
    serverExternalError = false;
    accessExpired = false;
    externalErrorMessages = [];

    returnRoute: string;
    returnUrl: string;

    constructor(private router: Router, private route: ActivatedRoute, private extLoginService: ExternalLoginService, private members: MembersService, private config: JobChINAngularConfig, private profile: JobChINProfileConfig) {
    }

    ngOnInit() {
        this.route.queryParamMap.subscribe(queryParams => {
            this.returnRoute = queryParams.get('return');
            this.returnUrl = queryParams.get('returnUrl');
            if (this.profile.student) {
                this.redirectStudent();
            } else if (queryParams.get('provider')) {
                this.onLogin();
            } else {
                this.login();
            }
        });
    }

    login() {
        this.extLoginService.login(this.providerId, {
            callbackFunction: () => this.onLogin(),
            returnUrl: this.getReturnUrl(),
        });
    }

    onLogin() {
        this.members.loginExternal(this.providerId).subscribe(
            (profile: any) => {
                this.profile.student = profile.student;
                this.redirectStudent();
            },
            (err: WebApiErrorResponse) => {
                this.loading = false;
                this.externalErrorMessages = err.messages;
                if (err.httpResponse.status === 201 || err.httpResponse.status === 404) {
                    this.router.navigate(RouterUtils.GetStudentRegisterRouterLink(), { relativeTo: null, state: { providerId: this.providerId } });
                } else if (err.httpResponse.status === 400) {
                    this.accessExpired = true;
                } else {
                    this.serverExternalError = true;
                }
            });
    }

    getReturnUrl(): string {
        let result = RouterUtils.GetStudentLoginRoute();
        if (this.returnRoute) {
            result += `?return=${this.returnRoute}`;
        } else if (this.returnUrl) {
            result += `?returnUrl=${this.returnUrl}`;
        }
        return result;
    }

    private redirectStudent() {
        if (this.returnRoute) {
            this.router.navigate([this.returnRoute]);
        } else if (this.returnUrl) {
            window.location.href = this.returnUrl;
        } else {
            window.location.href = this.config.studentAfterLogin;
        }
    }
}
