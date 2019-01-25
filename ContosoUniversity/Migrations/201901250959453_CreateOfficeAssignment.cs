namespace ContosoUniversity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateOfficeAssignment : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OfficeAssignment",
                c => new
                    {
                        InstructorID = c.Int(nullable: false),
                        Location = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.InstructorID)
                .ForeignKey("dbo.Instructor", t => t.InstructorID)
                .Index(t => t.InstructorID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OfficeAssignment", "InstructorID", "dbo.Instructor");
            DropIndex("dbo.OfficeAssignment", new[] { "InstructorID" });
            DropTable("dbo.OfficeAssignment");
        }
    }
}
