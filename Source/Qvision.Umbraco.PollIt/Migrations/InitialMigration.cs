namespace Qvision.Umbraco.PollIt.Migrations
{
    using global::Umbraco.Core;
    using global::Umbraco.Core.Logging;
    using global::Umbraco.Core.Persistence;
    using global::Umbraco.Core.Persistence.Migrations;
    using global::Umbraco.Core.Persistence.SqlSyntax;

    using Qvision.Umbraco.PollIt.Constants;
    using Qvision.Umbraco.PollIt.Models.Pocos;

    [Migration("0.5.0", 1, ApplicationConstants.ProductName)]
    public class InitialMigration : MigrationBase
    {
        private readonly UmbracoDatabase database = ApplicationContext.Current.DatabaseContext.Database;
        private readonly DatabaseSchemaHelper databaseSchema;

        public InitialMigration(ISqlSyntaxProvider sqlSyntax, ILogger logger) : base(sqlSyntax, logger)
        {
            this.databaseSchema = new DatabaseSchemaHelper(this.database, logger, sqlSyntax);
        }

        public override void Up()
        {
            if (!this.databaseSchema.TableExist(TableConstants.Questions.TableName))
            {                
                this.Create.Table(TableConstants.Questions.TableName)
                    .WithColumn("Id").AsInt32().NotNullable()
                        .PrimaryKey("PK_poll_questions").Identity()
                    .WithColumn("Name").AsString().NotNullable()
                    .WithColumn("StartDate").AsDateTime().Nullable()
                    .WithColumn("EndDate").AsDateTime().Nullable()
                    .WithColumn("CreatedDate").AsDateTime().NotNullable();
            }

            if (!this.databaseSchema.TableExist(TableConstants.Answers.TableName))
            {
                this.Create.Table(TableConstants.Answers.TableName)
                    .WithColumn("Id").AsInt32().NotNullable()
                        .PrimaryKey("PK_poll_answers").Identity()
                    .WithColumn("Value").AsString().NotNullable()
                    .WithColumn("Index").AsInt32().NotNullable()
                    .WithColumn("QuestionId").AsInt32().NotNullable();

                this.Create.ForeignKey("QuestionId_Answer")
                    .FromTable(TableConstants.Answers.TableName).ForeignColumn("QuestionId")
                    .ToTable(TableConstants.Questions.TableName).PrimaryColumn("Id");

                this.Create.Index("QuestionId_Answer")
                    .OnTable(TableConstants.Answers.TableName)
                    .OnColumn("QuestionId").Ascending().WithOptions().NonClustered();
            }

            if (!this.databaseSchema.TableExist(TableConstants.Responses.TableName))
            {
                this.Create.Table(TableConstants.Responses.TableName)
                    .WithColumn("Id").AsInt32().NotNullable()
                        .PrimaryKey("PK_poll_responses").Identity()
                    .WithColumn("ResponseDate").AsDateTime().NotNullable()
                    .WithColumn("QuestionId").AsInt32().NotNullable()
                    .WithColumn("AnswerId").AsInt32().NotNullable();              

                this.Create.ForeignKey("QuestionId_Response")
                    .FromTable(TableConstants.Responses.TableName).ForeignColumn("QuestionId")
                    .ToTable(TableConstants.Questions.TableName).PrimaryColumn("Id");

                this.Create.ForeignKey("AnswerId_Response")
                    .FromTable(TableConstants.Responses.TableName).ForeignColumn("AnswerId")
                    .ToTable(TableConstants.Answers.TableName).PrimaryColumn("Id");

                this.Create.Index("QuestionId_Response")
                    .OnTable(TableConstants.Responses.TableName)
                    .OnColumn("QuestionId").Ascending().WithOptions().NonClustered();

                this.Create.Index("AnswerId_Response")
                    .OnTable(TableConstants.Responses.TableName)
                    .OnColumn("AnswerId").Ascending().WithOptions().NonClustered();
            }
        }

        public override void Down()
        {
            this.databaseSchema.DropTable<Response>();
            this.databaseSchema.DropTable<Answer>();
            this.databaseSchema.DropTable<Question>();
        }
    }
}
