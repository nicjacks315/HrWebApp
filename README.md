# HR Database & Webapp Source
- A temporal schema database designed to allow retroactive record changes while keeping audit logs of all changes. CSS files used from Microsoft's Contosa .NET/C# MVVC example, but these can be recreated easily.
The purpose of this repository is to demonstrate my familiarity with SQL based databases, full stack development, and showcase coding styles.
- Reposity Pattern & Model-View-Controller/Model-View-Viewmodel
The application follows a basic repository pattern and uses Linq to pull data into Data Transfer Objects (DTO) at the Data Access Layer (DAL). DTOs are then processed in a unit of work at the controller layer, data is validated and updated DTOs are pumped back into the local repositories, but not treated as effective data until SQL Server returns the all-clear from its ownb data validation.
- All user input is scrubbed to avoid mistakes and SQL injection attacks
- Database creation code in TSQL has been excluded for privacy reasons
