import { FormConfig } from './../../../../_templates/muni-web/components/form/form2.component';
import { MuniWebButtonState } from './../../../../_templates/muni-web/models/button-state';
import { SendUserInvitation } from './sendUserInvitation';

export interface UserInvitation {
    state: MuniWebButtonState;
    config: FormConfig<SendUserInvitation, any>;
    success: boolean;
    postedEmail: string;
}