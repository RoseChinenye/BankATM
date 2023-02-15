# BANK ATM 😃👓👓

## About 👓

This is a simple implementation of Bank ATM application using ADO.NET and Microsoft SQL Server Management System.

---
## Definition of Terms 👓

**ADO.NET :**
The .NET platform defines several namespaces that allow you to interact with relational database systems. 
Collectively speaking, these namespaces are known as ADO.NET.

ADO.NET provides consistent access to data sources such as SQL Server and XML, and to data sources exposed through OLE DB and ODBC. Data-sharing consumer applications can use ADO.NET to connect to these data sources and retrieve, handle, and update the data that they contain.

**Microsoft SQL Server Management System :**
It is an integrated environment for managing any SQL infrastructure. In this project, we used SSMS to access, configure, manage, administer, and develop all components of the SQL Server.

---
## ATM SERVICES 👓

- Deposit
- Withdrawal
- Check Balance
- Transfer

N/B : All the operations done in this ATM are saved!

---
## Demo Login Credentials 👓

| Card Number | Pin |
| ----------- | ----------- |
| 1020304050 | 1234 |
| 1121314151 | 1030 |
| 1222324052 | 1935 |

---
## Steps to Run 👓

Follow the following steps to successfully run and use this application.
1. **Paste your system's server name on the Connection String**
- Open the *ATM_DAL* project
- Open the *Database* Folder
- Open the *CreateAtmDB.cs* class
- Edit the connection string and paste your system server name in the *Data Source* value of the *ConnectionString* variable.
```C#
string connectionString = (@"Data Source = (Your Server Name); Integrated Security = True; Connect Timeout = 30; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

```
- Save

2. **Install the SqlClient Data provider on the ATM_DAL Assembly**
- Open *ATM_DAL* project
- Right Click on *Dependencies* and select *Manage NuGet packages*
- Browse *Microsoft.Data.SqlClient* and install.

3. **Create the Database on your system programmatically by running the *CreateDatabase* method**
- Open *ATM_UI* project
- Open *program.cs* class
- Uncomment the *CreateAtmDB.CreateDatabase();*
```C#
CreateAtmDB.CreateDatabase();
```
- Save and Run the program
- Comment out the *CreateAtmDB.CreateDatabase();* after running it successfully.

4. **Connect the newly created Database**
- Open *ATM_DAL* project
- Open *Database* folder
- Open *AtmDbContext.cs*
- Edit the connection string and pass your newly created Database connection string to the *AtmDbContext* Constructor.
```C#
public AtmDbContext() : this(@"(paste your newly created database Connection String here)")
{

}
        
```
- Save

5. **Create the default ATM users data programmatically on your system**
- Open the program.cs class 
- Uncomment the *CreateUsers atmUsers = new CreateUsers(new AtmDbContext());* and *atmUsers.CreateAtmUsers();*
```C#
CreateUsers atmUsers = new CreateUsers(new AtmDbContext());

atmUsers.CreateAtmUsers();
```
- Save and Run the program
- Comment out the *atmUsers.CreateAtmUsers();* ater running it successfully.

6. **Create the Transaction History Table programmatically on your system**
- Open the program.cs class 
- Uncomment the *CreateTransactionHistory history = new CreateTransactionHistory(new AtmDbContext());* and *history.TransactionHistory();*
```C#
CreateTransactionHistory history = new CreateTransactionHistory(new AtmDbContext());

history.TransactionHistory();
```
- Save and Run the program
- Comment out the *CreateTransactionHistory history = new CreateTransactionHistory(new AtmDbContext());* and *history.TransactionHistory();*ater running it successfully.

7. **Run the ATM Application**
- Open the program.cs class
- Uncomment the *starter.Run();*
```C#
starter.Run();
```
- Save and Run the app

---
## Software Development Summary 👓

- Technology: C# and ADO.NET
- Data Provider: SqlClient
- SQL Environment: Microsoft SSMS
- Console App Framework: .NET7
- Project Type: Class Library
- Class Library Framework: .Net standard 2.0
- IDE: Visual Studio (Version 2022)
- Paradigm or pattern of programming: Object-Oriented Programming (OOP)

NOTE: We appreciated the use of ADO.NET to interact with the relational database with the help of SqlClient data provider.
This repo is subject to future modifications.

