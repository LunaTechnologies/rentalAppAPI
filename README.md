# Rental API Endpoints

### Auth
Operation | Route | Description | Authorization
:---: | :---: | :---: | :---:
POST | https://localhost:44321/api/Auth/register | Createa a new user | -
POST | https://localhost:44321/api/Auth/login | Login | -
POST | https://localhost:44321/api/Auth/refresh | Get the refresh token | - 

### Users
Operation | Route | Description | Authorization
:---: | :---: | :---: | :---:
GET | https://localhost:44321/api/User/getAllUsers | Get all users | Admin
