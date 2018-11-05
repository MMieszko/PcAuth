# PcAuth

### Quickstart - .NET Core

Package avialalbe at [NuGet](https://www.nuget.org/packages/PortaCapena.Authentication.NetCore/)

> Install-Package PortaCapena.Authentication.NetCore -Version 1.0.1


As the authentication is based on roles first we have to define roles, requirement and handler i.e:

* Role

 ```csharp
    public class AdminRole : Role
    {
        public override object Value => 1;

        public override string ToString() => Value.ToString();

    }
 ```
* Identity handler

 ```csharp
    public class AdminIdentityHandler : PcIdentityHandler<AdminRole>
    {
        //base methods can be overriden
    }
 ```

* Requirement

```csharp
   public class AdminRequirement : PcIdentityRequirement<AdminRole>
   {
   }
```

Once we have defined the example role its time to register authentication in **Startup.cs** class.

First step is to set IdentityRequirements to *ConfigureServices* method as below. Keep it mind this has to be set before AddMvc() extensions method:
```csharp
services.SetIdentityRequirements<AdminRole, AdminRequirement, AdminIdentityHandler>(nameof(AdminRequirement));
```

Second step is to configure the TokenOptions and regiester identity middleware in **Configure** method as below:

```csharp
  app.SetIdentityMiddleware<PcIdentityMiddleware>(new TokenOptionsBuilder()
                .SetTokenName("access_token")
                .SetSecretKey("this is my custom Secret key for authnetication")
                .SetExpiration(TimeSpan.FromMinutes(15))
                .SetAutoRefresh(false)
                .Build());
```

The last step is handling unauthorize exception. This can be omitted if you have created your own exception middleware or so:
```csharp
  app.HandleAuthException(async (ctx, exc) =>
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await ctx.Response.WriteAsync(exc.Message);

            });
```

### Customizing - .NET Core

You can define custom middleware deriving from **PcIdentityMiddleware**
There are two method available to override:
* SetClaimsPrincipalAsync
 
 This method is called after authentication token was sucesfully validated. As a parameter of the method accepts generated **ClaimsPrincipal** instance. The method body sets HttpContext.User with given claims and insert to HttpContext.Items["UserId"] user id claim.
 
* RefreshTokenAsync

While creating TokenOptions and AutoRefresh is set to true then the method is being executed. As a parameter method accept newly generated token. The method adds new header to the response with name given while building token. 

You can also override methods when creating role identity handler derived from **PcIdentityHandler**

* HandleRequirementAsync

The method is responsible to decide if given principals in middleware are valid for current role.

* OnUnauthorizedAsync

The method is called when process from above method failed due to wrong role. The method throws **AuthException**.


### Quickstart - .NET Framework

To be added.

### Customizing - .NET Framework

To be added.


