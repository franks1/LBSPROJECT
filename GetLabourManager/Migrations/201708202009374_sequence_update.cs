namespace GetLabourManager.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class sequence_update : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.SequenceNumberings", newName: "MasterSequences");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.MasterSequences", newName: "SequenceNumberings");
        }
    }
}
