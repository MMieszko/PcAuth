# PcAuth

### Quickstart - .NET Core


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

The last step is handling unauthorize exception. This can be ommited if you have created your own exception middleware or so:
```csharp
  app.HandleAuthException(async (ctx, exc) =>
            {
                ctx.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await ctx.Response.WriteAsync(exc.Message);

            });
```

### Quickstart - .NET Framework

To be added.


### Customizing - .NET Core

To be added.


### Customizing - .NET Framework

To be added.


