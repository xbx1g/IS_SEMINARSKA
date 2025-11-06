using AutoServis.Data;
using AutoServis.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace AutoServis.Data;

public static class DbInitializer
{
    public static void Initialize(AutoServisContext context)
    {
        context.Database.EnsureCreated();
        if (context.Stranke.Any())
        {
            return;
        }
        var stranke = new Stranka[]
        {
            new Stranka{Ime="Janez",Priimek="Novak",Telefon="041-123-456",Email="janez.novak@email.com",DatumRegistracije=DateTime.Parse("2023-01-15")},
            new Stranka{Ime="Marija",Priimek="Horvat",Telefon="040-987-654",Email="marija.horvat@email.com",DatumRegistracije=DateTime.Parse("2023-02-20")},
            new Stranka{Ime="Marko",Priimek="Kranjec",Telefon="031-555-888",Email="marko.kranjec@email.com",DatumRegistracije=DateTime.Parse("2023-03-10")},
            new Stranka{Ime="Ana",Priimek="Kovač",Telefon="051-222-333",Email="ana.kovac@email.com",DatumRegistracije=DateTime.Parse("2023-04-05")}
        };
        context.Stranke.AddRange(stranke);
        context.SaveChanges();
        var vozila = new Vozilo[]
        {
            new Vozilo{VoziloID=1001,Znamka="Volkswagen",Model="Golf",Registracija="LJ-AB-123",Letnik=2020},
            new Vozilo{VoziloID=1002,Znamka="Audi",Model="A4",Registracija="LJ-CD-456",Letnik=2019},
            new Vozilo{VoziloID=1003,Znamka="BMW",Model="3 Series",Registracija="LJ-EF-789",Letnik=2021},
            new Vozilo{VoziloID=1004,Znamka="Renault",Model="Clio",Registracija="LJ-GH-012",Letnik=2018},
            new Vozilo{VoziloID=1005,Znamka="Ford",Model="Focus",Registracija="LJ-IJ-345",Letnik=2022}
        };
        context.Vozila.AddRange(vozila);
        context.SaveChanges();

        var mehaniki = new Mehanik[]
        {
            new Mehanik { Ime = "Peter", Priimek = "Kovač", Specializacija = "Motor", Telefon = "041-111-222", Email = "peter@servis.si" },
            new Mehanik { Ime = "Miha", Priimek = "Novak", Specializacija = "Podvozje", Telefon = "041-333-444", Email = "miha@servis.si" },
            new Mehanik { Ime = "Janez", Priimek = "Horvat", Specializacija = "Elektrika", Telefon = "041-555-666", Email = "janez@servis.si" }
        };
        context.Mehaniki.AddRange(mehaniki);
        context.SaveChanges();

        var rezervacije = new Rezervacija[]
        {
            new Rezervacija{StrankaID=1,VoziloID=1001,MehanikID=1,DatumRezervacije=DateTime.Parse("2024-01-20"),OpisTezave="Redno vzdrževanje",Status=StatusServisa.Koncano,Cena=150.50m,Potrjeno=true},
            new Rezervacija{StrankaID=1,VoziloID=1001,MehanikID=2,DatumRezervacije=DateTime.Parse("2024-02-15"),OpisTezave="Zamenjava zavornih ploščic",Status=StatusServisa.VTehu,Cena=85.00m},
            new Rezervacija{StrankaID=2,VoziloID=1002,MehanikID=1,DatumRezervacije=DateTime.Parse("2024-01-25"),OpisTezave="Diagnostika motorja",Status=StatusServisa.Koncano,Cena=65.00m,Potrjeno=true},
            new Rezervacija{StrankaID=3,VoziloID=1003,MehanikID=3,DatumRezervacije=DateTime.Parse("2024-02-10"),OpisTezave="Menjava pnevmatik",Status=StatusServisa.Cakajoce,Cena=120.00m},
            new Rezervacija{StrankaID=4,VoziloID=1004,MehanikID=2,DatumRezervacije=DateTime.Parse("2024-02-18"),OpisTezave="Popravilo klima naprave",Status=StatusServisa.Cakajoce,Cena=200.00m}
        };
        context.Rezervacije.AddRange(rezervacije);
        
        var roles = new IdentityRole[] {
            new IdentityRole{Id="1", Name="Administrator"},
            new IdentityRole{Id="2", Name="Manager"},
            new IdentityRole{Id="3", Name="Staff"}
        };
        foreach (IdentityRole r in roles)
        {
            context.Roles.Add(r);
        }
        var user = new ApplicationUser
        {
            FirstName = "Robert",
            LastName = "Avtomehanik",
            City = "Ljubljana",
            Email = "servis@avtoservis.si",
            NormalizedEmail = "SERVIS@AVTOSERVIS.SI",
            UserName = "servis@avtoservis.si",
            NormalizedUserName = "servis@avtoservis.si",
            PhoneNumber = "+386-1-123-4567",
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
            SecurityStamp = Guid.NewGuid().ToString("D")
        };
        if (!context.Users.Any(u => u.UserName == user.UserName))
        {
            var password = new PasswordHasher<ApplicationUser>();
            var hashed = password.HashPassword(user,"Testni123!");
            user.PasswordHash = hashed;
            context.Users.Add(user);
            
        }
        context.SaveChanges();
        
        var UserRoles = new IdentityUserRole<string>[]
        {
            new IdentityUserRole<string>{RoleId = roles[0].Id, UserId=user.Id},
            new IdentityUserRole<string>{RoleId = roles[1].Id, UserId=user.Id},
        };
        foreach (IdentityUserRole<string> r in UserRoles)
        {
            context.UserRoles.Add(r);
        }
        context.SaveChanges();
    }
}