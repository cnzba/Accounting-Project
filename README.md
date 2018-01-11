# CBA Accounting WebApp

## Getting Started / Software Installation
1. Install an editor
   * [Visual Studio 2017 Community](https://docs.microsoft.com/en-us/visualstudio/install/install-visual-studio), or
   * [Visual Studio Code](https://code.visualstudio.com/) and its [C# extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode.csharp), or
   * Other editor of your choice
2. Install [.NET Core 2.0.0 SDK](https://www.microsoft.com/net/core) or later
3. See [step 1 of the Angular QuickStart](https://angular.io/guide/quickstart) to install Node.js, NPM and the Angular CLI.
4. Get a copy of the code from the [repository](https://cbanewzealand.visualstudio.com/Accounting%20Software/_git/CBA%20Accounting). Note there are two repositories within the **Accounting Software** project. You want the GIT repository **CBA Accounting**.
See [Get Started with GIT and VSTS](https://docs.microsoft.com/en-us/vsts/git/gitquickstart?tabs=visual-studio) and [Clone an existing GIT repo](https://docs.microsoft.com/en-us/vsts/git/tutorial/clone?tabs=command-line) for instructions on how to clone a repository. Using VS2017 is easiest but you can also do it via the command line.
5. From the WebApp directory run the command `npm install` to download all the packages used by the Angular client. You can also run `dotnet restore` at this time to do the same thing for the web api, although it isn't strictly necessary as a restore is performed automatically whenever the project is built.

6. Add a connection string for your local database to your user secrets file. Step 5 is only needed for those making changes to the asp.net core code. **If you are just working on the Angular client you don't need to do this step.**
	1. (VS2017) Open your user secrets file: `Solution Explorer -> Right-click WebApp project -> Manage User Secrets`
	2. (VS2017) Check your available local SQL Server databases: `View -> SQL Server Object Explorer`
	3. Add the following into your secrets file (this will override the database connection string in `appsettings.json`) and if necessary adjust the server in the connection string as per what you found in the previous step:
		```
		{
			"ConnectionStrings": {
				"CBA_Database": "Server=(localdb)\\ProjectsV13;Initial Catalog=CBAWebAppDb;Integrated Security=True;MultipleActiveResultSets=true;;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False;"
			}
		}
		```
	4. Using a command prompt, and from the WebApp directory, run `dotnet ef database update --context CBAContext` to create and seed your local copy of the database.

## Development servers
From the WebApp project/directory:

Run `npm start` or `ng serve --proxy-config proxy.conf.json` for the angular server. 
Navigate to `http://localhost:4200/`. The app will automatically reload if you change any of the source files. 
URLs starting with /api (e.g. `http://localhost:4200/api/users`) are proxied to the dotnet server described below.

Run `dotnet run` to run the dotnet server (local web api.) Navigate to `http://localhost:62682/swagger`.

You need both servers for the application to run correctly: therefore you will need 2 command prompts.

