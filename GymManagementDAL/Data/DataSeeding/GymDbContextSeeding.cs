using GymManagementDAL.Data.Contexts;
using GymManagementDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GymManagementDAL.Data.DataSeed
{
    public static class GymDbContextSeeding
    {
        public static bool SeedData(GymDbContext dbContext)
        {
            try
            {
                var HasPlans = dbContext.Plans.Any();
                var HasCategories = dbContext.Categories.Any();
                if (HasPlans && HasCategories) return false;
                if (!HasPlans)
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json");
                    if (plans.Any())
                        dbContext.Plans.AddRange(plans);

                }
                if (!HasCategories)
                {
                    var Categories = LoadDataFromJsonFile<Category>("categories.json");
                    if (Categories.Any())
                        dbContext.Categories.AddRange(Categories);
                }
                return dbContext.SaveChanges() > 0;


            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Seeding Failed : {ex}");
                return false;

            }
        }


        private static List<T> LoadDataFromJsonFile<T>(string FileName)
        {
            //D:\Backend Route\MVC\Assignment\Session02\GymManagementPLSystemSolution\GymManagementPL\wwwroot\Fies\categories.json

            var FilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FileName);

            if (!File.Exists(FilePath)) throw new FileNotFoundException();

            string Date = File.ReadAllText(FilePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive =true
            };
            return JsonSerializer.Deserialize<List<T>>(Date , Options) ?? new List<T>();
        }

    }
}
