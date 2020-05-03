import { IUser } from "./user";

export class LoginUser implements IUser{
    email: string;
    name: string;
    active: boolean;
    constructor(){
        this.email ="";
        this.name  = "";
        this.active = false;

    }

}