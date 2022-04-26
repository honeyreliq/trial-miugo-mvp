# Macos Project Setup
See `README.md` as reference.

>## .NET Core 3.1
>---------------------------------------
* **1: Install .NET Core 3.1:**
To use dotnet commands, must [download](https://dotnet.microsoft.com/download/dotnet-core/thank-you/sdk-3.1.301-macos-x64-installer) and install .Net Core 3.1 for mac

* **2: Install Visual Studio for mac:**
This is the IDE we should use to run the app and debug.
Download [Visual Studio 2019 for mac](https://visualstudio.microsoft.com/thank-you-downloading-visual-studio-mac/?sku=communitymac&rel=16#)

    **Note:** Rider from JetBrains also works just fine, it requires a license after 30 days of evaluation.
* **3: Setup App default user:**

  - **3.1 Create this json file:** `{user_dir}/.microsoft/usersecrets/2e28c44d-15c4-43ea-a8f3-53fe7568189e/secrets.json`
  - **3.2 Ask for the content of the file.**
* **4:  Set SendGrid [macos environment variable](https://medium.com/@youngstone89/setting-up-environment-variables-in-mac-os-28e5941c771c):**

    Set the environment variable in `.bash_profile`, `.szhrc` and `.bashhrc`. **(ask for the secret key)**:

    `export SENDGRID_API_KEY="{SECRET}"`

>## NodeJS
>---------------------------------------
Since mac user should use **Visual Studio 2019 for mac**, and it does not recognize nodeJs versions managed by **NVM**, we should *[download](https://nodejs.org/en/download/) and install NodeJS (>=12.16.X) Natively*.


>## DataBase (MS SQL 2017)
>---------------------------------------
MS SQL cannot be installed in macos natively, so it is needed to download a docker container.

* **1: Download docker image:**

     sudo docker pull mcr.microsoft.com/mssql/server:2017-latest

* **2: Create SQL Server (*It runs as soon it is created*):**

    sudo docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=reallyStrongPwd123" -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2017-latest

* **3: Update DB String connections in (Do not commit these changes):**
  `../IUGOCare.API/appsettings.json`
  `../UGOCare.API/appsettings.Integration.json`
  `../IUGOCare.Application.IntegrationTests/appsettings.integration.json`

    <ins>String Connection:</ins>
`Server=127.0.0.1,1433;Database=IUGOCare;User Id=SA;Password=reallyStrongPwd123;`

* **4: Create DataBase:**
Once the SQL (docker) is up and started, run the following commands *(the cmd window should point to the root directory):*
    ```
    dotnet ef database update --project ./IUGOCare.Infrastructure/ --context ApplicationDbContext --startup-project ./IUGOCare.API/
    ```
    ```
    dotnet ef database update --project ./IUGOCare.Audit/ --context AuditDbContext --startup-project ./IUGOCare.API/
    ```
* **5: IDE for DB:**
Windows has its **SQL Server Management Studio**, however, there are no installers for macos nor Linux users, Windows users can use windows authentication because of that. Mac and Linux users have to configure the connection string, the User, and Password as per the docker container requirements.

    We can use [dbeaver](https://dbeaver.io/download/) as an option, which works just fine.

    Another useful option is [SQL Server (mssql)](https://marketplace.visualstudio.com/items?itemName=ms-mssql.mssql) extension for VS Code.

**Useful commands:**
 - check dockers: `docker ps -a`
 - start server (<ins>every time the pc starts, we should run this command</ins>): `docker start sql1`
 - stop server: `docker stop sql1`


>## Run and Debug
>---------------------------------------
Once everything is configured and in place, we can run the app from **Visual Studio for Mac** clicking the "<ins>Play</ins>" button.
The build process may take a while, once it is completed, **Chrome** browser will open [http://localhost:65045/](http://localhost:65045/) authomatically, ... we are ready to work.

>## Known Issues
>---------------------------------------
**localhost:65045 is in use:**
It might happen that the application stops running for some reason, if so, and we try to run the app again, we could see an error saying that `localhost:65045` is in use.
<ins>Solution:</ins>
 - Look the PIDs: `sudo lsof -i:65045`
 - Kill all related PIDs: `sudo kill -9 {PID}
`
