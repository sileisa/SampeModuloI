namespace Sampe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Sampe010620181724 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Usuarios", "ErrorMessage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Usuarios", "ErrorMessage", c => c.String(unicode: false));
        }
    }
}
