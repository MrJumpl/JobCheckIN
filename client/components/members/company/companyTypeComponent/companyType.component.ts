import { Component } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { addDays } from 'date-fns';

import { FormConfig } from '../../../../../../_templates/muni-web/components/form/form2.component';
import { CompanyTypeConfig } from '../../../../config/companyTypeConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { BuyCompanyTypeDto } from '../../../../models/company/buyCompanyType.dto.model';
import { CompanyCompanyTypeModel } from '../../../../models/company/companyCompanyType.model';
import { CompanyTypeModel } from '../../../../models/company/companyType.model';
import { CompanyService } from '../../../../services/company.service';

@Component({
    selector: 'jobchin-companytype',
    templateUrl: 'companyType.component.html',
    styleUrls: ['./companyType.component.scss'],
    styles: [' :host ::ng-deep .error .box-bg--white-border { border-color: red; }'],
})
export class CompanyTypeComponent {
    successBuy = false;
    formConfig: FormConfig<BuyCompanyTypeDto>
    config: CompanyTypeConfig;
    selected: number;
    ocReturn = false;
    ocPaid = false;

    constructor(private route: ActivatedRoute, private profile: JobChINProfileConfig, private companyService: CompanyService) {
        this.config = this.profile.company.companyTypeConfig;

        this.formConfig = {
            form: new FormGroup({
                activeFrom: new FormControl(this.getMinDate(), Validators.required),
                companyTypeId: new FormControl(null, Validators.required),
            }),
            onGetFormModel: form => {
                return ({
                    activeFrom: form.controls.activeFrom.value,
                    companyTypeId: form.controls.companyTypeId.value,
                });
            },
            onCallServer: (model) => {
                return this.companyService.buyCompanyType(model);
            },
            onServerCallback: (result, _) => {
                setTimeout(() => {
                    window.location.href = result;
                }, 2000);
                this.successBuy = true;
                return true;
            }
        }

        this.route.queryParamMap.subscribe(queryParams => {
            if (queryParams.get('ocReturn')) {
                this.ocReturn = true;
            }
            if (queryParams.get('ocPaid')) {
                this.ocPaid = queryParams.get('ocPaid') === 'true';
            }
        });
    }

    downloadInvoice(requestId: number) {
        return () => this.companyService.downloadInvoice(requestId);
    }

    getMinDate(): Date {
        if (this.config?.currentCompanyTypes) {
            return addDays(this.config.currentCompanyTypes[this.config.currentCompanyTypes.length - 1].activeTo, 1);
        }
        return new Date();
    }

    getCompanyTypes(): CompanyTypeModel[] {
        return this.config?.companyTypes.filter(x => x.visible);
    }

    isSelected(companyTypeId: number): boolean {
        return this.selected === companyTypeId;
    }

    hasCompanyTypes(): boolean {
        return this.config?.currentCompanyTypes?.length > 0;
    }

    public select(companyTypeId: number) {
        this.selected = companyTypeId;
        this.formConfig.form.controls.companyTypeId.setValue(companyTypeId);
    }

    public openOrderDetail(companyType: CompanyCompanyTypeModel, element: HTMLTableRowElement) {
        if (!companyType.orderState) {
            return;
        }
        element.hidden = !element.hidden
    }
}
