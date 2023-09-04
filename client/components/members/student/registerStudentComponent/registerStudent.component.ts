import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

import { MuniWebButtonState } from '../../../../../../_templates/muni-web/models/button-state';
import { MembersService } from '../../../../../../members/services/members.service';
import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { StudentAngularConfig } from '../../../../config/studentConfig';
import { StudentRegistrationConfig } from '../../../../config/studentRegistrationConfig';
import { jobchinLocalize } from '../../../../jobchin.localize';
import { StudentCreateModel } from '../../../../models/student/create.model';
import { StudentService } from '../../../../services/student.service';
import { RouterUtils } from '../../../../utils/routerUtils';
import { BaseForm } from '../../baseForm/baseForm';


@Component({
    selector: 'jobchin-registerStudent',
    templateUrl: 'registerStudent.component.html',
})
export class RegisterStudentComponent extends BaseForm<StudentCreateModel, StudentAngularConfig> implements OnInit {
    providerId: string;
    registration: FormGroup;
    submitState: MuniWebButtonState;

    loading = true;
    err = false;
    success = false;
    registrationConfig: StudentRegistrationConfig;

    constructor(private router: Router, private studentService: StudentService, private profile: JobChINProfileConfig, private config: JobChINAngularConfig, private members: MembersService) {
        super();

        this.submitButtonLabel = jobchinLocalize.studentRegister,
        this.onCallServer = (model) => this.studentService.registerStudent(model);

        let state = router.getCurrentNavigation().extras.state;
        if (state) {
            this.providerId = state.providerId;
        } else {
            this.router.navigate(RouterUtils.GetStudentLoginRouterLink())
        }
    }

    ngOnInit(): void {
        this.studentService.getRegistrationGonfig().subscribe(
            config => {
                this.registrationConfig = config;
                this.loading = false;
            },
            _ => {
                this.err = true;
                this.loading = false;
            }
        );
    }

    initForm(): FormGroup {
        return  new FormGroup( {
            agreement: new FormControl(false, Validators.requiredTrue),
            provideContact: new FormControl(false),
        });
    }

    mapModel(form: FormGroup): StudentCreateModel {
        return {
            externalLoginProvider: this.providerId,
            provideContact: form.controls.provideContact.value,
            gdprAgreement: form.controls.agreement.value,
        };
    }

    initModel(model: StudentCreateModel): void {
        this.formConfig.form.controls.agreement.setValue(true);
        this.formConfig.form.controls.provideContact.setValue(model.provideContact);
    }

    override onServerCallback(result: StudentAngularConfig): void {
        this.success = true;
        this.profile = {
            company: undefined,
            student: result,
        };
        this.formConfig.form.reset();
        setTimeout(() => {
            window.location.href = this.config.studentAfterLogin;
        }, 3000);
    }
}
