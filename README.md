# Rental API Endpoints

### Auth
| Operation | Route                                     | Description           | Authorization | Request Body                                                                                       | Response Body                                                                        |
|-----------|-------------------------------------------|-----------------------|---------------|----------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------|
| POST      | https://localhost:44321/api/Auth/register | Createa a new user    | -        | {<br>"userName": "string",<br>"email": "string",<br>"password": "string",<br>"role": "string"<br>} | -Registered<br>-Email already used<br>-Username already used<br>-Error at register       |
| POST      | https://localhost:44321/api/Auth/login    | Login                 | -             | {<br>"email": "string",<br>"password": "string"<br>}                                               | {<br>  "success": bool,<br>  "accessToken": string,<br>  "refreshToken": string<br>} |
| POST      | https://localhost:44321/api/Auth/refresh  | Get the refresh token | -             | {<br>  "accessToken": "string",<br>  "refreshToken": "string"<br>}                                 | refreshToken                                                                         |

### Users
| Operation | Route                                             | Description   | Authorization | Request Body | Response Body |
|-----------|---------------------------------------------------|---------------|---------------|--------------|---------------|
| GET       | https://localhost:44321/api/User/getAllUsers | Get all users | Admin         |      -       |       -       |
