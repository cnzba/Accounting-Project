# CBA Accounting WebApp

## Getting Started / Software Installation
1. Install an editor
* [Visual Studio 2017 Community](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio), or
* [Visual Studio Code](https://code.visualstudio.com/) and its [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp), or
* Other editor of your choice
2. Install [.NET Core 2.0.0 SDK](https://www.microsoft.com/net/core) or later
3. See [step 1 of the Angular QuickStart](https://angular.io/guide/quickstart) to install Node.js, NPM and the Angular CLI.
4. Get a copy of the code from the [repository](https://cbanewzealand.visualstudio.com/Accounting%20Software/_git/CBA%20Accounting). See this [tutorial](https://docs.microsoft.com/en-us/vsts/git/gitquickstart?tabs=visual-studio) 
for a quick start with Visual Studio or this [tutorial](https://docs.microsoft.com/en-us/vsts/git/tutorial/clone?tabs=command-line) 
for more in-depth instructions including how to do it from the command-line.

## Development servers
From the WebApp project/directory:

Run `npm start` or `ng serve --proxy-config proxy.conf.json` for the angular server. 
Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files. 
URLs starting with /api (e.g. `http://localhost:4200/api/users`) are proxied to the dotnet server described below.

Run `dotnet run` to run the dotnet server (local web api.) Navigate to `http://localhost:62682/swagger`.

You need both servers for the application to run correctly: therefore you will need 2 command prompts.

