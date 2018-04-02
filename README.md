# Overview
Tinix is a micro blogging solution for .Net Core, compatible with the dotnet engine storage file format. 

# Feature hightlights
- tiny application that doesn't need a database , blog posts are stored as xml files on disk.

- easy csutomization, the layout is based on boostrap 4. Support for themes (using custom css files + layouts).

- runs on Linux, Windows and MacOS


# Quickstart 
- download first release, deploy it on Kestrel or IIS .

- generate your password hash (dotnet run PasswordHasher).

- edit  appsettings.josn and enter your admin area user/password and salt. Also the blog details : titles, subtitle and main layout page

- acess the admin area at /admin/login and start posting 

