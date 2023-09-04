import { Directive, OnInit } from '@angular/core';

import { JobChINProfileConfig } from '../../../config/profileConfig';
import { CompanyUpdateModel } from '../../../models/company/update.model';
import { CompanyService } from '../../../services/company.service';
import { BaseForm } from '../baseForm/baseForm';

/**
 * Base class for custom implementations of data overview actions (e.g. some operations with selected records which require some UI)
 */
@Directive()
export abstract class CompanyBaseForm<T> extends BaseForm<T, CompanyUpdateModel> implements OnInit {

    constructor(protected readonly companyService: CompanyService, protected readonly profile: JobChINProfileConfig) {
        super();
    }

    ngOnInit(): void {
        if (this.standAlone && this.profile) {
            this.model = this.profile.company.model[this.propertyName];
            this.onCallServer = (model) => {
                let dto = new CompanyUpdateModel();
                dto[this.propertyName] = model;
                return this.companyService.updateCompany(dto);
            };
        }
    }

    abstract get propertyName(): string;

    override onServerCallback(result: CompanyUpdateModel, model: T): void {
        this.model = result[this.propertyName];
    }
}


