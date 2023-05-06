

using AFRICOMDMSWEB.Models;

namespace AFRICOMDMSWEB.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();

                context.Database.EnsureCreated();

                //Cinema
                if (!context.Folders.Any())
                {
                    context.Folders.AddRange(new List<Folder>()
                    {
                        new Folder()
                        {
                          FolderName = "AFRICOMDMS",
                          ParentFolderID = 0,
                          path = "AFRICOMDMS",
                        }, 
                    });
                    context.SaveChanges();
                }

                if (!context.Departments.Any())
                {
                    context.Departments.AddRange(new List<Department>()
                    {
                        new Department()
                        {
                            Name = "Human Resource"
                        },
                        new Department()
                        {
                            Name = "Finance"
                        }

                    });
                    context.SaveChanges();
                }
            }
        }
    }
}
