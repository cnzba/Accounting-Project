import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

@Component({
  selector: 'app-errorpage',
  templateUrl: './errorpage.component.html',
  styleUrls: ['./errorpage.component.css']
})
export class ErrorpageComponent implements OnInit {
    private errorMessage: string;

    constructor(private route: ActivatedRoute) {  }

  ngOnInit() {
      this.errorMessage = this.route.snapshot.paramMap.get('msg');
  }

}
