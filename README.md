# WebApiWithAuth
Playing around with ASP.NET 5 MVC Web API Auth

## How to...

### Create the user
Just follow the UI

### Grab the bearer token
Do a `POST` request to `http://localhost:5000/api/token` with `username` and `password` in form data.

You should receive a `JSON` with token string.

### Use token to authenticate
Add an `Authorize` header to request to Index page. Value of this header must be `Bearer <tokenstring>`.

If everything is correct, you should receive a page with login information.
