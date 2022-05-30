[![CI](https://github.com/wiktoriakeller/notes-app/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/wiktoriakeller/notes-app/actions/workflows/ci.yml)
# Notes App

REST WebAPI for managing notes with React client app.

## Backend Technologies
* ASP.NET Core 6
* Entity Framework
* FluentValidation
* AutoMapper
* Hashids
* xUnit
* FluentAssertions
* Moq

## Frontend Technologies
* React
* JavaScript
* HTML5
* CSS3

## Features
* Creating, updating, deleting and filtering notes
* User registration
* User authentication and authorization using JWT tokens
* Password recovery (an email is send to the user)
* Notes tagging
* Public link generation to let the guests view our private notes (note id is hashed using a random salt)

## Endpoints
| Request method 	|                    Path                    	|                                                                    Description                                                                   	|
|:-----------------:	|:------------------------------------------:	|:------------------------------------------------------------------------------------------------------------------------------------------------:	|
|        GET        	|          /notes-api/notes/{hashId}         	|                                                      Returns a user note with the specified hashid                                                     	|
|       PATCH       	|          /notes-api/notes/{hashId}         	|                                                 Partially updates a user note with the specified hashid                                                	|
|        PUT        	|          /notes-api/notes/{hashId}         	|                                                      Updates a user note with the specified hashid                                                     	|
|       DELETE      	|          /notes-api/notes/{hashId}         	|                                                      Deletes a user note with the specified hashid                                                     	|
|        GET        	|              /notes-api/notes              	|                                                              Returns all user notes                                                              	|
|        POST       	|              /notes-api/notes              	|                                                                 Creates a new note                                                                 	|
|        GET        	|      /notes-api/notes/public/{hashId}      	|  Returns a publicly accessible note with the specified hashid 	|
|        POST       	|        /notes-api/accounts/register        	|                                                             Creates a new account                                                             	|
|        POST       	|          /notes-api/accounts/login         	|                                                               Logs a user into their account                                                              	|
|        POST       	|     /notes-api/accounts/forgot-password    	|                                       Sends an email with link that allows users to reset their password                                       	|
|        POST       	| /notes-api/accounts/reset-password/{token} 	|                                                       Allows users to reset their password                                                       	|

## Screenshots
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/home-page.png" width="800"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/register-page.png" width="800"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/incorrect-password.png" width="400"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/forgot-password-form.png" width="400"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/reading-note.png" width="500"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/editing-note.png" width="500"/>
