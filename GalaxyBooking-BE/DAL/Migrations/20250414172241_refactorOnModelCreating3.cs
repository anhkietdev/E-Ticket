using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class refactorOnModelCreating3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmGenres_Films_FilmId1",
                table: "FilmGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_FilmGenres_Genres_GenreId1",
                table: "FilmGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_Projections_Films_FilmId1",
                table: "Projections");

            migrationBuilder.DropForeignKey(
                name: "FK_Projections_Rooms_RoomId1",
                table: "Projections");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Projections_ProjectionId1",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Seats_SeatId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ProjectionId1",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Projections_FilmId1",
                table: "Projections");

            migrationBuilder.DropIndex(
                name: "IX_Projections_RoomId1",
                table: "Projections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilmGenres",
                table: "FilmGenres");

            migrationBuilder.DropIndex(
                name: "IX_FilmGenres_FilmId1",
                table: "FilmGenres");

            migrationBuilder.DropIndex(
                name: "IX_FilmGenres_GenreId1",
                table: "FilmGenres");

            migrationBuilder.DropColumn(
                name: "ProjectionId1",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "FilmId1",
                table: "Projections");

            migrationBuilder.DropColumn(
                name: "RoomId1",
                table: "Projections");

            migrationBuilder.DropColumn(
                name: "FilmId1",
                table: "FilmGenres");

            migrationBuilder.DropColumn(
                name: "GenreId1",
                table: "FilmGenres");

            migrationBuilder.AlterColumn<Guid>(
                name: "ProjectionId",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomId",
                table: "Projections",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "FilmId",
                table: "Projections",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "GenreId",
                table: "FilmGenres",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<Guid>(
                name: "FilmId",
                table: "FilmGenres",
                type: "uniqueidentifier",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilmGenres",
                table: "FilmGenres",
                columns: new[] { "FilmId", "GenreId" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectionId",
                table: "Tickets",
                column: "ProjectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Projections_FilmId",
                table: "Projections",
                column: "FilmId");

            migrationBuilder.CreateIndex(
                name: "IX_Projections_RoomId",
                table: "Projections",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_FilmGenres_GenreId",
                table: "FilmGenres",
                column: "GenreId");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmGenres_Films_FilmId",
                table: "FilmGenres",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmGenres_Genres_GenreId",
                table: "FilmGenres",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projections_Films_FilmId",
                table: "Projections",
                column: "FilmId",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projections_Rooms_RoomId",
                table: "Projections",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Projections_ProjectionId",
                table: "Tickets",
                column: "ProjectionId",
                principalTable: "Projections",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Seats_SeatId",
                table: "Tickets",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FilmGenres_Films_FilmId",
                table: "FilmGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_FilmGenres_Genres_GenreId",
                table: "FilmGenres");

            migrationBuilder.DropForeignKey(
                name: "FK_Projections_Films_FilmId",
                table: "Projections");

            migrationBuilder.DropForeignKey(
                name: "FK_Projections_Rooms_RoomId",
                table: "Projections");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Projections_ProjectionId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Seats_SeatId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ProjectionId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Projections_FilmId",
                table: "Projections");

            migrationBuilder.DropIndex(
                name: "IX_Projections_RoomId",
                table: "Projections");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FilmGenres",
                table: "FilmGenres");

            migrationBuilder.DropIndex(
                name: "IX_FilmGenres_GenreId",
                table: "FilmGenres");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectionId",
                table: "Tickets",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectionId1",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "RoomId",
                table: "Projections",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "FilmId",
                table: "Projections",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "FilmId1",
                table: "Projections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId1",
                table: "Projections",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<int>(
                name: "GenreId",
                table: "FilmGenres",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "FilmId",
                table: "FilmGenres",
                type: "int",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "FilmId1",
                table: "FilmGenres",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "GenreId1",
                table: "FilmGenres",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FilmGenres",
                table: "FilmGenres",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ProjectionId1",
                table: "Tickets",
                column: "ProjectionId1");

            migrationBuilder.CreateIndex(
                name: "IX_Projections_FilmId1",
                table: "Projections",
                column: "FilmId1");

            migrationBuilder.CreateIndex(
                name: "IX_Projections_RoomId1",
                table: "Projections",
                column: "RoomId1");

            migrationBuilder.CreateIndex(
                name: "IX_FilmGenres_FilmId1",
                table: "FilmGenres",
                column: "FilmId1");

            migrationBuilder.CreateIndex(
                name: "IX_FilmGenres_GenreId1",
                table: "FilmGenres",
                column: "GenreId1");

            migrationBuilder.AddForeignKey(
                name: "FK_FilmGenres_Films_FilmId1",
                table: "FilmGenres",
                column: "FilmId1",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FilmGenres_Genres_GenreId1",
                table: "FilmGenres",
                column: "GenreId1",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projections_Films_FilmId1",
                table: "Projections",
                column: "FilmId1",
                principalTable: "Films",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projections_Rooms_RoomId1",
                table: "Projections",
                column: "RoomId1",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Projections_ProjectionId1",
                table: "Tickets",
                column: "ProjectionId1",
                principalTable: "Projections",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_Seats_SeatId",
                table: "Tickets",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
