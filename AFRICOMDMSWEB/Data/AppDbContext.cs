
using AFRICOMDMSWEB.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace AFRICOMDMSWEB.Data
{
    public class AppDbContext: IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    public DbSet<Document> Documents { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<Category> Catagories { get; set; }
    public DbSet<Folder> Folders { get; set; }
    public DbSet<FileShared> FileShares { get; set; }
    public DbSet<Department> Departments{ get; set; }
    public DbSet<DocumentVersions> DocumentVersions { get; set;}
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<ReviewDoc> Reviews { get; set; }
    public DbSet<Request> Requests { get; set; }
    public DbSet<RequestFile> RequestFiles { get; set; }
    public DbSet<Actions> Actions { get; set; }
    public DbSet<AprrovalAnswer> ApproveAnswers { get; set; }
    public DbSet<RequestAction> RequestActions { get; set; }
    public DbSet<Transition> Transitions { get; set; }
    public DbSet<ProjectState> ProjectStates { get; set; }  
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectFiles> ProjectFiles { get; set; }
    public DbSet<TeamLeaders> TeamLeaders { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<RequiestForm>  RequiestForms { get; set; }
    public DbSet<BackEndDeveloperTeam> BackEndDeveloperTeams { get; set; }
    public DbSet<FrontEndDeveloperTeam> FrontEndDeveloperTeams { get; set; }
    public DbSet<SolutionEnginneerTeam> SolutionEnginneerTeams { get; set; }
    public DbSet<softwareArctectureTeam> SoftwareArctectureTeams { get; set; }
    public DbSet<TesterTeam> TesterTeams { get; set; }
    public DbSet<UIDesignTeam> UIDesignTeams { get; set; }
    public DbSet<ReqComment> ReqComments { get; set; } 
    public DbSet<ReqAnswerFiles> ReqAnswerFiles { get; set; }
    public DbSet<Attendance> Atttendances { get; set; }
    public DbSet<AttendanceActions> AttendanceActions { get; set; }
    public DbSet<Folder_User> Folder_Users { get; set; }
    public DbSet<shareFolder> ShareFolders { get; set; }
    public DbSet<ReqProcess> ReqProcesss { get; set; }
    public DbSet<ReqType> ReqTypes { get; set; }
    }
}
