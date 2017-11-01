import { Component, OnInit } from '@angular/core';
import { User } from "../users/user";
import { UserService } from "../users/user.service";

@Component({
  selector: 'app-logintest',
  templateUrl: './logintest.component.html',
  styleUrls: ['./logintest.component.css']
})
export class LogintestComponent implements OnInit {
    currentUser: User;
    users: User[] = [];

    constructor(private userService: UserService) {
        this.currentUser = JSON.parse(localStorage.getItem('currentUser'));
    }

    ngOnInit() {
        this.loadAllUsers();
    }

    deleteUser(id: number) {
        this.userService.delete(id).subscribe(() => { this.loadAllUsers() });
    }

    private loadAllUsers() {
        this.userService.getAll().subscribe(users => { this.users = users; });
    }
}
