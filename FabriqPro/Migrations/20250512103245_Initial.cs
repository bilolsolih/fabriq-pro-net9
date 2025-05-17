using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace FabriqPro.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "accessories",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accessories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "clients",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    first_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    last_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    phone_number = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    address = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "colors",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    color_code = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_colors", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "materials",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_materials", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "miscellaneous",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_miscellaneous", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "parties",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parties", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "spare_parts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spare_parts", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    department = table.Column<int>(type: "integer", nullable: false),
                    first_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    last_name = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    profile_photo = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    phone_number = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    password = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    address = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    passport_series = table.Column<string>(type: "character varying(9)", maxLength: 9, nullable: false),
                    birthdate = table.Column<DateOnly>(type: "date", nullable: false),
                    role = table.Column<int>(type: "integer", nullable: false),
                    salary = table.Column<double>(type: "double precision", nullable: true),
                    working_hours = table.Column<double>(type: "double precision", nullable: true),
                    working_days = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_models",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_type_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: false),
                    color_id = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_models", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_models_colors_color_id",
                        column: x => x.color_id,
                        principalTable: "colors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_models_product_types_product_type_id",
                        column: x => x.product_type_id,
                        principalTable: "product_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_part_types",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    product_type_id = table.Column<int>(type: "integer", nullable: false),
                    title = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_part_types", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_part_types_product_types_product_type_id",
                        column: x => x.product_type_id,
                        principalTable: "product_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "accessory_department",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    department = table.Column<int>(type: "integer", nullable: false),
                    origin_id = table.Column<int>(type: "integer", nullable: true),
                    accepted_user_id = table.Column<int>(type: "integer", nullable: false),
                    from_user_id = table.Column<int>(type: "integer", nullable: false),
                    to_user_id = table.Column<int>(type: "integer", nullable: false),
                    accessory_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false),
                    unit = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accessory_department", x => x.id);
                    table.ForeignKey(
                        name: "FK_accessory_department_accessories_accessory_id",
                        column: x => x.accessory_id,
                        principalTable: "accessories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_accessory_department_accessory_department_origin_id",
                        column: x => x.origin_id,
                        principalTable: "accessory_department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_accessory_department_users_accepted_user_id",
                        column: x => x.accepted_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_accessory_department_users_from_user_id",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_accessory_department_users_to_user_id",
                        column: x => x.to_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "cuttings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    master_id = table.Column<int>(type: "integer", nullable: false),
                    waste = table.Column<double>(type: "double precision", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cuttings", x => x.id);
                    table.ForeignKey(
                        name: "FK_cuttings_users_master_id",
                        column: x => x.master_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "material_to_department",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    origin_id = table.Column<int>(type: "integer", nullable: true),
                    department = table.Column<int>(type: "integer", nullable: false),
                    material_id = table.Column<int>(type: "integer", nullable: false),
                    accepted_user_id = table.Column<int>(type: "integer", nullable: false),
                    from_user_id = table.Column<int>(type: "integer", nullable: false),
                    to_user_id = table.Column<int>(type: "integer", nullable: false),
                    party_id = table.Column<int>(type: "integer", nullable: false),
                    color_id = table.Column<int>(type: "integer", nullable: false),
                    thickness = table.Column<double>(type: "double precision", nullable: false),
                    width = table.Column<double>(type: "double precision", nullable: false),
                    has_patterns = table.Column<bool>(type: "boolean", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false),
                    unit = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_material_to_department", x => x.id);
                    table.ForeignKey(
                        name: "FK_material_to_department_colors_color_id",
                        column: x => x.color_id,
                        principalTable: "colors",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_to_department_material_to_department_origin_id",
                        column: x => x.origin_id,
                        principalTable: "material_to_department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_to_department_materials_material_id",
                        column: x => x.material_id,
                        principalTable: "materials",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_material_to_department_parties_party_id",
                        column: x => x.party_id,
                        principalTable: "parties",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_material_to_department_users_accepted_user_id",
                        column: x => x.accepted_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_to_department_users_from_user_id",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_material_to_department_users_to_user_id",
                        column: x => x.to_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "miscellaneous_department",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    department = table.Column<int>(type: "integer", nullable: false),
                    origin_id = table.Column<int>(type: "integer", nullable: true),
                    miscellaneous_id = table.Column<int>(type: "integer", nullable: false),
                    accepted_user_id = table.Column<int>(type: "integer", nullable: false),
                    from_user_id = table.Column<int>(type: "integer", nullable: false),
                    to_user_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false),
                    unit = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_miscellaneous_department", x => x.id);
                    table.ForeignKey(
                        name: "FK_miscellaneous_department_miscellaneous_department_origin_id",
                        column: x => x.origin_id,
                        principalTable: "miscellaneous_department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_miscellaneous_department_miscellaneous_miscellaneous_id",
                        column: x => x.miscellaneous_id,
                        principalTable: "miscellaneous",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_miscellaneous_department_users_accepted_user_id",
                        column: x => x.accepted_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_miscellaneous_department_users_from_user_id",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_miscellaneous_department_users_to_user_id",
                        column: x => x.to_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "spare_part_department",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    department = table.Column<int>(type: "integer", nullable: false),
                    origin_id = table.Column<int>(type: "integer", nullable: true),
                    spare_part_id = table.Column<int>(type: "integer", nullable: false),
                    accepted_user_id = table.Column<int>(type: "integer", nullable: false),
                    from_user_id = table.Column<int>(type: "integer", nullable: false),
                    to_user_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false),
                    unit = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_spare_part_department", x => x.id);
                    table.ForeignKey(
                        name: "FK_spare_part_department_spare_part_department_origin_id",
                        column: x => x.origin_id,
                        principalTable: "spare_part_department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_spare_part_department_spare_parts_spare_part_id",
                        column: x => x.spare_part_id,
                        principalTable: "spare_parts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_spare_part_department_users_accepted_user_id",
                        column: x => x.accepted_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_spare_part_department_users_from_user_id",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_spare_part_department_users_to_user_id",
                        column: x => x.to_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    origin_id = table.Column<int>(type: "integer", nullable: true),
                    department = table.Column<int>(type: "integer", nullable: false),
                    master_id = table.Column<int>(type: "integer", nullable: false),
                    from_user_id = table.Column<int>(type: "integer", nullable: true),
                    to_user_id = table.Column<int>(type: "integer", nullable: true),
                    product_type_id = table.Column<int>(type: "integer", nullable: false),
                    product_model_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_products_product_models_product_model_id",
                        column: x => x.product_model_id,
                        principalTable: "product_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_product_types_product_type_id",
                        column: x => x.product_type_id,
                        principalTable: "product_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_products_origin_id",
                        column: x => x.origin_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_users_from_user_id",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_users_master_id",
                        column: x => x.master_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_products_users_to_user_id",
                        column: x => x.to_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "product_parts",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    origin_id = table.Column<int>(type: "integer", nullable: true),
                    department = table.Column<int>(type: "integer", nullable: false),
                    master_id = table.Column<int>(type: "integer", nullable: false),
                    from_user_id = table.Column<int>(type: "integer", nullable: true),
                    to_user_id = table.Column<int>(type: "integer", nullable: true),
                    product_part_type_id = table.Column<int>(type: "integer", nullable: false),
                    product_model_id = table.Column<int>(type: "integer", nullable: false),
                    quantity = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated = table.Column<DateTime>(type: "timestamp with time zone", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product_parts", x => x.id);
                    table.ForeignKey(
                        name: "FK_product_parts_product_models_product_model_id",
                        column: x => x.product_model_id,
                        principalTable: "product_models",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_parts_product_part_types_product_part_type_id",
                        column: x => x.product_part_type_id,
                        principalTable: "product_part_types",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_parts_product_parts_origin_id",
                        column: x => x.origin_id,
                        principalTable: "product_parts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_parts_users_from_user_id",
                        column: x => x.from_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_parts_users_master_id",
                        column: x => x.master_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_product_parts_users_to_user_id",
                        column: x => x.to_user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CuttingMaterial",
                columns: table => new
                {
                    CuttingId = table.Column<int>(type: "integer", nullable: false),
                    MaterialsUsedId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuttingMaterial", x => new { x.CuttingId, x.MaterialsUsedId });
                    table.ForeignKey(
                        name: "FK_CuttingMaterial_cuttings_CuttingId",
                        column: x => x.CuttingId,
                        principalTable: "cuttings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CuttingMaterial_material_to_department_MaterialsUsedId",
                        column: x => x.MaterialsUsedId,
                        principalTable: "material_to_department",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CuttingProductPart",
                columns: table => new
                {
                    CuttingId = table.Column<int>(type: "integer", nullable: false),
                    ProducedPartsId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuttingProductPart", x => new { x.CuttingId, x.ProducedPartsId });
                    table.ForeignKey(
                        name: "FK_CuttingProductPart_cuttings_CuttingId",
                        column: x => x.CuttingId,
                        principalTable: "cuttings",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CuttingProductPart_product_parts_ProducedPartsId",
                        column: x => x.ProducedPartsId,
                        principalTable: "product_parts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_accessories_title",
                table: "accessories",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accessory_department_accepted_user_id",
                table: "accessory_department",
                column: "accepted_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_accessory_department_accessory_id",
                table: "accessory_department",
                column: "accessory_id");

            migrationBuilder.CreateIndex(
                name: "IX_accessory_department_department_to_user_id_accessory_id_Sta~",
                table: "accessory_department",
                columns: new[] { "department", "to_user_id", "accessory_id", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_accessory_department_from_user_id",
                table: "accessory_department",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_accessory_department_origin_id",
                table: "accessory_department",
                column: "origin_id");

            migrationBuilder.CreateIndex(
                name: "IX_accessory_department_to_user_id",
                table: "accessory_department",
                column: "to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_colors_color_code",
                table: "colors",
                column: "color_code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_colors_title",
                table: "colors",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CuttingMaterial_MaterialsUsedId",
                table: "CuttingMaterial",
                column: "MaterialsUsedId");

            migrationBuilder.CreateIndex(
                name: "IX_CuttingProductPart_ProducedPartsId",
                table: "CuttingProductPart",
                column: "ProducedPartsId");

            migrationBuilder.CreateIndex(
                name: "IX_cuttings_master_id",
                table: "cuttings",
                column: "master_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_to_department_accepted_user_id",
                table: "material_to_department",
                column: "accepted_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_to_department_color_id",
                table: "material_to_department",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_to_department_department_to_user_id_material_id_pa~",
                table: "material_to_department",
                columns: new[] { "department", "to_user_id", "material_id", "party_id", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_material_to_department_from_user_id",
                table: "material_to_department",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_to_department_material_id",
                table: "material_to_department",
                column: "material_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_to_department_origin_id",
                table: "material_to_department",
                column: "origin_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_to_department_party_id",
                table: "material_to_department",
                column: "party_id");

            migrationBuilder.CreateIndex(
                name: "IX_material_to_department_to_user_id",
                table: "material_to_department",
                column: "to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_materials_title",
                table: "materials",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_miscellaneous_title",
                table: "miscellaneous",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_miscellaneous_department_accepted_user_id",
                table: "miscellaneous_department",
                column: "accepted_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_miscellaneous_department_department_to_user_id_miscellaneou~",
                table: "miscellaneous_department",
                columns: new[] { "department", "to_user_id", "miscellaneous_id", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_miscellaneous_department_from_user_id",
                table: "miscellaneous_department",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_miscellaneous_department_miscellaneous_id",
                table: "miscellaneous_department",
                column: "miscellaneous_id");

            migrationBuilder.CreateIndex(
                name: "IX_miscellaneous_department_origin_id",
                table: "miscellaneous_department",
                column: "origin_id");

            migrationBuilder.CreateIndex(
                name: "IX_miscellaneous_department_to_user_id",
                table: "miscellaneous_department",
                column: "to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_parties_title",
                table: "parties",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_models_color_id",
                table: "product_models",
                column: "color_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_models_product_type_id",
                table: "product_models",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_part_types_product_type_id",
                table: "product_part_types",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_part_types_title",
                table: "product_part_types",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_parts_department_master_id_product_part_type_id_Sta~",
                table: "product_parts",
                columns: new[] { "department", "master_id", "product_part_type_id", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_product_parts_from_user_id",
                table: "product_parts",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_parts_master_id",
                table: "product_parts",
                column: "master_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_parts_origin_id",
                table: "product_parts",
                column: "origin_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_parts_product_model_id",
                table: "product_parts",
                column: "product_model_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_parts_product_part_type_id",
                table: "product_parts",
                column: "product_part_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_parts_to_user_id",
                table: "product_parts",
                column: "to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_product_types_title",
                table: "product_types",
                column: "title");

            migrationBuilder.CreateIndex(
                name: "IX_products_from_user_id",
                table: "products",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_master_id",
                table: "products",
                column: "master_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_origin_id",
                table: "products",
                column: "origin_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_product_model_id",
                table: "products",
                column: "product_model_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_product_type_id",
                table: "products",
                column: "product_type_id");

            migrationBuilder.CreateIndex(
                name: "IX_products_to_user_id",
                table: "products",
                column: "to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_spare_part_department_accepted_user_id",
                table: "spare_part_department",
                column: "accepted_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_spare_part_department_department_to_user_id_spare_part_id_S~",
                table: "spare_part_department",
                columns: new[] { "department", "to_user_id", "spare_part_id", "Status" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_spare_part_department_from_user_id",
                table: "spare_part_department",
                column: "from_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_spare_part_department_origin_id",
                table: "spare_part_department",
                column: "origin_id");

            migrationBuilder.CreateIndex(
                name: "IX_spare_part_department_spare_part_id",
                table: "spare_part_department",
                column: "spare_part_id");

            migrationBuilder.CreateIndex(
                name: "IX_spare_part_department_to_user_id",
                table: "spare_part_department",
                column: "to_user_id");

            migrationBuilder.CreateIndex(
                name: "IX_spare_parts_title",
                table: "spare_parts",
                column: "title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_users_phone_number",
                table: "users",
                column: "phone_number",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "accessory_department");

            migrationBuilder.DropTable(
                name: "clients");

            migrationBuilder.DropTable(
                name: "CuttingMaterial");

            migrationBuilder.DropTable(
                name: "CuttingProductPart");

            migrationBuilder.DropTable(
                name: "miscellaneous_department");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "spare_part_department");

            migrationBuilder.DropTable(
                name: "accessories");

            migrationBuilder.DropTable(
                name: "material_to_department");

            migrationBuilder.DropTable(
                name: "cuttings");

            migrationBuilder.DropTable(
                name: "product_parts");

            migrationBuilder.DropTable(
                name: "miscellaneous");

            migrationBuilder.DropTable(
                name: "spare_parts");

            migrationBuilder.DropTable(
                name: "materials");

            migrationBuilder.DropTable(
                name: "parties");

            migrationBuilder.DropTable(
                name: "product_models");

            migrationBuilder.DropTable(
                name: "product_part_types");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "colors");

            migrationBuilder.DropTable(
                name: "product_types");
        }
    }
}
