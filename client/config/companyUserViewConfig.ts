import { Role } from '../models/company/role.enum';

export interface CompanyUserViewConfig {
    memberId: number;
    fullName: string;
    role: Role;
}
