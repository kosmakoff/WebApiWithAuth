# WebApiWithAuth
Playing around with ASP.NET 5 MVC Web API Auth

## How to...

### Setup Google Auth
Use [Google Console](https://console.developers.google.com/apis/dashboard) to create the OAuth ID. Next, allow the Google+ API for that account. Finally, provide your `ClientId` and `Secret` values to secrets file using commands:

```
dotnet user-secrets add "Auth:Google:ClientId" <your-client-id-here>
dotnet user-secrets add "Auth:Google:Secret" <your-secret-here>
```

After that was done you will be able to link your locally created user to your google account.

### Create the user
Just follow the UI

### Grab the bearer token
Do a form-encoded `POST` request to `http://localhost:9080/connect/token` with `username`, `password` and other parameters in form data. For example:

```
username:<your registered user name>
password:<your registered user password>
grant_type:password
client_id:api_client
client_secret:client_secret
```

Note: 3 last parameters are hardcoded in the source code. See `Config.cs`

You should receive a `JSON` with token string in parameter `access_token`.

### Use token to authenticate
Add an `Authorize` header to request API. Value of this header must be `Bearer <access token>`.

The only secured API in demo project is at `/api/values/{id}`.

If everything is correct, you should receive a page with login information.
