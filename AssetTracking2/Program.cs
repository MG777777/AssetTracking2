using AssetTracking2;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.X86;
Console.ForegroundColor = ConsoleColor.Cyan;
string str = "--------Welcome to AssetTracking 2--------\n";
Console.SetCursorPosition((Console.WindowWidth - str.Length) / 2, Console.CursorTop);
Console.WriteLine(str);
Console.ResetColor();
Console.ForegroundColor = ConsoleColor.DarkYellow;
Console.WriteLine("Note: In this version you will be able to add assets to the database and print the report in a separate file.");
Console.ResetColor();
Console.ForegroundColor = ConsoleColor.Green;
Console.WriteLine("\nPlease follow the steps to create your Assets tracking.\n");

while (true)
{
    Console.ForegroundColor = ConsoleColor.Blue;
    Console.WriteLine("1. List all Assets\n");
    Console.WriteLine("2. Add a new Asset\n");
    Console.WriteLine("3. Edit Asset\n");
    Console.WriteLine("4. Delete Asset\n");
    Console.WriteLine("5. Exit application and Save Assets Report to file");
    Console.ResetColor();
    MyDbContext Context = new MyDbContext();
    Asset asset1 = new Asset();
    List<Asset> assets = new List<Asset>();

    Console.Write("\nEnter your choice: ");
    string choice = Console.ReadLine();
    if (choice.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        SaveAsset(Context);
        break;
    }
    int choiceNumber;
    if (!int.TryParse(choice, out choiceNumber))
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid choice. Please enter a number or 'exit'.");
        Console.ResetColor();
        continue;
    }
    switch (choiceNumber)
    {
        case 1:
            ListAsset(Context); //Select all records from database
            break;
        case 2:
            AddAsset(Context, asset1); // Insert records into database
            break;
        case 3:
            EditAsset(Context); // Update records in database
            break;
        case 4:
            RemoveAsset(Context); // Remove records from database
            break;
        case 5:
            SaveAsset(Context); // Exit and save to file
            return;
        default:
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid choice. Please try again.");
            Console.ResetColor();
            break;
    };
}
static void AddAsset(MyDbContext context, Asset asset1) // To add a Asset into database 
{
    Console.Write("Enter Asset Type: ");
    string assetName = Console.ReadLine();
    Console.Write("Enter Asset Brand: ");
    string assetBrand = Console.ReadLine();
    Console.Write("Enter Asset Model: ");
    string assetModel = Console.ReadLine();
    Console.Write("Enter Asset Office: ");
    string assetOffice = Console.ReadLine().Trim();
    Console.Write("Purchase Date (YYYY/MM/DD): ");
    DateTime assetPurchaseDate = DateTime.Parse(Console.ReadLine());
    Console.Write("Price in USD: ");
    double assetPriceinUsd = double.Parse(Console.ReadLine());
    double usdToSekRate = 11.11;
    double usdToEurRate = 0.93;
    double localPriceTodaySek = assetPriceinUsd * usdToSekRate;
    double localPriceTodayEur = assetPriceinUsd * usdToEurRate;
    double localPriceTodayUsd = assetPriceinUsd;
    
    Office myOffice = context.Offices.Include(x => x.Assets).FirstOrDefault(x => x.Name.Equals(assetOffice));
    if (assetOffice == "Sweden")
    {
        Office MyOffice = context.Offices.Include(x => x.Assets).SingleOrDefault(x => x.Id == 1);
        asset1.Localpricetoday = localPriceTodaySek;
    }
    else if (assetOffice == "Spain")
    {
        Office MyOffice = context.Offices.Include(x => x.Assets).SingleOrDefault(x => x.Id == 2);
        asset1.Localpricetoday = localPriceTodayEur;
    }
    else if (assetOffice == "Usa")
    {
        Office MyOffice = context.Offices.Include(x => x.Assets).SingleOrDefault(x => x.Id == 3);
        asset1.Localpricetoday=localPriceTodayUsd;
    }
    if (myOffice != null)
    {
        asset1.Name = assetName;
        asset1.Brand = assetBrand;
        asset1.Model = assetModel;
        asset1.PurchaseDate = assetPurchaseDate;
        asset1.PriceinUsd = assetPriceinUsd;
        
        myOffice.Assets.Add(asset1); // Add the Asset to the specific Office's Assets collection
        context.SaveChanges();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Asset added successfully.");
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid office name. Asset was not added.");
    }
    Console.ResetColor();
}
static void ListAsset(MyDbContext context)
{
    DateTime currentDate = DateTime.Now;
    List<Asset> Result = context.Assets.Include(x => x.Office).ToList();

    Result.Sort((a1, a2) => // Sort the assets by office and then purchase date
    {
        int officeComparison = string.Compare(a1.Office.Name, a2.Office.Name);
        if (officeComparison == 0)
        {
            return DateTime.Compare(a1.PurchaseDate, a2.PurchaseDate);
        }
        return officeComparison;
    });
    Console.WriteLine("\nAssets in Database:");
    Console.ForegroundColor = ConsoleColor.DarkCyan;
    Console.WriteLine("ID".PadRight(10) + "Type".PadRight(10) + "Brand".PadRight(10) + "Model".PadRight(10) + "Office".PadRight(10) + "Purchase Date".PadRight(15) + "Price in USD".PadRight(15) + "Currency".PadRight(10) + "Local Price Today".PadRight(10));
    Console.WriteLine("---".PadRight(10) + "----".PadRight(10) + "-----".PadRight(10) + "-----".PadRight(10) + "------".PadRight(10) + "-------------".PadRight(15) + "------------".PadRight(15) + "--------".PadRight(10) + "-----------------".PadRight(10));
    Console.ResetColor();
    foreach (Asset asset in Result)
    {
        DateTime threeYearsFromNow = currentDate.AddYears(3);
        DateTime threeMonthsAgo = currentDate.AddMonths(-3);
        DateTime sixMonthsAgo = currentDate.AddMonths(-6);
       
        if (asset.PurchaseDate > threeMonthsAgo && asset.PurchaseDate < threeYearsFromNow)// Check if the date is less than 3 months away from 3 years
        {
            Console.ForegroundColor = ConsoleColor.Red;  
        }
        else if (asset.PurchaseDate > sixMonthsAgo && asset.PurchaseDate < threeYearsFromNow)// Check if the date is less than 6 months away from 3 years
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
        }
        
        Console.Write($"{asset.Id.ToString().PadRight(10)}{asset.Name.PadRight(10)} {asset.Brand.PadRight(10)} {asset.Model.PadRight(10)} {asset.Office.Name.PadRight(10)} {asset.PurchaseDate.ToShortDateString().PadRight(15)} {asset.PriceinUsd.ToString().PadRight(15)} {asset.Office.Currency.PadRight(10)} {asset.Localpricetoday.ToString()}\n");

        Console.ResetColor();
    }
    Console.WriteLine();
}
static void EditAsset(MyDbContext Context) // Edit Asset
{
    ListAsset(Context); // List Assets.
    List<Asset> Result = Context.Assets.Include(x => x.Office).ToList();
    Console.Write("Enter the ID of the Asset to edit: ");
    if (int.TryParse(Console.ReadLine(), out int Id))
    {
        Asset assetToEdit = Context.Assets.FirstOrDefault(asset => asset.Id == Id);
        if (assetToEdit != null)
        {
            Console.Write("Enter new Asset Type: ");
            string assetName = Console.ReadLine();
            Console.Write("Enter new Asset Brand: ");
            string assetBrand = Console.ReadLine();
            Console.Write("Enter new Asset Model: ");
            string assetModel = Console.ReadLine();
            Console.Write("Enter new Office: ");
            string assetOffice = Console.ReadLine().Trim();
            Console.Write("Enter a new Purchase Date: ");
            DateTime assetPurchaseDate = DateTime.Parse(Console.ReadLine());
            Console.Write("Enter new Price in USD: ");
            double assetPriceinUsd = double.Parse(Console.ReadLine());
            double usdToSekRate = 11.11;
            double usdToEurRate = 0.93;
            double localPriceTodaySek = assetPriceinUsd * usdToSekRate;
            double localPriceTodayEur = assetPriceinUsd * usdToEurRate;
            double localPriceTodayUsd = assetPriceinUsd;
            Office myOffice = Context.Offices.Include(x => x.Assets).FirstOrDefault(x => x.Name.Equals(assetOffice));
            if (assetOffice == "Sweden")
            {
                Office MyOffice = Context.Offices.Include(x => x.Assets).SingleOrDefault(x => x.Id == 1);
                assetToEdit.Localpricetoday = localPriceTodaySek;
            }
            else if (assetOffice == "Spain")
            {
                Office MyOffice = Context.Offices.Include(x => x.Assets).SingleOrDefault(x => x.Id == 2);
                assetToEdit.Localpricetoday = localPriceTodayEur;
            }
            else if (assetOffice == "Usa")
            {
                Office MyOffice = Context.Offices.Include(x => x.Assets).SingleOrDefault(x => x.Id == 3);
                assetToEdit.Localpricetoday = localPriceTodayUsd;
            }

            if (myOffice != null)
            {
                assetToEdit.Name = assetName;
                assetToEdit.Brand = assetBrand;
                assetToEdit.Model = assetModel;
                assetToEdit.PriceinUsd = assetPriceinUsd;
                assetToEdit.PurchaseDate = assetPurchaseDate;
                assetToEdit.OfficeId = myOffice.Id;

                Context.Assets.Update(assetToEdit);
                Context.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Asset edited successfully.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Invalid Price format. Editing failed.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Asset with index " + Id + " not found.");
            Console.ResetColor();
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid input. Please enter a valid ID number.");
        Console.ResetColor();
    }
}
static void RemoveAsset(MyDbContext Context)// Delete Asset
{
    ListAsset(Context);
    List<Asset> Result = Context.Assets.Include(x => x.Office).ToList();

    Console.Write("Enter the ID of the Asset to Delete: ");
    if (int.TryParse(Console.ReadLine(), out int Id))
    {
        Asset assetToRemove = Context.Assets.FirstOrDefault(asset => asset.Id == Id);
        if (assetToRemove != null) 
        {
            Context.Assets.Remove(assetToRemove);
            Context.SaveChanges();
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Asset Deleted successfully.");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Invalid index. Please enter a valid ID number.");
            Console.ResetColor();
        }
    }
    else
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Invalid input. Please enter a valid ID number.");
        Console.ResetColor();
    }
}
static void SaveAsset(MyDbContext Context) // Save Asset Report to the file
{
    
    string dataFilePath = @"C:\AssetTracking2\AssetsReport.txt";
    string directoryPath = Path.GetDirectoryName(dataFilePath); // Check if the directory exists, and if not, create it
    if (!Directory.Exists(directoryPath))
    {
        Directory.CreateDirectory(directoryPath);
    }
    List<Asset> Result = Context.Assets.Include(x => x.Office).ToList();
    double totalUsdSum = 0.0;
    using (StreamWriter writer = new StreamWriter(dataFilePath)) //To write Assets report into a text file.
    {
        writer.WriteLine("Type".PadRight(10) + "Brand".PadRight(12) + "Model".PadRight(10) + "Office".PadRight(10) + "Purchase Date".PadRight(15) + "Price in USD".PadRight(15) + "Currency".PadRight(10) + "Local Price Today".PadRight(10));
        writer.WriteLine("----".PadRight(10) + "-----".PadRight(12) + "-----".PadRight(10) + "------".PadRight(10) + "-------------".PadRight(15) + "------------".PadRight(15) + "--------".PadRight(10) + "-----------------".PadRight(10));
        foreach (Asset asset in Result)
        {
            totalUsdSum += asset.PriceinUsd;
            writer.Write($"{asset.Name.ToString().PadRight(10)} {asset.Brand.PadRight(12)} {asset.Model.PadRight(10)} {asset.Office.Name.PadRight(10)} {asset.PurchaseDate.ToShortDateString().PadRight(15)} {asset.PriceinUsd.ToString().PadRight(15)} {asset.Office.Currency.PadRight(10)}{asset.Localpricetoday.ToString()}\n");
        }
        writer.WriteLine("\n\n---------------------------------");
        writer.WriteLine($"Total Assets: {Result.Count}");
        writer.WriteLine("\n\n---------------------------------");
        writer.WriteLine($"Total Price in USD : {totalUsdSum}");
        ////////////////////////////////////////////////////////// Create separate lists for assets that meet different End life
        DateTime currentDate = DateTime.Now;
        DateTime threeYearsFromNow = currentDate.AddYears(3);
        DateTime threeMonthsAgo = currentDate.AddMonths(-3);
        DateTime sixMonthsAgo = currentDate.AddMonths(-6);
        List<Asset> assetsLessThan3Months = Result.Where(asset =>
            asset.PurchaseDate > threeMonthsAgo && asset.PurchaseDate < threeYearsFromNow
        ).ToList();
        List<Asset> assetsLessThan6Months = Result.Where(asset =>
            asset.PurchaseDate > sixMonthsAgo && asset.PurchaseDate < threeYearsFromNow
        ).ToList();
        /////////////////////////////////////////////////////////
        if (assetsLessThan3Months.Count > 0)// Report for assets less than 3 months away from 3 years
        {
            writer.WriteLine("\n\nAssets have a Purchase Date less than 3 months away from 3 years");
            writer.WriteLine("**************************************************************\n");
            PrintAssetTable(writer, assetsLessThan3Months);
        }
        if (assetsLessThan6Months.Count > 0)// Report for assets less than 6 months away from 3 years
        {
            writer.WriteLine("\n\nAssets have a Purchase Date less than 6 months away from 3 years");
            writer.WriteLine("**************************************************************\n");
            PrintAssetTable(writer, assetsLessThan6Months);
        }
        void PrintAssetTable(StreamWriter writer, List<Asset> assets)// This method to save reports in the same file
        {
            writer.WriteLine("Type".PadRight(10) + "Brand".PadRight(12) + "Model".PadRight(10) + "Office".PadRight(10) + "Purchase Date".PadRight(15) + "Price in USD".PadRight(15) + "Currency".PadRight(10) + "Local Price Today".PadRight(10));
            writer.WriteLine("----".PadRight(10) + "-----".PadRight(12) + "-----".PadRight(10) + "------".PadRight(10) + "-------------".PadRight(15) + "------------".PadRight(15) + "--------".PadRight(10) + "-----------------".PadRight(10));

            foreach (Asset asset in assets)
            {
                writer.Write($"{asset.Name.ToString().PadRight(10)} {asset.Brand.PadRight(12)} {asset.Model.PadRight(10)} {asset.Office.Name.PadRight(10)} {asset.PurchaseDate.ToShortDateString().PadRight(15)} {asset.PriceinUsd.ToString().PadRight(15)} {asset.Office.Currency.PadRight(10)}{asset.Localpricetoday.ToString()}\n");
            }
        }
        Console.WriteLine();
    }
    Console.ForegroundColor = ConsoleColor.Green;
    Console.WriteLine("The Assets Report saved to file.");
    Console.ResetColor();
}
