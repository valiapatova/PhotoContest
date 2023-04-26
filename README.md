# PhotoContest
Web App with C#, .NetCore, EntityFramework, REST full Api
**Application for photographers who wants to share their awesome photos and win great prices.**
# Technologies
***

- ASP .NET Core
- Entity Framework Core
- MS SQL Server
- SQL
- RESTful API
- HTML 
- CSS 
- MVC

# Database relations
***


# Administrative part
***
Admins are responsible for managing the application. Their responsibilities are:

```
1. Create new contest
2. View contests in Phase One, Phase Two and Finished
3. View list of all photographers
3. Edit Profile
```
# Users part
***
The mission of the Photo Contest app is to allow photographers to sign up to contests. 
Apart from that, every photographer should be able to:

```
1. Join contests
2. View open contests
3. Participate in contest like jury when invited for that role from admin
4. View finished contests
5. View current contests in which it participates
6. Edit Profile
7. Upload photo to contest
8. View photos of other users
```

# Areas
***
- Public - accessible without authentication
- Private - available after registration

# Public Part
***

The public part of our application should be accessible without authentication. This includes the application start page, the user login and user registration forms.

People that are not authenticated cannot see any user-specific activities, they can only fill up the registration form or log in.  

- **Login** : The newly created user can log in to our app by using email and password.
- **Registration** : It requires filling the more information about the upcoming user: username, fist name, last name, email and password. 
