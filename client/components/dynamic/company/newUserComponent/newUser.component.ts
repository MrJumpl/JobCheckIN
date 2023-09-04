import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { UmbracoService } from '../../../../../../_shared/services/umbraco.service';
import { MemberDataModel } from '../../../../../../members/models/member-data.model';
import { ContactPersonModel } from '../../../../models/company/contactPerson.model';
import { CompanyCreateModel } from '../../../../models/company/create.model';
import { TakeOverAccountModel } from '../../../../models/company/newCompanyUser.model';
import { RegistrationSteps } from '../../../../models/registerSteps.enum';
import { CompanyService } from '../../../../services/company.service';

@Component({
    selector: 'jobchin-company-newUser',
    templateUrl: 'newUser.component.html',
})
export class CompanyNewUserComponent {
    RegistrationSteps = RegistrationSteps;

    step = RegistrationSteps.SignUp;
    postResult = false;

    token: string;
    companyName: string;
    signUpModel: MemberDataModel;
    contactPersonModel: ContactPersonModel;

    constructor(private umbraco: UmbracoService, private companyService: CompanyService) {
            this.token = this.umbraco.angularConfig.token;
            this.companyName = this.umbraco.angularConfig.companyName;
            this.signUpModel = {
                email: this.umbraco.angularConfig.email,
            };
        }

    nextSignedUp(model: MemberDataModel) {
        this.signUpModel = model;
        this.step = RegistrationSteps.ContactPerson;
    }

    nextSuccess(result: string) {
        this.step = RegistrationSteps.Success;
        if (result) {
            this.postResult = true;
            setTimeout(() => {
                window.location.href = result;
            }, 3000);
        }
    }

    takeOverAccount = (model: ContactPersonModel): Observable<CompanyCreateModel> => {
        this.contactPersonModel = model;
        let takeOverAccount: TakeOverAccountModel = {
            token: this.token,
            signUpModel: this.signUpModel,
            contactPerson: this.contactPersonModel,
        }
        return this.companyService.createNewUser(takeOverAccount);
    }

    backContactPerson = (model: ContactPersonModel): void => {
        this.contactPersonModel = model;
        this.step = RegistrationSteps.SignUp;
    }
}
