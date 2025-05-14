
# Agri-Energy Connect

## PROG7313 Assignemnt 2

Youtube Link : https://youtu.be/rA8cegBEL_I

## Setting up the development environment

1. Install .NET SDK:

- Download and install the .NET SDK (version 7.0 or later) from the official Microsoft website: https://dotnet.microsoft.com/download

2. Install a Code Editor:

It is recommended to use either:

- Visual Studio Code: A free and open-source code editor with excellent support for C# and ASP.NET Core.  Download it from: https://code.visualstudio.com/

- Visual Studio: A powerful integrated development environment (IDE) from Microsoft.  There is a free Community edition available.  Download it from: https://visualstudio.microsoft.com/

3. Obtain the Code:

- Clone the repository containing the project to your local machine using Git:

- git clone <repository_url>
- cd <project_directory>

- Replace <repository_url> with the URL of your Git repository and <project_directory> with the name of the directory where you want to clone the repository.
## Building and Running the Prototype

1. Open the Project:

- Open the project in your code editor:

  - Visual Studio Code: Open the project folder in VS Code.

  - Visual Studio: Open the solution file (.sln) in Visual Studio.

2. Restore Dependencies:

- If the project does not automatically restore NuGet packages, restore them manually:

  - Visual Studio Code: Open the command palette (Ctrl+Shift+P or Cmd+Shift+P) and type dotnet: Restore.  Select dotnet: Restore from the list.

  - Visual Studio: Right-click on the solution in the Solution Explorer and select "Restore NuGet Packages".

3. Build the Application:

- Build the application:

  - Visual Studio Code: Use the build task (Ctrl+Shift+B or Cmd+Shift+B) or run the following command in the terminal: dotnet build

  - Visual Studio: Go to Build -> Build Solution (or press Ctrl+Shift+B).

4. Run the Application:

- Run the application:

  - Visual Studio Code: If you have a launch configuration set up, you can press F5 to start debugging, or use the following command in the terminal: dotnet run

  - Visual Studio: Press F5 to start debugging, or Ctrl+F5 to run without debugging.

5. View the Application:

- Once the application is running, it will display a URL in the console (e.g., http://localhost:5000).  Open this URL in your web browser to view the prototype.
## User roles and system functionalities

### User roles
1. Employee = Admin
2. Farmer = User

### System functionalities

- Employee
  - Can login
  - Add new farmer profiles with essential details
  - View and filter a comprehensive list of products from any farmer based on criteria such as date range and product type.

- Farmer
  - Can login
  - Product addition feature where farmers can add new products with the following details name, category, description and production date.
  - View their products
  - Edit their products
  - Delete their products 
## References

SKIPSHARPIERO ENTEPRISES : Biogas: Definition, Uses, Types & Sources , 30/08/2023 , https://shapiroe.com/blog/biogas-generation-guide/

Google: Gemini , 2024 , https://g.co/gemini/share/597f14a6ee9f

Stack Overflow : How do you get the footer to stay at the bottom of a Web page? , 23/09/2024 , https://stackoverflow.com/questions/42294/how-do-you-get-the-footer-to-stay-at-the-bottom-of-a-web-page

jsDelivr : bootswatch , ND , https://www.jsdelivr.com/package/npm/bootswatch?tab=files&path=dist%2Fbrite

mineral resources & energy : Wind-Power , ND , https://www.dmre.gov.za/energy-resources/energy-sources/renewable-alternative-fuels/wind-power

