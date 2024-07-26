using FluentMigrator;

namespace MyRecipeBook.Infrastructure.Migrations.Versions;

[Migration(DatabaseVersions.TABLE_USER, "Criação da tabela de usuário para armazenar informações do mesmo")]
public class Version0000001 : VersionBase
{
    public override void Up()
    {
       CreateTable("Users")
            .WithColumn("UserIdentifier").AsGuid().NotNullable()
            .WithColumn("Name").AsString(255).NotNullable()
            .WithColumn("Email").AsString(255).NotNullable()
            .WithColumn("Password").AsString(2000).NotNullable();
    }
}
