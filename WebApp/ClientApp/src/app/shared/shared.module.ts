/**
 * Keep the shared module and component in this module
 * 1. Import and export the modules shared in the App.
 * 2. The other module only need to import shared module and then can use all the shared modules.
 */
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatToolbarModule, MatIconModule, MatButtonModule, MatCardModule, MatInputModule, MatListModule, MatGridListModule, MatDialogModule, MatAutocompleteModule, MatSidenavModule } from '@angular/material';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

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
        MatSidenavModule
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
        MatSidenavModule
    ],
    declarations: [],
    
})
export class SharedModule {}