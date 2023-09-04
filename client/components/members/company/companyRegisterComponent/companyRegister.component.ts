import { Component } from '@angular/core';
import { Observable } from 'rxjs';

import { EntyMenuState } from '../../../../../../members/models/entry-menu-state.enum';
import { MemberDataModel } from '../../../../../../members/models/member-data.model';
import { ContactPersonModel } from '../../../../models/company/contactPerson.model';
import { CompanyCreateModel } from '../../../../models/company/create.model';
import { GeneralInfoModel } from '../../../../models/company/generalInfo.model';
import { RegistrationSteps } from '../../../../models/registerSteps.enum';
import { CompanyService } from '../../../../services/company.service';
import { RouterUtils } from '../../../../utils/routerUtils';

@Component({
    selector: 'jobchin-companyRegister',
    templateUrl: 'companyRegister.component.html',
})
export class CompanyRegisterComponent {
    EntyMenuState = EntyMenuState;
    RouterUtils = RouterUtils;
    RegistrationSteps = RegistrationSteps;

    step = RegistrationSteps.SignUp;
    registrationSuccessful: string;

    signUpModel: MemberDataModel;
    contactPersonModel: ContactPersonModel;
    generalInfoModel: GeneralInfoModel;

    constructor(private companyService: CompanyService) { }


    nextSignedUp(model: MemberDataModel) {
        this.signUpModel = model;
        this.step = RegistrationSteps.ContactPerson;
    }

    nextContactPerson(model: ContactPersonModel) {
        this.contactPersonModel = model;
        this.step = RegistrationSteps.GeneralInfo;
        return true;
    }

    nextSuccess(result: string) {
        this.registrationSuccessful = result;
        this.step = RegistrationSteps.Success;
    }

    createAccount = (model: GeneralInfoModel): Observable<string> => {
        this.generalInfoModel = model;
        let createDto: CompanyCreateModel = {
            signUpModel: this.signUpModel,
            contactPerson: this.contactPersonModel,
            generalInfo: this.generalInfoModel,
        }
        return this.companyService.registerCompany(createDto);
    }

    backContactPerson = (model: ContactPersonModel): void => {
        this.contactPersonModel = model;
        this.step = RegistrationSteps.SignUp;
    }

    backGeneralInfo = (model: GeneralInfoModel): void => {
        this.generalInfoModel = model;
        this.step = RegistrationSteps.ContactPerson;
    }
}
