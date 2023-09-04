import { Directive, EventEmitter, HostListener, Input, Output, ViewChild } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { Observable } from 'rxjs';

import { areEquals } from '../../../../../../core/utils/object-utils';
import { MuniWebForm2Component } from '../../../../../_templates/muni-web/components/form/form2.component';
import { JobChINFormConfig } from '../../../config/formConfig';
import { jobchinLocalize } from '../../../jobchin.localize';
import { ComponentCanDeactivate } from '../../../services/pendingChanges.guard';

/**
 * Base class for custom implementations of data overview actions (e.g. some operations with selected records which require some UI)
 */
@Directive()
export abstract class BaseForm<T, TResult> implements ComponentCanDeactivate {

    submitAttempt = false;
    formConfig: JobChINFormConfig<T, TResult>;

    private _model: T;
    get model(): T {
        return this._model;
    }
    @Input() set model(value: T) {
        this._model = value;
        if (value != null) {
            this.initModel(value);
        }
    }

    @ViewChild(MuniWebForm2Component) formRef: MuniWebForm2Component;

    @Input() standAlone = true;

    @Input() set submitButtonLabel(label: string) {
        this.formConfig.submitButtonLabel = label;
    }

    @Input() set onCallServer(call: (model: T) => Observable<any>) {
        this.formConfig.onCallServer = call;
    }

    @Input() set onCallBack(call: (model: T) => void) {
        this.formConfig.onCallBack = () => {
            const model = this.mapModel(this.formConfig.form);
            call(model);
        };
    }

    @Output() onSubmit: EventEmitter<any> = new EventEmitter();

    @HostListener('window:beforeunload') canDeactivate(): Observable<boolean> | boolean {
        if (this.formConfig.form.dirty) {
            if (this.model == null || (Object.keys(this.model).length === 0 && Object.getPrototypeOf(this.model) === Object.prototype)) {
                return false;
            }
            return areEquals(this.mapModel(this.formConfig.form), this.model);
        }
        return true;
    }

    constructor() {
        this.formConfig = {
            form: this.initForm(),
            onGetFormModel: form => this.mapModel(form),
            onCallServer: (model) => new Observable<T>(subscriber => {
                subscriber.next(model);
              }),
            onServerCallback: (result, model) => this.serverCallback(result, model),
            submitButtonLabel: jobchinLocalize.save,
        }
    }

    serverCallback(result: TResult, model: T): boolean {
        this.formRef?.reset();
        this.onServerCallback(result, model);
        this.onSubmit.emit(result);
        return true;
    }

    abstract initForm(): FormGroup;
    abstract mapModel(form: FormGroup): T;
    abstract initModel(model: T): void;

    onServerCallback(result: TResult, model: T): void {
        this.model = model;
    }
}
