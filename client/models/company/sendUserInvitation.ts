import { Role } from './role.enum';

export interface SendUserInvitation {
    email: string;
    role: Role;
}
