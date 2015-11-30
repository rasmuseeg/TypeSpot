namespace Typespot.Migrations
{
  using System;
  using System.Data.Entity;
  using System.Data.Entity.Migrations;
  using System.Linq;
  using Microsoft.AspNet.Identity.EntityFramework;
  using Typespot.Models;

  public class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
  {
    public Configuration()
    {
      AutomaticMigrationsEnabled = true;
      ContextKey = "Typespot.Models.ApplicationDbContext";
    }

    protected override void Seed(ApplicationDbContext context)
    {
      ////  This method will be called after migrating to the latest version.
      //context.Centers.AddOrUpdate(
      //   p => p.Id,
      //   new Center { Id = 1, Name = "Krop", Description = "Retf�rdighed / Action / Kontrol\n- Action og handling frem for snak.\n- Pr�cise forklaringer, som ikke er pakket ind. \n- Selvst�ndighed og ansvar" },
      //   new Center { Id = 2, Name = "Hjerte", Description = "Anerkendelse / Ros / Personlige relationer \n- Empati og forst�else \n- Forst� dig og f� en relation\n- At finde og bruge l�sninger i samarbejde" },
      //   new Center { Id = 3, Name = "Hoved", Description = "Fakta / Viden / Intelligens\n- En objektiv og analytisk tilgang\n- Kommunikation med fakta og viden\n- At finde l�sninger igennem analyse og tanke" }
      //);

      //context.Tonalities.AddOrUpdate(
      //   p => p.Id,
      //   new Tonality { Id = 1, Name = "Kraftfuldt", Description = "" },
      //   new Tonality { Id = 2, Name = "Syngende", Description = "" },
      //   new Tonality { Id = 3, Name = "Monotomt", Description = "" }
      //);

      //context.HarmonicGroups.AddOrUpdate(
      //   p => p.Id,
      //   new HarmonicGroup { Id = 1, Name = "Kompetence", Description = @"- Tr�k p� deres viden � �sp�rg for at anerkende�!\n- Lad dem komme til orde! - p� deres viden og kompetencer.\n- Anerkend dem for at v�re kompetente, vidende og dygtige. Ros dem p� deres faglighed, eksempelvis:� Det er rigtigt godt set og godt du er velforberedt og g�r professionelt efter en l�sning." },
      //   new HarmonicGroup { Id = 2, Name = "Positivt livssyn", Description = @"- M�d dem fra en positiv vinkel!\n- Hold en �ben dagsorden, s� de oplever en fleksibel stemning.\n- Sig for eksempel: �Det skal nok g� alt sammen. Jeg ved godt, der er meget, men du er jo helt fantastisk til det du g�r, s� jeg er sikker p� at det bliver godt.�" },
      //   new HarmonicGroup { Id = 3, Name = "Udtryk", Description = @"- M�d dem fra en positiv vinkel!\n- Hold en �ben dagsorden, s� de oplever en fleksibel stemning.\n- Sig for eksempel: �Det skal nok g� alt sammen. Jeg ved godt, der er meget, men du er jo helt fantastisk til det du g�r, s� jeg er sikker p� at det bliver godt.�" }
      //);

      //context.SocialStyles.AddOrUpdate(
      //   p => p.Id,
      //   new SocialStyle { Id = 1, Name = "Pligt", Description = @"- Giv klart udtryk for, om det er en aftale, eller om det blot var et forslag!\n- Sig eksempelvis: �Her er nogle muligheder, men det er bare ideer vi kan overveje og s� kan vi tr�ffe den egentlige aftale bagefter.�\n- Sp�rg for at motivere; �Hvad ville v�re det rigtige at g�re i denne situation?�." },
      //   new SocialStyle { Id = 2, Name = "Selvsikker stil", Description = @"- Lad dem g� foran!- Anerkend deres ih�rdighed og engagement!- Sig eksempelvis; �Det kan jeg virkelig godt forst�. Hvad har du t�nkt der kan g�res?�. " },
      //   new SocialStyle { Id = 3, Name = "Afventende stil", Description = @"- Giv dem tid og plads til at danne sig et overblik og lad dem ogs� selv komme med deres input.\n- Lyt til deres input uden at omtolke det! (For ellers tier de stille)\n- Anerkend dem for deres forslag og ideer" }
      //);

      //context.Personalities.AddOrUpdate(
      //    p => p.Id,
      //    new Personality { Id = 1, Type = 1, Name = "Perfektionisten", CenterId = 1, TonalityId = 1, HarmonicGroupId = 1, SocialStyleId = 1, Description = "Type One is principled, purposeful, self-controlled, and perfectionistic." },
      //    new Personality { Id = 2, Type = 2, Name = "Hj�lperen", CenterId = 2, TonalityId = 2, HarmonicGroupId = 2, SocialStyleId = 2, Description = "Type Two is generous, demonstrative, people-pleasing, and possessive." },
      //    new Personality { Id = 3, Type = 3, Name = "Udretteren", CenterId = 2, TonalityId = 2, HarmonicGroupId = 1, SocialStyleId = 1, Description = "Type Three is adaptable, excelling, driven, and image-conscious." },
      //    new Personality { Id = 4, Type = 4, Name = "Romantikeren", CenterId = 2, TonalityId = 2, HarmonicGroupId = 3, SocialStyleId = 3, Description = "Type Four is expressive, dramatic, self-absorbed, and temperamental." },
      //    new Personality { Id = 5, Type = 5, Name = "Specialisten", CenterId = 3, TonalityId = 3, HarmonicGroupId = 1, SocialStyleId = 1, Description = "Type Five is perceptive, innovative, secretive, and isolated." },
      //    new Personality { Id = 6, Type = 6, Name = "Skeptikeren", CenterId = 3, TonalityId = 3, HarmonicGroupId = 3, SocialStyleId = 2, Description = "Type Six is engaging, responsible, anxious, and suspicious." },
      //    new Personality { Id = 7, Type = 7, Name = "Eventyren", CenterId = 3, TonalityId = 3, HarmonicGroupId = 2, SocialStyleId = 3, Description = "Type Six is engaging, responsible, anxious, and suspicious." },
      //    new Personality { Id = 8, Type = 8, Name = "Udfordreren", CenterId = 1, TonalityId = 1, HarmonicGroupId = 3, SocialStyleId = 3, Description = "Type Eight is self-confident, decisive, willful, and confrontational." },
      //    new Personality { Id = 9, Type = 9, Name = "M�gleren", CenterId = 1, TonalityId = 1, HarmonicGroupId = 2, SocialStyleId = 2, Description = "Type Nine is receptive, reassuring, complacent, and resigned." }
      //);

      //context.Roles.AddOrUpdate(
      //    p => p.Id,
      //    new IdentityRole { Id = "1", Name = "Superusers" },
      //    new IdentityRole { Id = "2", Name = "Employees" }
      //);

      context.Settings.AddOrUpdate(
          p => p.Id,
          new PropertyValue { Id = Guid.NewGuid(), Name = "AllowRegistration", Value = "false", PropertyType = "Boolean" },
          new PropertyValue { Id = Guid.NewGuid(), Name = "EmailInvitation", PropertyType = "String" },
          new PropertyValue { Id = Guid.NewGuid(), Name = "EmailForgotPassword", PropertyType = "String" }
      );
    }

    void Settings(ApplicationDbContext context)
    {

    }
  }
}
