namespace Sampe.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _010620182350 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Cargoes", "DescricaoCargo");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cargoes", "DescricaoCargo", c => c.String(unicode: false));
        }
    }
}
