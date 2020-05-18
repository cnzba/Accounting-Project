/**
 * Keep the shared module and component in this module
 * 1. Import and export the modules shared in the App.
 * 2. The other module only need to import shared module and then can use all the shared modules.
 */
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule, MatIconModule, MatButtonModule, MatCardModule, 
    MatInputModule, MatListModule, MatGridListModule, MatDialogModule, 
    MatAutocompleteModule, MatSidenavModule, MatTableModule, MatFormFieldModule,
     MatFormFieldControl, MatTabsModule, MatSelectModule, MatCheckboxModule, MAT_CHECKBOX_CLICK_ACTION, ErrorStateMatcher } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {CdkTableModule} from '@angular/cdk/table';
import { ReactiveFormsModule, FormsModule } from "@angular/forms";
import { UploadComponent } from './upload';
import { RestrictInputDirective } from "../directives/restrict-input.directive";


@NgModule({
    
    imports: [
        MatToolbarModule,
        MatIconModule,
        MatButtonModule,
        MatCardModule,
        MatInputModule,
        BrowserAnimationsModule,
        MatListModule,
        MatGridListModule,
        MatDialogModule,
        MatAutocompleteModule,
        MatSidenavModule,
        MatTableModule,
        MatFormFieldModule,
        MatTabsModule,
        ReactiveFormsModule,
        MatSelectModule,
        MatCheckboxModule,
        FormsModule,
         ],
    exports: [
        MatToolbarModule,
        MatIconModule,
        MatButtonModule,
        MatCardModule,
        MatInputModule,
        BrowserAnimationsModule,
        MatListModule,
        MatGridListModule,
        MatDialogModule,
        MatAutocompleteModule,
        MatSidenavModule,
        MatTableModule,
        MatFormFieldModule,
        MatTabsModule,
        ReactiveFormsModule,
        MatSelectModule,
        MatCheckboxModule,
        FormsModule,
        UploadComponent,
        RestrictInputDirective
    ],

    providers:[
        {provide: MAT_CHECKBOX_CLICK_ACTION, useValue: 'check'}
    ],
    declarations: [
        UploadComponent,
        RestrictInputDirective
    ],
    
})
export class SharedModule {}