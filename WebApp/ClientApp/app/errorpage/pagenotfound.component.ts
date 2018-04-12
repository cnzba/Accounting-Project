import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from "@angular/router";

@Component({
    templateUrl: './pagenotfound.component.html',
    styleUrls: ['./pagenotfound.component.css']
})
export class PageNotFoundComponent implements OnInit {
    private errorMessage: string;

    constructor(private route: ActivatedRoute) { }

    ngOnInit() {
        if (this.route.snapshot.paramMap.has('msg'))
            this.errorMessage = this.route.snapshot.paramMap.get('msg');
        else this.errorMessage = "404 Page not found."
    }

}
