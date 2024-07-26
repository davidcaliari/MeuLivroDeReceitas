using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_REFRESH_TOKEN, "Adicionar nova tabela para armazenar os refresh tokens de cada usuário")]
public class Version0000004 : VersionBase
{
    private const string RECIPE_TABLE_NAME = "RefreshTokens";
    public override void Up()
    {
        CreateTable(RECIPE_TABLE_NAME)
            .WithColumn("Value").AsString().Nullable()
            .WithColumn("UserId").AsInt64().NotNullable().ForeignKey("FK_RefreshTokens_User_Id", "Users", "Id")
            .OnDelete(System.Data.Rule.Cascade); ;

    }
}
