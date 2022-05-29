[![CI](https://github.com/wiktoriakeller/notes-app/actions/workflows/ci.yml/badge.svg?branch=main)](https://github.com/wiktoriakeller/notes-app/actions/workflows/ci.yml)
# Notes App

REST WebAPI for managing notes with react client app.

## Backend Technologies
* ASP.NET 6
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
* Creating, updating, filtering and reading notes
* User registration
* User authentication and authorization using jwt tokens
* Password recovery using user email
* Notes tagging
* Public link generation to let guests view our private notes (note id is hashed using random salt)

## Endpoints
| Request<br>method 	|                    Path                    	|                                                                    Description                                                                   	|
|:-----------------:	|:------------------------------------------:	|:------------------------------------------------------------------------------------------------------------------------------------------------:	|
|        GET        	|          /notes-api/notes/{hashId}         	|                                                      Returns user note with specified hashid                                                     	|
|       PATCH       	|          /notes-api/notes/{hashId}         	|                                                 Partially updates user note with specified hashid                                                	|
|        PUT        	|          /notes-api/notes/{hashId}         	|                                                      Updates user note with specified hashid                                                     	|
|       DELETE      	|          /notes-api/notes/{hashId}         	|                                                      Deletes user note with specified hashid                                                     	|
|        GET        	|              /notes-api/notes              	|                                                              Returns all user notes                                                              	|
|        POST       	|              /notes-api/notes              	|                                                                 Creates new note                                                                 	|
|        GET        	|      /notes-api/notes/public/{hashId}      	| Publicly accessible endpoint that returns note <br>with specified hashid (this hashid is different <br>from the ones used in previous endpoints) 	|
|        POST       	|        /notes-api/accounts/register        	|                                                             Creates new user account                                                             	|
|        POST       	|          /notes-api/accounts/login         	|                                                               Logging into account                                                               	|
|        POST       	|     /notes-api/accounts/forgot-password    	|                                       Sends email with link that allows users to <br>reset their passwords                                       	|
|        POST       	| /notes-api/accounts/reset-password/{token} 	|                                                       Allows users to reset their password                                                       	|

## Screenshots
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/login-page.png" width="800"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/register-page.png" width="800"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/incorrect-password.png" width="400"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/forgot-password-form.png" width="400"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/password-recovery-page.png" width="800"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/home-page.png" width="800"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/reading-note.png" width="500"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/creating-new-note.png" width="500"/>
<img src="https://github.com/wiktoriakeller/notes-app/blob/main/images/editing-note.png" width="500"/>
