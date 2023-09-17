# AssetTracking2

# Asset Tracking 2 installation instructions:
1 - Download AssetTracking2 folder.

2 - Open AssetTracking2 with Visual Studio 2022.

3 - Check if you have all needed framework pakages by click on Tools > NuGet Package Manager > Manage Packages for Solution then click on Installed tap you should see three packages EntityFrameworkCore, EntityFrameworkCore.Tools and EntityFrameworkCore.SqlServer so if not installed so please install it by Browse and search be sure to install version 6.0.21.

4 - Check MyDbContext.cs file it should have a database (Asset) please update database by terminal command update- database (to open terminal command click on Tools > NuGet Package Manager > Package Manager Console.

5 - Install Migrations by command terminal write add-migration AddingAssetsAndOfficesTables (click enter)
then update-database (ckick enter) go to database and check, open command terminal write add-migration SeedingOfficesData (click enter) then update-database after installing, go to database and check if all tables is loaded correct.

6 - Run the App and enjoy.

Note: When you will exit and save the app by choosing number 5 in the app so your report saved into drive C: folder name AssetTracking2 like this C:\AssetTracking2.
Enjoy 😎
