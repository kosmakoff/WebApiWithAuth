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
Do a `POST` request to `http://localhost:9080/api/token` with `username` and `password` in form data.

You should receive a `JSON` with token string.

### Use token to authenticate
Add an `Authorize` header to request to Index page. Value of this header must be `Bearer <tokenstring>`.

If everything is correct, you should receive a page with login information.
