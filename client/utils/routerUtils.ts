import { jobchinRoutes } from '../jobchin.routes';
import { membersRoutes } from './../../../members/members.routes';


export class RouterUtils {
    public static GetCompanyRegisterRouterLink() {
        return [`/${membersRoutes.register}`, jobchinRoutes.company];
    }

    public static GetCompanyLoginRouterLink() {
        return [`/${membersRoutes.login}`, jobchinRoutes.company];
    }

    public static GetCompanyLoginRoute() {
        return this.GetCompanyLoginRouterLink().join('/');
    }

    public static GetStudentRegisterRouterLink() {
        return [`/${membersRoutes.register}`, jobchinRoutes.student];
    }

    public static GetStudentLoginRoute() {
        return this.GetStudentLoginRouterLink().join('/');
    }

    public static GetStudentLoginRouterLink() {
        return [`/${membersRoutes.login}`, jobchinRoutes.student];
    }

    public static GetWorkPositionNotFoundRouterLink() {
        return [`/${membersRoutes.profile}`, jobchinRoutes.myOffres, jobchinRoutes.notFound];
    }
}
