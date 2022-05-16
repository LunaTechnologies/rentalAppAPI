# Rental API Endpoints

### Auth
| Operation | Route                                     | Description           | Authorization | Request Body                                                                                       | Response Body                                                                        |
|-----------|-------------------------------------------|-----------------------|---------------|----------------------------------------------------------------------------------------------------|--------------------------------------------------------------------------------------------|
| POST      |{{api_url}}/api/Auth/register | Createa a new user    | -        | {<br>"userName": "string",<br>"email": "string",<br>"password": "string",<br>"role": "string"<br>} | -Registered<br>-Email already used<br>-Username already used<br>-Error at register       |
| POST      | {{api_url}}/api/Auth/login    | Login                 | -             | {<br>"email": "string",<br>"password": "string"<br>}                                               | {<br>  "success": bool,<br>  "accessToken": string,<br>  "refreshToken": string<br>} |
| POST      | {{api_url}}/api/Auth/refresh  | Get the refresh token | -             | {<br>  "accessToken": "string",<br>  "refreshToken": "string"<br>}                                 | refreshToken                                                                         |

### Users
| Operation | Route                                             | Parameter  | Authorization | Request Body | Response Body |
|-----------|---------------------------------------------------|---------------|---------------|--------------|---------------|
| GET       | {{api_url}}/api/User/getAllUsers |  | Admin         |      -       |       -       |
| DELETE    | {{api_url}}/api/User/removeUser    |                | -             | {<br>"userName":"string"<br>} |
| GET    | {{api_url}}/api/User/emailExist    |   email               | -             | |false/true |
| GET    | {{api_url}}/api/User/usernameExist    |   username               | -             |  |false/true |


## Service

| Operation | Route                                             | Parameter   | Authorization | Request Body | Response Body |
|-----------|---------------------------------------------------|---------------|---------------|--------------|---------------|
| POST   | {{api_url}}/api/Service/CreateService    |                | -             | {"title": "string","description":"string","phoneNumber": "string","price": 0,"username": "string","servType": "string","pictures": [{"path": "string"}]}| identificationString |
| DELETE    | {{api_url}}/api/Service/DeleteService    | IdentificationString              | -             | | success/Service Doesn't exist |
| GET    | {{api_url}}/api/Service/GetServiceByIdentificationString   | IdentificationString              | -             | | {"title": "smecher","description": "string","phoneNumber": "string","price": int,"username": "string","servType": "string","pictures": [{"path": "string"}]}<br>/<br>Service doesn't exist|

