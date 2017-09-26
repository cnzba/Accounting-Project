# CBA Accounting ClientApp

This project was generated with [Angular CLI](https://github.com/angular/angular-cli) version 1.4.0.

## Editor configuration
The .editorconfig overrides some editor settings that may have been set within Visual Studio or other editor.
This is primarily to keep formatting consistent (e.g. number of spaces per tab) when different developers are using different editors.

## Overview of top-level project files
* _.NET Core_
* **WebApp.csproj** Project file for .NET Core
* **appsettings.json** Settings file for .NET Core
* **Program.cs** Program main entry file for .NET Core (builds web host)
* **Startup.cs** Configuration file for .NET Core (configures middleware)
* _Angular_
* **.angular-cli.json** The main configuration file for Angular
* **proxy.conf.json** Links the angular client to the .NET Core Web API. When using "npm start", all api requests (/api/...) are routed to the local .NET server. The port in this file should be the same as the port in Properties/launchSettings.json.
* **karma.conf.js** Unit test configuration for Angular
* **protractor.conf.js** End to end test configuration for Angular
* **package.json** NPM package configuration for Angular
* **tsconfig.json** TypeScript configuration for Angular
* **tslint.json** Lint tool configuration for Angular

## Development server

Run `npm start` or `ng serve --proxy-config proxy.conf.json` for a dev server. Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files.
Run `dotnet run` to run the local web api. Navigate to `http://localhost:62682/api/users`.

## Code scaffolding

Run `ng generate component component-name` to generate a new component. You can also use `ng generate directive|pipe|service|class|guard|interface|enum|module`.

## Build

Run `ng build` to build the project. The build artifacts will be stored in the `wwwroot/` directory. Use the `-prod` flag for a production build.

## Running unit tests

Run `ng test` to execute the unit tests via [Karma](https://karma-runner.github.io).

## Running end-to-end tests

Run `ng e2e` to execute the end-to-end tests via [Protractor](http://www.protractortest.org/).
Before running the tests make sure you are serving the app via `ng serve`.

## Further help

To get more help on the Angular CLI use `ng help` or go check out the [Angular CLI README](https://github.com/angular/angular-cli/blob/master/README.md).
