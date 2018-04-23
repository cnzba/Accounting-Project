import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

@Component({
    templateUrl: './pagenotfound.component.html',
    styleUrls: ['./pagenotfound.component.css']
})
export class PageNotFoundComponent implements OnInit {

    constructor(private route: ActivatedRoute) { }

    ngOnInit() {
    }

}
