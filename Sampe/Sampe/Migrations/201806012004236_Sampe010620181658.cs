namespace Sampe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sampe010620181658 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Usuarios", "ErrorMessage", c => c.String(unicode: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Usuarios", "ErrorMessage");
        }
    }
}
