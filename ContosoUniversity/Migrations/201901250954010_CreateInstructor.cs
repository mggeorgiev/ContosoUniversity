namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateInstructor : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Instructor",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        LastName = c.String(nullable: false, maxLength: 50),
                        FirstName = c.String(nullable: false, maxLength: 50),
                        HireDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.Course", "DepartmentID", c => c.Int(nullable: false));
            AddColumn("dbo.Course", "Instructor_ID", c => c.Int());
            AlterColumn("dbo.Course", "Title", c => c.String(maxLength: 50));
            CreateIndex("dbo.Course", "Instructor_ID");
            AddForeignKey("dbo.Course", "Instructor_ID", "dbo.Instructor", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Course", "Instructor_ID", "dbo.Instructor");
            DropIndex("dbo.Course", new[] { "Instructor_ID" });
            AlterColumn("dbo.Course", "Title", c => c.String());
            DropColumn("dbo.Course", "Instructor_ID");
            DropColumn("dbo.Course", "DepartmentID");
            DropTable("dbo.Instructor");
        }
    }
}
