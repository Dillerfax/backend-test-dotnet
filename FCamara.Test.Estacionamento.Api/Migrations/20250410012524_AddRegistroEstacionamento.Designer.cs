﻿// <auto-generated />
using System;
using FCamara.Test.Estacionamento.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace FCamara.Test.Estacionamento.Api.Migrations
{
    [DbContext(typeof(EstacionamentoDbContext))]
    [Migration("20250410012524_AddRegistroEstacionamento")]
    partial class AddRegistroEstacionamento
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("FCamara.Test.Estacionamento.Api.Entities.Estabelecimento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CNPJ")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Endereco")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuantidadeVagasCarros")
                        .HasColumnType("int");

                    b.Property<int>("QuantidadeVagasMotos")
                        .HasColumnType("int");

                    b.Property<string>("Telefone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CNPJ")
                        .IsUnique();

                    b.ToTable("Estabelecimentos");
                });

            modelBuilder.Entity("FCamara.Test.Estacionamento.Api.Entities.RegistroEstacionamento", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EstabelecimentoId")
                        .HasColumnType("int");

                    b.Property<DateTime>("HoraEntrada")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("HoraSaida")
                        .HasColumnType("datetime2");

                    b.Property<int>("VeiculoId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EstabelecimentoId");

                    b.HasIndex("VeiculoId");

                    b.ToTable("RegistrosEstacionamento");
                });

            modelBuilder.Entity("FCamara.Test.Estacionamento.Api.Entities.Veiculo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Cor")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Marca")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Modelo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Placa")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Placa")
                        .IsUnique();

                    b.ToTable("Veiculos");
                });

            modelBuilder.Entity("FCamara.Test.Estacionamento.Api.Entities.RegistroEstacionamento", b =>
                {
                    b.HasOne("FCamara.Test.Estacionamento.Api.Entities.Estabelecimento", "Estabelecimento")
                        .WithMany()
                        .HasForeignKey("EstabelecimentoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("FCamara.Test.Estacionamento.Api.Entities.Veiculo", "Veiculo")
                        .WithMany()
                        .HasForeignKey("VeiculoId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Estabelecimento");

                    b.Navigation("Veiculo");
                });
#pragma warning restore 612, 618
        }
    }
}
