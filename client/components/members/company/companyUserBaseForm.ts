import { Directive, OnInit } from '@angular/core';

import { JobChINProfileConfig } from '../../../config/profileConfig';
import { CompanyUserUpdateModel } from '../../../models/company/companyUserUpdate.model';
import { CompanyService } from '../../../services/company.service';
import { BaseForm } from '../baseForm/baseForm';

/**
 * Base class for custom implementations of data overview actions (e.g. some operations with selected records which require some UI)
 */
@Directive()
export abstract class CompanyUserBaseForm<T> extends BaseForm<T, CompanyUserUpdateModel> implements OnInit {

    constructor(protected readonly companyService: CompanyService, protected readonly profile: JobChINProfileConfig) {
        super();
    }

    ngOnInit(): void {
        if (this.standAlone && this.profile) {
            this.model = this.profile.company.user[this.propertyName];
            this.onCallServer = (model) => {
                let dto = new CompanyUserUpdateModel();
                dto[this.propertyName] = model;
                return this.companyService.updateCompanyUser(dto);
            };
        }
    }

    abstract get propertyName(): string;

    override onServerCallback(result: CompanyUserUpdateModel, model: T): void {
        this.model = result[this.propertyName];
    }
}
