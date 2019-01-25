namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRepationships : DbMigration
    {
        public override void Up()
        {
            CreateIndex("dbo.Course", "DepartmentID");
            CreateIndex("dbo.Department", "InstructorID");
            AddForeignKey("dbo.Department", "InstructorID", "dbo.Instructor", "ID");
            AddForeignKey("dbo.Course", "DepartmentID", "dbo.Department", "DepartmentID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Course", "DepartmentID", "dbo.Department");
            DropForeignKey("dbo.Department", "InstructorID", "dbo.Instructor");
            DropIndex("dbo.Department", new[] { "InstructorID" });
            DropIndex("dbo.Course", new[] { "DepartmentID" });
        }
    }
}
