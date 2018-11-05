namespace AutoBank.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ModifiedAccNoToLongForAccountNumber : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Accounts", "uniqaccountnumber");
            AlterColumn("dbo.Accounts", "AccountNumber", c => c.Long(nullable: false));
            CreateIndex("dbo.Accounts", "AccountNumber", unique: true, name: "uniqaccountnumber");
        }
        
        public override void Down()
        {
            DropIndex("dbo.Accounts", "uniqaccountnumber");
            AlterColumn("dbo.Accounts", "AccountNumber", c => c.Int(nullable: false));
            CreateIndex("dbo.Accounts", "AccountNumber", unique: true, name: "uniqaccountnumber");
        }
    }
}
