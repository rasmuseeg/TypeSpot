using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using SendGrid;
using Typespot.Models;
using System.Security.Cryptography;
using System.Text;

namespace Typespot
{
  public class EmailService : IIdentityMessageService
  {
    public Task SendAsync(IdentityMessage message)
    {
      // Plug in your email service here to send an email.
      return ConfigSendSMTPasync(message);
    }

    private Task ConfigSendSMTPasync(IdentityMessage message)
    {
      var myMessage = new MailMessage();
      myMessage.To.Add(message.Destination);
      myMessage.From = new MailAddress(
        "no-reply@ewis.dk", // From
        "Typespot"); // DisplayName
      myMessage.Subject = message.Subject;

      // Subject and multipart/alternative Body
      string text = message.Body;
      string html = "<p>" + message.Body + "</p>";
      myMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(text, null, MediaTypeNames.Text.Plain));
      myMessage.AlternateViews.Add(AlternateView.CreateAlternateViewFromString(html, null, MediaTypeNames.Text.Html));

      // Init SmtpClient and send
      // Gmail account
      //SmtpClient smtpClient = new SmtpClient{
      //    Host = "smtp.gmail.com",
      //    Port = 587,
      //    DeliveryMethod = SmtpDeliveryMethod.Network,
      //    EnableSsl = true,
      //    UseDefaultCredentials = false,
      //    Credentials = new System.Net.NetworkCredential( "rasmuseegmoeller@gmail.com", "caseqgmwkuugohzl" )  
      //};
      // use what is written in web.config
      SmtpClient smtpClient = new SmtpClient();

      // Send the email.
      if (smtpClient != null)
      {
        smtpClient.Send(myMessage);
        return Task.FromResult(0);
      }
      else
      {
        return Task.FromResult(0);
      }
    }
  }

  public class SmsService : IIdentityMessageService
  {
    public Task SendAsync(IdentityMessage message)
    {
      // Plug in your SMS service here to send a text message.
      return Task.FromResult(0);
    }
  }

  // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
  public class ApplicationUserManager : UserManager<ApplicationUser>
  {
    private const int _passwordRequiredLength = 8;
    private const bool _passwordRequireNonLetterOrDigit = true;
    private const bool _passwordRequireDigit = true;
    private const bool _passwordRequireLowercase = true;
    private const bool _passwordRequireUppercase = true;

    public ApplicationUserManager(IUserStore<ApplicationUser> store)
        : base(store)
    {
    }

    public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options, IOwinContext context)
    {
      var manager = new ApplicationUserManager(new UserStore<ApplicationUser>(context.Get<ApplicationDbContext>()));
      // Configure validation logic for usernames
      manager.UserValidator = new UserValidator<ApplicationUser>(manager)
      {
        AllowOnlyAlphanumericUserNames = false,
        RequireUniqueEmail = true
      };

      // Configure validation logic for passwords
      manager.PasswordValidator = new PasswordValidator
      {
        RequiredLength = _passwordRequiredLength,
        RequireNonLetterOrDigit = _passwordRequireNonLetterOrDigit,
        RequireDigit = _passwordRequireDigit,
        RequireLowercase = _passwordRequireLowercase,
        RequireUppercase = _passwordRequireUppercase,
      };

      // Configure user lockout defaults
      manager.UserLockoutEnabledByDefault = true;
      manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
      manager.MaxFailedAccessAttemptsBeforeLockout = 5;

      // Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
      // You can write your own provider and plug it in here.
      manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
      {
        MessageFormat = "Your security code is {0}"
      });
      manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
      {
        Subject = "Security Code",
        BodyFormat = "Your security code is {0}"
      });
      manager.EmailService = new EmailService();
      manager.SmsService = new SmsService();
      var dataProtectionProvider = options.DataProtectionProvider;
      if (dataProtectionProvider != null)
      {
        manager.UserTokenProvider =
            new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
      }
      return manager;
    }

    public async Task<string> GenerateRandomPasswordAsync()
    {
      const string lower = @"abcdefghijklmnopqrstuvwxyz";
      const string upper = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
      const string digits = @"1234567890";
      const string nonld = @"~!@#$%^&*()-_=+[{]}\|;"":',<.>/?";

      // See note at the the top (constants)
      // using required character sets, create appropriate source 
      var source = String.Format("{0}{1}{2}{3}",
          (_passwordRequireLowercase ? lower : String.Empty),
          (_passwordRequireUppercase ? upper : String.Empty),
          (_passwordRequireDigit ? digits : String.Empty),
          (_passwordRequireNonLetterOrDigit ? nonld : String.Empty)
          );
      // sanity check, this should never occur
      if (source.Length == 0)
      {
        throw new InvalidOperationException("Source character set is empty!");
      }

      var sourceChars = source.ToCharArray();
      var data = new byte[_passwordRequiredLength];
      var crypto = new RNGCryptoServiceProvider();
      crypto.GetNonZeroBytes(data);
      var result = new StringBuilder(_passwordRequiredLength);
      foreach (var b in data)
      {
        result.Append(sourceChars[b % (sourceChars.Length)]);
      }
      var generatedPassword = result.ToString();

      // sanity check, this should never occur
      var isValid = await this.PasswordValidator.ValidateAsync(generatedPassword);
      if (!isValid.Succeeded)
      {
        throw new InvalidOperationException("Generated password failed validation!");
      }

      return generatedPassword;
    }
  }

  // Configure the application sign-in manager which is used in this application.
  public class ApplicationSignInManager : SignInManager<ApplicationUser, string>
  {
    public ApplicationSignInManager(ApplicationUserManager userManager, IAuthenticationManager authenticationManager)
        : base(userManager, authenticationManager)
    {
    }

    public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
    {
      return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
    }

    public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, IOwinContext context)
    {
      return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), context.Authentication);
    }
  }

}
