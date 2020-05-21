import { Component, OnInit, Input, Output,EventEmitter, SimpleChanges } from '@angular/core';
import { IUser } from 'src/app/users/user';
import { Router } from '@angular/router';


@Component({
  selector: 'app-header',
  templateUrl: './app-header.component.html',
  styleUrls: ['./app-header.component.css']
})
export class AppHeaderComponent implements OnInit {
  @Input() currentUser:IUser; 
  @Output() toggleSidebar = new EventEmitter<void>();
  @Output() isLogout = new EventEmitter<boolean>();

   _currentUser:IUser;

  constructor(private router:Router) { }

  ngOnInit() {
  }

  ngOnChanges(changes: SimpleChanges): void {    
    this._currentUser = this.currentUser;    
  }

  onLogout(){
    this.isLogout.emit(true);
  }

  toggleSidenav(){
    this.toggleSidebar.emit();
  }
}
