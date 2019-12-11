import { PermissionValues } from './permission.model';


export interface LoginResponse {
    access_token: string;
    id_token: string;
    refresh_token: string;
    expires_in: number;
}


export interface IdToken {
  //sub: string;
  id: string;
  username: string;
  fullname: string;
  //jobtitle: string;
  email: string;
  //phone: string;
  roles: string | string[];
  permission: PermissionValues | PermissionValues[];
  //permission: string | string[];
  //configuration: string;
}
