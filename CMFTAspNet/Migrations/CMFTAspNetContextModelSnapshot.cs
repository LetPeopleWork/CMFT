﻿// <auto-generated />
using System;
using CMFTAspNet.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace CMFTAspNet.Migrations
{
    [DbContext(typeof(Data.AppContext))]
    partial class CMFTAspNetContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.2");

            modelBuilder.Entity("CMFTAspNet.Models.Feature", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("Order")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Features");
                });

            modelBuilder.Entity("CMFTAspNet.Models.Forecast.WhenForecast", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FeatureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TotalTrials")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId")
                        .IsUnique();

                    b.ToTable("WhenForecast");
                });

            modelBuilder.Entity("CMFTAspNet.Models.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IncludeUnparentedItems")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("SearchBy")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SearchTerm")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("TargetDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("WorkItemTypes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Projects");
                });

            modelBuilder.Entity("CMFTAspNet.Models.RemainingWork", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FeatureId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RemainingWorkItems")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TeamId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("FeatureId");

                    b.HasIndex("TeamId");

                    b.ToTable("RemainingWork");
                });

            modelBuilder.Entity("CMFTAspNet.Models.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AdditionalRelatedFields")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("AreaPaths")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("FeatureWIP")
                        .HasColumnType("INTEGER");

                    b.Property<string>("IgnoredTags")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("ProjectId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ProjectName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("RawThroughput")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ThroughputHistory")
                        .HasColumnType("INTEGER");

                    b.Property<string>("WorkItemTypes")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("WorkTrackingSystem")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Teams");
                });

            modelBuilder.Entity("CMFTAspNet.WorkTracking.WorkTrackingSystemOption", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("TeamId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("TeamId");

                    b.ToTable("WorkTrackingSystemOption");
                });

            modelBuilder.Entity("CMFTAspNet.Models.Feature", b =>
                {
                    b.HasOne("CMFTAspNet.Models.Project", null)
                        .WithMany("Features")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("CMFTAspNet.Models.Forecast.WhenForecast", b =>
                {
                    b.HasOne("CMFTAspNet.Models.Feature", "Feature")
                        .WithOne("Forecast")
                        .HasForeignKey("CMFTAspNet.Models.Forecast.WhenForecast", "FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feature");
                });

            modelBuilder.Entity("CMFTAspNet.Models.RemainingWork", b =>
                {
                    b.HasOne("CMFTAspNet.Models.Feature", "Feature")
                        .WithMany("RemainingWork")
                        .HasForeignKey("FeatureId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("CMFTAspNet.Models.Team", "Team")
                        .WithMany()
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Feature");

                    b.Navigation("Team");
                });

            modelBuilder.Entity("CMFTAspNet.Models.Team", b =>
                {
                    b.HasOne("CMFTAspNet.Models.Project", null)
                        .WithMany("InvolvedTeams")
                        .HasForeignKey("ProjectId");
                });

            modelBuilder.Entity("CMFTAspNet.WorkTracking.WorkTrackingSystemOption", b =>
                {
                    b.HasOne("CMFTAspNet.Models.Team", null)
                        .WithMany("WorkTrackingSystemOptions")
                        .HasForeignKey("TeamId");
                });

            modelBuilder.Entity("CMFTAspNet.Models.Feature", b =>
                {
                    b.Navigation("Forecast")
                        .IsRequired();

                    b.Navigation("RemainingWork");
                });

            modelBuilder.Entity("CMFTAspNet.Models.Project", b =>
                {
                    b.Navigation("Features");

                    b.Navigation("InvolvedTeams");
                });

            modelBuilder.Entity("CMFTAspNet.Models.Team", b =>
                {
                    b.Navigation("WorkTrackingSystemOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
