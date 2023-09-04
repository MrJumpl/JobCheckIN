import { Directive, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { JobChINProfileConfig } from '../../../config/profileConfig';
import { WorkPositionUpdateModel } from '../../../models/workPosition/update.model';
import { CompanyService } from '../../../services/company.service';
import { BaseForm } from '../baseForm/baseForm';

/**
 * Base class for custom implementations of data overview actions (e.g. some operations with selected records which require some UI)
 */
@Directive()
export abstract class WorkPositionBaseForm<T> extends BaseForm<T, WorkPositionUpdateModel> implements OnInit {

    constructor(protected readonly route: ActivatedRoute, protected readonly companyService: CompanyService, protected readonly profile: JobChINProfileConfig) {
        super();
    }

    ngOnInit(): void {
        if (this.standAlone) {
            this.model = this.profile.company.workPosition[this.propertyName];
            const id = Number(this.route.parent.snapshot.paramMap.get('id'));
            this.onCallServer = (model) => {
                let dto = new WorkPositionUpdateModel();
                dto.workPositionId = id;
                dto[this.propertyName] = model;
                return this.companyService.updateWorkPosition(dto);
            };
        }
    }

    abstract get propertyName(): string;

    override onServerCallback(result: WorkPositionUpdateModel, model: T): void {
        this.model = result[this.propertyName];
    }

    public getCurrentModel(): T {
        if (!this.formConfig.form.valid) {
            return null;
        }
        return this.mapModel(this.formConfig.form);
    }
}
