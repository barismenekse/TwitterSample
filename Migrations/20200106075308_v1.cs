using Microsoft.EntityFrameworkCore.Migrations;

namespace Twitter.Migrations
{
    public partial class v1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<string>(nullable: false),
                    name = table.Column<string>(nullable: true),
                    surname = table.Column<string>(nullable: true),
                    profilePhoto = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Following",
                columns: table => new
                {
                    followingId = table.Column<string>(nullable: false),
                    followedId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Following", x => new { x.followedId, x.followingId });
                    table.ForeignKey(
                        name: "FK_Following_users_followedId",
                        column: x => x.followedId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Following_users_followingId",
                        column: x => x.followingId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "tweets",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    content = table.Column<string>(nullable: true),
                    date = table.Column<string>(nullable: true),
                    image = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tweets", x => x.id);
                    table.ForeignKey(
                        name: "FK_tweets_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    tweetId = table.Column<int>(nullable: false),
                    userId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likes", x => new { x.tweetId, x.userId });
                    table.ForeignKey(
                        name: "FK_likes_tweets_tweetId",
                        column: x => x.tweetId,
                        principalTable: "tweets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_likes_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "retweets",
                columns: table => new
                {
                    tweetId = table.Column<int>(nullable: false),
                    userId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_retweets", x => new { x.tweetId, x.userId });
                    table.ForeignKey(
                        name: "FK_retweets_tweets_tweetId",
                        column: x => x.tweetId,
                        principalTable: "tweets",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_retweets_users_userId",
                        column: x => x.userId,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Following_followingId",
                table: "Following",
                column: "followingId");

            migrationBuilder.CreateIndex(
                name: "IX_likes_userId",
                table: "likes",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_retweets_userId",
                table: "retweets",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_tweets_UserId",
                table: "tweets",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Following");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "retweets");

            migrationBuilder.DropTable(
                name: "tweets");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
