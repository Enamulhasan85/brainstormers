using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskForge.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UserProfileIdTypeChanged : Migration
    {
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Notifications_UserProfiles_UserProfileId1",
				table: "Notifications");

			migrationBuilder.DropIndex(
				name: "IX_Notifications_UserProfileId1",
				table: "Notifications");

			migrationBuilder.DropColumn(
				name: "UserProfileId1",
				table: "Notifications");

			// ❌ Drop and recreate the column since conversion is not possible
			migrationBuilder.DropColumn(
				name: "UserProfileId",
				table: "Notifications");

			migrationBuilder.AddColumn<int>(
				name: "UserProfileId",
				table: "Notifications",
				type: "integer",
				nullable: false,
				defaultValue: 0); // Or nullable if necessary

			migrationBuilder.DropColumn(
				name: "RelatedEntityId",
				table: "Notifications");

			migrationBuilder.AddColumn<int>(
				name: "RelatedEntityId",
				table: "Notifications",
				type: "integer",
				nullable: true);

			migrationBuilder.CreateIndex(
				name: "IX_Notifications_UserProfileId",
				table: "Notifications",
				column: "UserProfileId");

			migrationBuilder.AddForeignKey(
				name: "FK_Notifications_UserProfiles_UserProfileId",
				table: "Notifications",
				column: "UserProfileId",
				principalTable: "UserProfiles",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}



		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql(
	@"ALTER TABLE ""Notifications"" 
      ALTER COLUMN ""UserProfileId"" TYPE uuid 
      USING ""UserProfileId""::uuid;");

			migrationBuilder.Sql(
				@"ALTER TABLE ""Notifications"" 
      ALTER COLUMN ""RelatedEntityId"" TYPE uuid 
      USING ""RelatedEntityId""::uuid;");
		}
	}
}
