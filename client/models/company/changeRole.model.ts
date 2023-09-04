import { Role } from './role.enum';

export interface ChangeRoleModel {
    memberId: number;
    role: Role;
}
