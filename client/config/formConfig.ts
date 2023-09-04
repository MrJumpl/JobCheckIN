import { FormConfig } from '../../../_templates/muni-web/components/form/form2.component';

export class JobChINFormConfig<TModel = any, TServerResult = any> extends FormConfig<TModel, TServerResult> {
    onCallBack?: () => void;
    submitButtonLabel: string;
}
