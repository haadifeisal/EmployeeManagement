﻿// <auto-generated />
using System;
using EmployeeManagement.WebApi.Repositories.EmployeeManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace EmployeeManagement.WebApi.Migrations
{
    [DbContext(typeof(EmployeeManagementContext))]
    partial class EmployeeManagementContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "Finnish_Swedish_CI_AS")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("EmployeeManagement.WebApi.Repositories.EmployeeManagement.Department", b =>
                {
                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uuid")
                        .HasColumnName("departmentId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.HasKey("DepartmentId");

                    b.ToTable("Department");
                });

            modelBuilder.Entity("EmployeeManagement.WebApi.Repositories.EmployeeManagement.Employee", b =>
                {
                    b.Property<Guid>("EmployeeId")
                        .HasColumnType("uuid")
                        .HasColumnName("employeeId");

                    b.Property<Guid>("DepartmentId")
                        .HasColumnType("uuid")
                        .HasColumnName("departmentId");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<long>("Salary")
                        .HasColumnType("bigint")
                        .HasColumnName("salary");

                    b.HasKey("EmployeeId");

                    b.HasIndex("DepartmentId");

                    b.ToTable("Employee");
                });

            modelBuilder.Entity("EmployeeManagement.WebApi.Repositories.EmployeeManagement.Employee", b =>
                {
                    b.HasOne("EmployeeManagement.WebApi.Repositories.EmployeeManagement.Department", "Department")
                        .WithMany("Employees")
                        .HasForeignKey("DepartmentId")
                        .HasConstraintName("FK_Employee_Department")
                        .IsRequired();

                    b.Navigation("Department");
                });

            modelBuilder.Entity("EmployeeManagement.WebApi.Repositories.EmployeeManagement.Department", b =>
                {
                    b.Navigation("Employees");
                });
#pragma warning restore 612, 618
        }
    }
}
