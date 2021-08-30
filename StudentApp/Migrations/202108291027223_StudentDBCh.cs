namespace StudentApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StudentDBCh : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Students", "StudentClass_ID", "dbo.StudentClasses");
            DropIndex("dbo.Students", new[] { "StudentClass_ID" });
            RenameColumn(table: "dbo.Students", name: "StudentClass_ID", newName: "StudentClassID");
            AlterColumn("dbo.Students", "StudentClassID", c => c.Int(nullable: false));
            CreateIndex("dbo.Students", "StudentClassID");
            AddForeignKey("dbo.Students", "StudentClassID", "dbo.StudentClasses", "ID", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Students", "StudentClassID", "dbo.StudentClasses");
            DropIndex("dbo.Students", new[] { "StudentClassID" });
            AlterColumn("dbo.Students", "StudentClassID", c => c.Int());
            RenameColumn(table: "dbo.Students", name: "StudentClassID", newName: "StudentClass_ID");
            CreateIndex("dbo.Students", "StudentClass_ID");
            AddForeignKey("dbo.Students", "StudentClass_ID", "dbo.StudentClasses", "ID");
        }
    }
}
