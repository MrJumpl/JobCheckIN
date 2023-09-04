import { Component, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable } from 'rxjs';

import {
    MuniWebPopUpButtonComponent,
} from '../../../../../../_templates/muni-web/components/pop-up-button/pop-up-button.component';
import { MuniWebButtonState } from '../../../../../../_templates/muni-web/models/button-state';
import { CompanyUserViewConfig } from '../../../../config/companyUserViewConfig';
import { JobChINProfileConfig } from '../../../../config/profileConfig';
import { jobchinSettings } from '../../../../jobchin.settings';
import { ChangeRoleModel } from '../../../../models/company/changeRole.model';
import { Role } from '../../../../models/company/role.enum';
import { SendUserInvitation } from '../../../../models/company/sendUserInvitation';
import { UserInvitation } from '../../../../models/company/userInvitation.model';
import { CompanyService } from '../../../../services/company.service';
import { ContactPersonComponent } from '../contactPersonComponent/contactPerson.component';
import { FormConfig } from './../../../../../../_templates/muni-web/components/form/form2.component';

@Component({
    selector: 'jobchin-company-newUserInvitation',
    templateUrl: 'users.component.html',
    styleUrls:  ['../../../../assets/error.scss'],
})
export class UsersComponent {
    jobchinSettings = jobchinSettings;
    
    invitation: UserInvitation = {
        state: MuniWebButtonState.init,
        config: null,
        success: false,
        postedEmail: null,
    }
    takeOver: UserInvitation = {
        state: MuniWebButtonState.init,
        config: null,
        success: false,
        postedEmail: null,
    }
    editingStates: { [index:number]: {
        state: MuniWebButtonState,
        popUp: MuniWebPopUpButtonComponent,
        config: FormConfig<ChangeRoleModel, CompanyUserViewConfig>,
    } } = {};
    deletingStates: { [index:number]: {
        state: MuniWebButtonState,
        popUp: MuniWebPopUpButtonComponent,
    } } = {};

    @ViewChild(ContactPersonComponent) contactPerson: ContactPersonComponent;

    canDeactivate(): Observable<boolean> | boolean {
        return this.contactPerson.canDeactivate();
    }

    constructor(private companyService: CompanyService, private profile: JobChINProfileConfig) {
        this.invitation.config = {
            form: new FormGroup({
                newEmail: new FormControl(null, Validators.compose([
                    Validators.pattern('[^ @]*@[^ @]*'),
                    Validators.required,
                    Validators.maxLength(250)
                ])),
                role: new FormControl(null, Validators.required),
            }),
            onGetFormModel: form => this.getInvitationModel(form),
            onCallServer: (model) => {
                this.invitation.state = MuniWebButtonState.loading;
                return this.companyService.sendInvitation(model)
            },
            onServerCallback: (_, model) => this.onSuccess(this.invitation, model),
            onServerErrorCallback: (err, model) => this.onError(this.invitation),
        }

        this.takeOver.config = {
            form: new FormGroup({
                newEmail: new FormControl(null, Validators.compose([
                    Validators.pattern('[^ @]*@[^ @]*'),
                    Validators.required,
                    Validators.maxLength(250)
                ])),
            }),
            onGetFormModel: form => this.getInvitationModel(form),
            onCallServer: (model) => {
                this.takeOver.state = MuniWebButtonState.loading;
                return this.companyService.sendInvitation(model)
            },
            onServerCallback: (_, model) => this.onSuccess(this.takeOver, model),
            onServerErrorCallback: (err, model) => this.onError(this.takeOver),
        }

        for (let user of this.profile.company.users) {
            this.deletingStates[user.memberId] = {
                state: MuniWebButtonState.init,
                popUp: null, 
            };
            this.editingStates[user.memberId] = {
                state: MuniWebButtonState.init,
                popUp: null,
                config: {
                    form: new FormGroup({
                        memberId: new FormControl(user.memberId),
                        role: new FormControl(user.role, Validators.required),
                    }),
                    onGetFormModel: form => this.getEditModel(form),
                    onCallServer: (model) => {
                        this.editingStates[user.memberId].state = MuniWebButtonState.loading;
                        return this.companyService.changeRole(model)
                    },
                    onServerCallback: (_, model) => {
                        this.editingStates[user.memberId].state = MuniWebButtonState.success;
                        setTimeout(() => {
                            this.editingStates[user.memberId].popUp.closePopUp();
                        }, 3000);
                        return false;
                    },
                    onServerErrorCallback: (err, model) => {
                        this.editingStates[user.memberId].state = MuniWebButtonState.error;
                        return false;
                    },
                }
            } ;
        }
    }

    getUsers() {
        return this.profile.company.users;
    }

    getRoleDisplay(role: Role): string {
        return this.jobchinSettings.roleOptions.find(x => x.value === role)?.displayValue;
    }
    
    isSameUser(memberId: number): boolean {
        return this.profile.company.memberId === memberId;
    }

    reset(invitation: UserInvitation): void {
        invitation.success = false;
        invitation.config.form.controls.newEmail.setValue(null);
        if (invitation.config.form.contains('role')) {
            invitation.config.form.controls.role.setValue(null);
        }
    }

    deleteUser(memberId: number) {
        this.deletingStates[memberId].state = MuniWebButtonState.loading;
        this.companyService.deleteUser(memberId).subscribe(
            _ => {
                this.deletingStates[memberId].state = MuniWebButtonState.success;
                setTimeout(() => {
                    this.deletingStates[memberId].popUp.closePopUp();
                }, 1000)
            },
            _ => {
                this.deletingStates[memberId].state = MuniWebButtonState.error;
            }
        )
    }
    
    onEditPopUpOpen(memberId: number, popUp: MuniWebPopUpButtonComponent) {
        this.editingStates[memberId].popUp = popUp;
    }
    
    onDeletePopUpOpen(memberId: number, popUp: MuniWebPopUpButtonComponent) {
        this.deletingStates[memberId].popUp = popUp;
    }

    closeDeletePopUp(memberId: number) {
        this.deletingStates[memberId].popUp.closePopUp();
    }

    private getEditModel(form: FormGroup): ChangeRoleModel {
        return {
            memberId: form.controls.memberId.value,
            role: form.controls.role.value,
        };
    }

    private getInvitationModel(form: FormGroup): SendUserInvitation {
        let role = Role.CompanyAdmin;
        if (form.contains('role')) {
            role = form.controls.role.value;
        }
        return {
            email: form.controls.newEmail.value,
            role: role,
        };
    }
    
    private onSuccess(invitation: UserInvitation, model: SendUserInvitation): boolean {
        invitation.success = true;
        invitation.postedEmail = model.email;
        invitation.state = MuniWebButtonState.success;
        return true;
    }

    private onError(invitation: UserInvitation): boolean {
        invitation.state = MuniWebButtonState.error;
        return false;
    }
}
