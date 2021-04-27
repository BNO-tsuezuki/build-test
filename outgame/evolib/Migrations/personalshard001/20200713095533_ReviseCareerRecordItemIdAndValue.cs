using Microsoft.EntityFrameworkCore.Migrations;

namespace evolib.Migrations.personalshard001
{
    public partial class ReviseCareerRecordItemIdAndValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "intValue",
                table: "CareerRecords");

            migrationBuilder.RenameColumn(
                name: "decimalValue",
                table: "CareerRecords",
                newName: "value");

            migrationBuilder.AlterColumn<string>(
                name: "recordItemId",
                table: "CareerRecords",
                maxLength: 32,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 6);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "value",
                table: "CareerRecords",
                newName: "decimalValue");

            migrationBuilder.AlterColumn<string>(
                name: "recordItemId",
                table: "CareerRecords",
                maxLength: 6,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 32);

            migrationBuilder.AddColumn<int>(
                name: "intValue",
                table: "CareerRecords",
                nullable: false,
                defaultValue: 0);
        }
    }
}
