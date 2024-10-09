﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Persistence.Context;

#nullable disable

namespace Persistence.Migrations
{
    [DbContext(typeof(Context.Context))]
    [Migration("20241009124749_initial")]
#pragma warning disable CS8981
    partial class initial
#pragma warning restore CS8981
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.Entities.Deliver", b =>
                {
                    b.Property<string>("Identifier")
                        .HasColumnType("text")
                        .HasColumnName("identificador");

                    b.Property<DateTime>("Birthday")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_nascimento");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_criada");

                    b.Property<string>("DriverLicenseImageS3")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("imagem_cnh_s3");

                    b.Property<string>("DriverLicenseNumber")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("numero_cnh");

                    b.Property<string>("DriverLicenseType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("tipo_cnh");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("nome");

                    b.Property<string>("UniqueIdentifier")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("cnpj");

                    b.HasKey("Identifier");

                    b.HasIndex("DriverLicenseNumber")
                        .IsUnique();

                    b.HasIndex("Identifier")
                        .IsUnique();

                    b.ToTable("entregador");
                });

            modelBuilder.Entity("Domain.Entities.Lease", b =>
                {
                    b.Property<string>("Identifier")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasColumnName("identificador");

                    b.Property<string>("DeliverId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("entregador_id");

                    b.Property<DateTime?>("DevolutionDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_devolucao");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_termino");

                    b.Property<DateTime>("InitialDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_inicio");

                    b.Property<string>("MotocycleBikeId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("moto_id");

                    b.Property<int>("Plan")
                        .HasColumnType("integer")
                        .HasColumnName("plano");

                    b.Property<DateTime>("PreviewEndDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_previsao_termino");

                    b.Property<double?>("Value")
                        .HasColumnType("double precision")
                        .HasColumnName("valor_diaria");

                    b.HasKey("Identifier");

                    b.ToTable("locacao");
                });

            modelBuilder.Entity("Domain.Entities.MotocycleBike", b =>
                {
                    b.Property<string>("Identifier")
                        .HasColumnType("text")
                        .HasColumnName("identificador");

                    b.Property<DateTime?>("CreatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_criada");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("modelo");

                    b.Property<string>("Plate")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("placa");

                    b.Property<DateTime?>("UpdatedDate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("data_atualizada");

                    b.Property<int>("Year")
                        .HasColumnType("integer")
                        .HasColumnName("ano");

                    b.HasKey("Identifier");

                    b.HasIndex("Plate")
                        .IsUnique();

                    b.ToTable("moto");
                });
#pragma warning restore 612, 618
        }
    }
}
