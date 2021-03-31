# HR Database & Webapp Source
- The purpose of this repository is to demonstrate my familiarity with SQL based databases, full stack development, and showcase coding styles
- Code  contains a temporal database schema designed to allow retroactive record changes while keeping audit logs of all changes
- CSS files used from Microsoft's Contosa .NET/C# MVVC example, but these can be recreated easily.
- All user input is scrubbed to avoid mistakes and SQL injection attacks
- Database creation code in TSQL has been excluded for privacy reasons
## Reposity Pattern & Model-View-Controller/Model-View-Viewmodel
The application follows a basic repository pattern and uses Linq to pull data into Data Transfer Objects (DTO) at the Data Access Layer (DAL). DTOs are then processed in a unit of work at the controller layer, data is validated and updated DTOs are pumped back into the local repositories, but not treated as effective data until SQL Server returns the all-clear from its own data validation.
- MVVM is included here because the design is closely related to MVC. Since the application is mostly a basic Create-Read-Update-Delete (CRUD) application, controller methods were mostly empty, or at least only consisted of one or two lines of logic. For read-only pages, a sort of MVVM pattern was followed to eliminate excessive empty controller methods.
## Contents
- The contents of this reposity reflects the code and systems built by myself in a larger collaborative Visual Studio solution. It does not represent a solution in its entirety.

| Folder | Description |
| --- | --- |
| Models | Entity classes aggregating database tables into non-abstracted information.  |
| Views | CSHTML webpages; The web interface of the application |
| Controllers | Methods for retrieving data, processign changes, and submitting updated data and records back to repositores |
| DTO | Data Transfer Objects; Structs with complementary mapping functions used purely to move data from Linq queries to models |
| DAL | Data Access Layer; Responsible for pulling data from repositories into DTOs, but with the primary responsiblity of organizing data chronologically and viewing at a specified point-in-time as opposed to only-immediately. |
| SQL | Stored procedures and syncing scripts used to deploy this solution in parallel to an existing SQL-based source-of-truth. |
