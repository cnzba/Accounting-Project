import { IUser } from "./user";

export class LoginUser implements IUser{
    email: string;
    name: string;
    active: boolean;

}