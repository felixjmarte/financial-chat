using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FinancialChat.Infrastructure.Persistence.Migrations
{
    public partial class ChatRoomGlobal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Global",
                table: "ChatRooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Global",
                table: "ChatRooms");
        }
    }
}
