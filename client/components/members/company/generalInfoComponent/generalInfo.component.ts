import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';

import { WebApiErrorResponse } from '../../../../../../_shared/models/webapi-error-response';
import { JobChINAngularConfig } from '../../../../config/angularConfig';
import { CompanyRegistrationConfig } from '../../../../config/companyRegistrationConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { GeneralInfoDTO } from '../../../../models/company/generalInfo.dto.model';
import { GeneralInfoModel } from '../../../../models/company/generalInfo.model';
import { CompanyUpdateModel } from '../../../../models/company/update.model';
import { IcoStates } from '../../../../models/icoStates.enum';
import { Sector } from '../../../../models/sector.enum';
import { CompanyService } from '../../../../services/company.service';
import { BaseForm } from '../../baseForm/baseForm';

@Component({
    selector: 'jobchin-company-generalInfo',
    templateUrl: 'generalInfo.component.html',
    styleUrls: [ '../../../../assets/socialMedia.scss' ]
})
export class GeneralInfoComponent extends BaseForm<GeneralInfoModel, CompanyUpdateModel> implements OnInit {
    IcoStates = IcoStates;
    jobchinSettings = jobchinSettings;

    icoState: IcoStates;
    companyTypes = [Sector.PrivateSector, Sector.NonProfitSector, Sector.PublicSector];

    lastIcoChecked: string;
    loadingInfo = true;
    infoError = false;
    static registrationConfig: CompanyRegistrationConfig;

    constructor(private profile: JobChINProfileConfig, public config: JobChINAngularConfig, private companyService: CompanyService) {
        super();

        if (this.standAlone && this.profile.company) {
            this.lastIcoChecked = this.profile.company.model.generalInfo.ico.toString();
            this.icoState = IcoStates.Success;

            this.model = this.profile.company.model.generalInfo;
            this.onCallServer = (model) => {
                let dto = new CompanyUpdateModel();
                dto.generalInfo = model;
                return this.companyService.updateCompany(dto);
            };
            this.formConfig.form.controls.agreement.setValue(true);
        } else {
            this.icoState = IcoStates.Short;
        }
    }

    ngOnInit(): void {
        if (!this.standAlone && !GeneralInfoComponent.registrationConfig) {
            this.companyService.getRegistrationGonfig().subscribe(
                result => {
                    GeneralInfoComponent.registrationConfig = result;
                    this.loadingInfo = false;
                },
                err => {
                    this.infoError = true;
                    this.loadingInfo = false;
                })
        } else {
            this.loadingInfo = false;
        }
    }

    initForm(): FormGroup {
        let form = new FormGroup({
            ico: new FormControl(null, [Validators.maxLength(jobchinSettings.icoLength), Validators.required]),
            hasNoDic: new FormControl(false),
            dic: new FormControl(null, Validators.maxLength(jobchinSettings.dicMaxLength)),
            companyName: new FormControl(null, [Validators.maxLength(jobchinSettings.nameMaxLength), Validators.required]),
            street: new FormControl(null, [Validators.maxLength(jobchinSettings.streetMaxLength), Validators.required]),
            city: new FormControl(null, [Validators.maxLength(jobchinSettings.cityMaxLength), Validators.required]),
            zipCode: new FormControl(null, [Validators.maxLength(jobchinSettings.zipCodeMaxLength), Validators.required]),
            countryId: new FormControl(null, Validators.required),
            hasCor: new FormControl(false),
            corStreet: new FormControl(null, Validators.maxLength(jobchinSettings.streetMaxLength)),
            corCity: new FormControl(null, Validators.maxLength(jobchinSettings.cityMaxLength)),
            corZipCode: new FormControl(null, Validators.maxLength(jobchinSettings.zipCodeMaxLength)),
            corCountryId: new FormControl(null),
            workerCountRangeId: new FormControl(null, Validators.required),
            sector: new FormControl(null, Validators.required),
            agreement: new FormControl(false, Validators.requiredTrue),
        });

        form.controls.ico.valueChanges.subscribe(x => this.onICOChanged(x));
        form.controls.hasCor.valueChanges.subscribe(x => this.onHasCorChanged(x));

        return form;
    }

    mapModel(form: FormGroup): GeneralInfoModel {
        const hasDic = !form.controls.hasNoDic.value;
        const hasCor = form.controls.hasCor.value;
        return ({
            ico: form.controls.ico.value,
            dic: hasDic ? form.controls.dic.value : null,
            companyName: form.controls.companyName.value,
            street: form.controls.street.value,
            city: form.controls.city.value,
            zipCode: form.controls.zipCode.value,
            countryId: form.controls.countryId.value,
            correspondenceStreet: hasCor ? form.controls.corStreet.value : null,
            correspondenceCity: hasCor ? form.controls.corCity.value : null,
            correspondenceZipCode: hasCor ? form.controls.corZipCode.value : null,
            correspondenceCountryId: hasCor ? form.controls.corCountryId.value : null,
            workerCountRangeId: form.controls.workerCountRangeId.value,
            sector: form.controls.sector.value,
            gdprAgreement: form.controls.agreement.value,
        });
    }

    initModel(model: GeneralInfoModel): void {
        const hasCor = model.correspondenceStreet != null && model.correspondenceStreet !== '';

        this.formConfig.form.controls.ico.setValue(model.ico);
        this.formConfig.form.controls.hasNoDic.setValue(model.dic == null || model.dic === '');
        this.formConfig.form.controls.dic.setValue(model.dic);
        this.formConfig.form.controls.companyName.setValue(model.companyName);
        this.formConfig.form.controls.street.setValue(model.street);
        this.formConfig.form.controls.city.setValue(model.city);
        this.formConfig.form.controls.zipCode.setValue(model.zipCode);
        this.formConfig.form.controls.countryId.setValue(model.countryId);
        this.formConfig.form.controls.hasCor.setValue(hasCor);
        this.formConfig.form.controls.corStreet.setValue(model.correspondenceStreet);
        this.formConfig.form.controls.corCity.setValue(model.correspondenceCity);
        this.formConfig.form.controls.corZipCode.setValue(model.correspondenceZipCode);
        this.formConfig.form.controls.corCountryId.setValue(model.correspondenceCountryId);
        this.formConfig.form.controls.workerCountRangeId.setValue(model.workerCountRangeId);
        this.formConfig.form.controls.sector.setValue(model.sector);

        if (this.standAlone) {
            this.formConfig.form.controls.ico.disable();
            this.formConfig.form.controls.sector.disable();
        }
    }

    showDic() {
        return !this.formConfig.form.controls.hasNoDic.value;
    }

    hasCor() {
        return this.formConfig.form.controls.hasCor.value;
    }

    icoLoaded(): boolean {
        return this.icoState === IcoStates.Success;
    }

    icoError(): boolean {
        return this.icoState === IcoStates.NotFound || this.icoState === IcoStates.WrongFormat || this.icoState === IcoStates.Conflict || this.icoState === IcoStates.Error;
    }

    onICOChanged(ico: string) {
        if (ico === this.lastIcoChecked) {
            return;
        }
        this.lastIcoChecked = ico;
        let len = ico?.length || 0;
        if (len !== 8) {
            this.icoState = IcoStates.Short;
            return;
        }
        if (!ico.match(/\d+/)) {
            this.icoState = IcoStates.WrongFormat;
            return;
        }
        this.formConfig.form.controls.ico.setErrors({ loading: true });
        this.icoState = IcoStates.Loading;
        this.companyService.validateCompanyIco(ico).subscribe(
            (model: GeneralInfoDTO) => {
                if (model.ico !== this.lastIcoChecked) {
                    return;
                }
                this.icoState = IcoStates.Success;
                this.formConfig.form.controls.ico.setErrors(null);

                this.formConfig.form.controls.dic.setValue(model.dic);
                this.formConfig.form.controls.hasNoDic.setValue(model.dic == null || model.dic === '');
                this.formConfig.form.controls.companyName.setValue(model.companyName);
                this.formConfig.form.controls.street.setValue(model.street);
                this.formConfig.form.controls.city.setValue(model.city);
                this.formConfig.form.controls.zipCode.setValue(model.zipCode);
                this.formConfig.form.controls.countryId.setValue(model.countryId);

            },
            (err: WebApiErrorResponse) => {
                if (ico !== this.lastIcoChecked) {
                    return;
                }
                if (err.httpResponse.status === 409) {
                    this.icoState = IcoStates.Conflict;
                    this.formConfig.form.controls.ico.setErrors({ conflict: true });
                } else if (err.httpResponse.status === 404) {
                    this.icoState = IcoStates.NotFound;
                    this.formConfig.form.controls.ico.setErrors({ notExists: true });
                } else {
                    this.icoState = IcoStates.Error;
                    this.formConfig.form.controls.ico.setErrors({ serverError: true });
                }
            }

        );
    }

    onHasCorChanged(hasCor: boolean) {
        if (hasCor) {
            this.formConfig.form.controls.corStreet.setValidators(Validators.required);
            this.formConfig.form.controls.corStreet.updateValueAndValidity();
            this.formConfig.form.controls.corCity.setValidators(Validators.required);
            this.formConfig.form.controls.corCity.updateValueAndValidity();
            this.formConfig.form.controls.corZipCode.setValidators(Validators.required);
            this.formConfig.form.controls.corZipCode.updateValueAndValidity();
            this.formConfig.form.controls.corCountryId.setValidators(Validators.required);
            this.formConfig.form.controls.corCountryId.updateValueAndValidity();
        } else {
            this.formConfig.form.controls.corStreet.clearValidators();
            this.formConfig.form.controls.corStreet.updateValueAndValidity();
            this.formConfig.form.controls.corCity.clearValidators();
            this.formConfig.form.controls.corCity.updateValueAndValidity();
            this.formConfig.form.controls.corZipCode.clearValidators();
            this.formConfig.form.controls.corZipCode.updateValueAndValidity();
            this.formConfig.form.controls.corCountryId.clearValidators();
            this.formConfig.form.controls.corCountryId.updateValueAndValidity();
        }
    }

    getCompanyTypeName(type: Sector) {
        switch (type) {
            case Sector.PrivateSector:
                return $localize`:@@JobChIN.privateSector:Soukromý`;
            case Sector.NonProfitSector:
                return $localize`:@@JobChIN.nonProfitSector:Neziskový`;
            case Sector.PublicSector:
                return $localize`:@@JobChIN.publicSector:Veřejný`;
            default:
                return '';
        }
    }

    getGdprLink() {
        return GeneralInfoComponent.registrationConfig?.gdprUrl;
    }
}
