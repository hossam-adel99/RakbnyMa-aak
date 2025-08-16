using Microsoft.AspNetCore.Identity;
using RakbnyMa_aak.Data;
using RakbnyMa_aak.Models;
using RakbnyMa_aak.Utilities;
using static RakbnyMa_aak.Utilities.Enums;

namespace RakbnyMa_aak.SeedData
{
    public static class DbSeeder
    {
        public static async Task SeedRolesAsync(RoleManager<IdentityRole> roleManager)
        {
            foreach (var roleName in Enum.GetNames(typeof(UserType)))
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    var role = new IdentityRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper(),
                        ConcurrencyStamp = Guid.NewGuid().ToString()
                    };

                    await roleManager.CreateAsync(role);
                }
            }
        }
        public static async Task SeedGovernoratesAndCitiesAsync(AppDbContext context)
        {
            if (!context.Governorates.Any())
            {
                var governorates = new List<Governorate>
                 {
                    new Governorate
                    {
                        Name = "القاهرة",
                        Cities = new List<City>
                        {
                            new City { Name = "مدينة نصر" },
                            new City { Name = "مصر الجديدة" },
                            new City { Name = "المعادي" },
                            new City { Name = "التجمع" },
                            new City { Name = "حلوان" },
                            new City { Name = "عين شمس" },
                        }
                    },
                    new Governorate
                    {
                        Name = "الجيزة",
                        Cities = new List<City>
                        {
                            new City { Name = "الهرم" },
                            new City { Name = "العجوزة" },
                            new City { Name = "إمبابة" },
                            new City { Name = "6 أكتوبر" },
                            new City { Name = "الشيخ زايد" },
                            new City { Name = "البدرشين" },
                        }
                    },
                    new Governorate
                    {
                        Name = "الإسكندرية",
                        Cities = new List<City>
                        {
                            new City { Name = "سيدي جابر" },
                            new City { Name = "العصافرة" },
                            new City { Name = "محطة الرمل" },
                            new City { Name = "العامرية" },
                            new City { Name = "برج العرب" },
                            new City { Name = "المنتزه" },
                        }
                    },
                    new Governorate
                    {
                        Name = "الدقهلية",
                        Cities = new List<City>
                        {
                            new City { Name = "المنصورة" },
                            new City { Name = "طلخا" },
                            new City { Name = "ميت غمر" },
                            new City { Name = "بلقاس" },
                            new City { Name = "السنبلاوين" },
                            new City { Name = "دكرنس" },
                        }
                    },
                    new Governorate
                    {
                        Name = "الشرقية",
                        Cities = new List<City>
                        {
                            new City { Name = "الزقازيق" },
                            new City { Name = "بلبيس" },
                            new City { Name = "العاشر من رمضان" },
                            new City { Name = "منيا القمح" },
                            new City { Name = "أبو حماد" },
                            new City { Name = "فاقوس" },
                        }
                    },
                    new Governorate
                    {
                        Name = "الغربية",
                        Cities = new List<City>
                        {
                            new City { Name = "طنطا" },
                            new City { Name = "المحلة الكبرى" },
                            new City { Name = "كفر الزيات" },
                            new City { Name = "زفتى" },
                            new City { Name = "سمنود" },
                            new City { Name = "بسيون" },
                        }
                    },
                    new Governorate
                    {
                        Name = "القليوبية",
                        Cities = new List<City>
                        {
                            new City { Name = "بنها" },
                            new City { Name = "شبرا الخيمة" },
                            new City { Name = "قليوب" },
                            new City { Name = "الخانكة" },
                            new City { Name = "طوخ" },
                            new City { Name = "كفر شكر" },
                        }
                    },
                    new Governorate
                    {
                        Name = "المنوفية",
                        Cities = new List<City>
                        {
                            new City { Name = "شبين الكوم" },
                            new City { Name = "منوف" },
                            new City { Name = "أشمون" },
                            new City { Name = "سرس الليان" },
                            new City { Name = "تلا" },
                            new City { Name = "بركة السبع" },
                        }
                    },
                    new Governorate
                    {
                        Name = "الفيوم",
                        Cities = new List<City>
                        {
                            new City { Name = "الفيوم" },
                            new City { Name = "سنورس" },
                            new City { Name = "إطسا" },
                            new City { Name = "طامية" },
                            new City { Name = "يوسف الصديق" },
                            new City { Name = "أبشواي" },
                        }
                    },
                    new Governorate
                    {
                        Name = "بني سويف",
                        Cities = new List<City>
                        {
                            new City { Name = "بني سويف" },
                            new City { Name = "الواسطى" },
                            new City { Name = "ناصر" },
                            new City { Name = "إهناسيا" },
                            new City { Name = "سمسطا" },
                            new City { Name = "الفشن" },
                        }
                    },
                    new Governorate
                    {
                        Name = "المنيا",
                        Cities = new List<City>
                        {
                            new City { Name = "المنيا" },
                            new City { Name = "ملوي" },
                            new City { Name = "أبوقرقاص" },
                            new City { Name = "مطاي" },
                            new City { Name = "سمالوط" },
                            new City { Name = "بني مزار" },
                        }
                    },
                    new Governorate
                    {
                        Name = "أسيوط",
                        Cities = new List<City>
                        {
                            new City { Name = "أسيوط" },
                            new City { Name = "ديروط" },
                            new City { Name = "منفلوط" },
                            new City { Name = "أبنوب" },
                            new City { Name = "البداري" },
                            new City { Name = "ساحل سليم" },
                        }
                    },
                    new Governorate
                    {
                        Name = "سوهاج",
                        Cities = new List<City>
                        {
                            new City { Name = "سوهاج" },
                            new City { Name = "أخميم" },
                            new City { Name = "جرجا" },
                            new City { Name = "البلينا" },
                            new City { Name = "طهطا" },
                            new City { Name = "المراغة" },
                        }
                    },
                    new Governorate
                    {
                        Name = "قنا",
                        Cities = new List<City>
                        {
                            new City { Name = "قنا" },
                            new City { Name = "نجع حمادي" },
                            new City { Name = "دشنا" },
                            new City { Name = "قفط" },
                            new City { Name = "قوص" },
                            new City { Name = "نقادة" },
                        }
                    },
                    new Governorate
                    {
                        Name = "الأقصر",
                        Cities = new List<City>
                        {
                            new City { Name = "الأقصر" },
                            new City { Name = "إسنا" },
                            new City { Name = "البياضية" },
                            new City { Name = "أرمنت" },
                            new City { Name = "الزينية" },
                            new City { Name = "القرنة" },
                        }
                    },
                    new Governorate
                    {
                        Name = "أسوان",
                        Cities = new List<City>
                        {
                            new City { Name = "أسوان" },
                            new City { Name = "إدفو" },
                            new City { Name = "كوم أمبو" },
                            new City { Name = "نصر النوبة" },
                            new City { Name = "دراو" },
                        }
                    },
                    new Governorate
                    {
                        Name = "البحر الأحمر",
                        Cities = new List<City>
                        {
                            new City { Name = "الغردقة" },
                            new City { Name = "رأس غارب" },
                            new City { Name = "سفاجا" },
                            new City { Name = "القصير" },
                            new City { Name = "مرسى علم" },
                            new City { Name = "الشلاتين" },
                        }
                    },
                    new Governorate
                    {
                        Name = "السويس",
                        Cities = new List<City>
                        {
                            new City { Name = "السويس" },
                            new City { Name = "الجناين" },
                            new City { Name = "عتاقة" },
                            new City { Name = "فيصل" },
                        }
                    },
                    new Governorate
                    {
                        Name = "الإسماعيلية",
                        Cities = new List<City>
                        {
                            new City { Name = "الإسماعيلية" },
                            new City { Name = "فايد" },
                            new City { Name = "القنطرة شرق" },
                            new City { Name = "القنطرة غرب" },
                            new City { Name = "التل الكبير" },
                            new City { Name = "أبوصوير" },
                        }
                    },
                    new Governorate
                    {
                        Name = "بورسعيد",
                        Cities = new List<City>
                        {
                            new City { Name = "بورسعيد" },
                            new City { Name = "بورفؤاد" },
                        }
                    },
                    new Governorate
                    {
                        Name = "دمياط",
                        Cities = new List<City>
                        {
                            new City { Name = "دمياط" },
                            new City { Name = "رأس البر" },
                            new City { Name = "الزرقا" },
                            new City { Name = "فارسكور" },
                            new City { Name = "كفر سعد" },
                            new City { Name = "كفر البطيخ" },
                        }
                    },
                    new Governorate
                    {
                        Name = "مطروح",
                        Cities = new List<City>
                        {
                            new City { Name = "مرسى مطروح" },
                            new City { Name = "الحمام" },
                            new City { Name = "العلمين" },
                            new City { Name = "النجيلة" },
                            new City { Name = "سيدي براني" },
                            new City { Name = "السلوم" },
                        }
                    },
                    new Governorate
                    {
                        Name = "الوادي الجديد",
                        Cities = new List<City>
                        {
                            new City { Name = "الخارجة" },
                            new City { Name = "الداخلة" },
                            new City { Name = "الفرافرة" },
                            new City { Name = "باريس" },
                            new City { Name = "بلاط" },
                        }
                    },
                    new Governorate
                    {
                        Name = "شمال سيناء",
                        Cities = new List<City>
                        {
                            new City {  Name = "العريش" },
                            new City {  Name = "الشيخ زويد" },
                            new City {  Name = "رفح" },
                            new City {  Name = "بئر العبد" },
                            new City {  Name = "الحسنة" },
                            new City {  Name = "نخل" },
                        }
                    },
                    new Governorate
                    {
                        Name = "جنوب سيناء",
                        Cities = new List<City>
                        {
                            new City {  Name = "شرم الشيخ" },
                            new City {  Name = "طور سيناء" },
                            new City {  Name = "دهب" },
                            new City {  Name = "نويبع" },
                            new City {  Name = "سانت كاترين" },
                            new City {  Name = "أبو رديس" },
                        }
                    },
                    };
                await context.Governorates.AddRangeAsync(governorates);
                await context.SaveChangesAsync();
            }
           
        }
        public static async Task SeedAdminUserAsync(
        UserManager<ApplicationUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IConfiguration config)
        {
            var section = config.GetSection("AdminUser");
            var adminEmail = section["Email"];
            var adminUserName = section["UserName"];
            var adminPassword = section["Password"];
            var adminFullName = section["FullName"];

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                var user = new ApplicationUser
                {
                    UserName = adminUserName,
                    FullName = adminFullName,
                    Email = adminEmail,
                    EmailConfirmed = true,
                    PhoneNumber = "01158039656",
                    Picture = "https://res.cloudinary.com/dbrz7pbsa/image/upload/v1751624539/default-profile_zo7m6z.png",
                    UserType = Enums.UserType.Admin
                };

                var createResult = await userManager.CreateAsync(user, adminPassword);

                if (createResult.Succeeded)
                {
                    if (!await roleManager.RoleExistsAsync("Admin"))
                    {
                        await roleManager.CreateAsync(new IdentityRole("Admin"));
                    }

                    await userManager.AddToRoleAsync(user, "Admin");
                }
            }
        }

    }

}

