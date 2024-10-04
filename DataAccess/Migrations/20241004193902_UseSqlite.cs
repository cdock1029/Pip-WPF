using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pip.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UseSqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Treasuries",
                columns: table => new
                {
                    Cusip = table.Column<string>(type: "TEXT", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    AccruedInterestPer1000 = table.Column<string>(type: "TEXT", nullable: true),
                    AccruedInterestPer100 = table.Column<string>(type: "TEXT", nullable: true),
                    AdjustedAccruedInterestPer1000 = table.Column<string>(type: "TEXT", nullable: true),
                    AdjustedPrice = table.Column<string>(type: "TEXT", nullable: true),
                    AllocationPercentage = table.Column<string>(type: "TEXT", nullable: true),
                    AllocationPercentageDecimals = table.Column<string>(type: "TEXT", nullable: true),
                    AnnouncedCusip = table.Column<string>(type: "TEXT", nullable: true),
                    AnnouncementDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    AuctionDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    AuctionDateYear = table.Column<string>(type: "TEXT", nullable: true),
                    AuctionFormat = table.Column<string>(type: "TEXT", nullable: true),
                    AverageMedianDiscountRate = table.Column<string>(type: "TEXT", nullable: true),
                    AverageMedianInvestmentRate = table.Column<string>(type: "TEXT", nullable: true),
                    AverageMedianPrice = table.Column<string>(type: "TEXT", nullable: true),
                    AverageMedianDiscountMargin = table.Column<string>(type: "TEXT", nullable: true),
                    AverageMedianYield = table.Column<string>(type: "TEXT", nullable: true),
                    BackDated = table.Column<string>(type: "TEXT", nullable: true),
                    BackDatedDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    BidToCoverRatio = table.Column<string>(type: "TEXT", nullable: true),
                    CallDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    Callable = table.Column<string>(type: "TEXT", nullable: true),
                    CalledDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    CashManagementBillCmb = table.Column<string>(type: "TEXT", nullable: true),
                    ClosingTimeCompetitive = table.Column<string>(type: "TEXT", nullable: true),
                    ClosingTimeNoncompetitive = table.Column<string>(type: "TEXT", nullable: true),
                    CompetitiveAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    CompetitiveBidDecimals = table.Column<string>(type: "TEXT", nullable: true),
                    CompetitiveTendered = table.Column<string>(type: "TEXT", nullable: true),
                    CompetitiveTendersAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    CorpusCusip = table.Column<string>(type: "TEXT", nullable: true),
                    CpiBaseReferencePeriod = table.Column<string>(type: "TEXT", nullable: true),
                    CurrentlyOutstanding = table.Column<string>(type: "TEXT", nullable: true),
                    DatedDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    DirectBidderAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    DirectBidderTendered = table.Column<string>(type: "TEXT", nullable: true),
                    EstimatedAmountOfPubliclyHeldMaturingSecuritiesByType = table.Column<string>(type: "TEXT", nullable: true),
                    FimaIncluded = table.Column<string>(type: "TEXT", nullable: true),
                    FimaNoncompetitiveAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    FimaNoncompetitiveTendered = table.Column<string>(type: "TEXT", nullable: true),
                    FirstInterestPeriod = table.Column<string>(type: "TEXT", nullable: true),
                    FirstInterestPaymentDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    FloatingRate = table.Column<string>(type: "TEXT", nullable: true),
                    FrnIndexDeterminationDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    FrnIndexDeterminationRate = table.Column<string>(type: "TEXT", nullable: true),
                    HighDiscountRate = table.Column<string>(type: "TEXT", nullable: true),
                    HighInvestmentRate = table.Column<string>(type: "TEXT", nullable: true),
                    HighPrice = table.Column<string>(type: "TEXT", nullable: true),
                    HighDiscountMargin = table.Column<string>(type: "TEXT", nullable: true),
                    HighYield = table.Column<string>(type: "TEXT", nullable: true),
                    IndexRatioOnIssueDate = table.Column<string>(type: "TEXT", nullable: true),
                    IndirectBidderAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    IndirectBidderTendered = table.Column<string>(type: "TEXT", nullable: true),
                    InterestPaymentFrequency = table.Column<string>(type: "TEXT", nullable: true),
                    InterestRate = table.Column<string>(type: "TEXT", nullable: true),
                    LowDiscountRate = table.Column<string>(type: "TEXT", nullable: true),
                    LowInvestmentRate = table.Column<string>(type: "TEXT", nullable: true),
                    LowPrice = table.Column<string>(type: "TEXT", nullable: true),
                    LowDiscountMargin = table.Column<string>(type: "TEXT", nullable: true),
                    LowYield = table.Column<string>(type: "TEXT", nullable: true),
                    MaturingDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    MaturityDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    MaximumCompetitiveAward = table.Column<string>(type: "TEXT", nullable: true),
                    MaximumNoncompetitiveAward = table.Column<string>(type: "TEXT", nullable: true),
                    MaximumSingleBid = table.Column<string>(type: "TEXT", nullable: true),
                    MinimumBidAmount = table.Column<string>(type: "TEXT", nullable: true),
                    MinimumStripAmount = table.Column<string>(type: "TEXT", nullable: true),
                    MinimumToIssue = table.Column<string>(type: "TEXT", nullable: true),
                    MultiplesToBid = table.Column<string>(type: "TEXT", nullable: true),
                    MultiplesToIssue = table.Column<string>(type: "TEXT", nullable: true),
                    NlpExclusionAmount = table.Column<string>(type: "TEXT", nullable: true),
                    NlpReportingThreshold = table.Column<string>(type: "TEXT", nullable: true),
                    NoncompetitiveAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    NoncompetitiveTendersAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    OfferingAmount = table.Column<string>(type: "TEXT", nullable: true),
                    OriginalCusip = table.Column<string>(type: "TEXT", nullable: true),
                    OriginalDatedDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    OriginalIssueDate = table.Column<DateOnly>(type: "TEXT", nullable: true),
                    OriginalSecurityTerm = table.Column<string>(type: "TEXT", nullable: true),
                    PdfFilenameAnnouncement = table.Column<string>(type: "TEXT", nullable: true),
                    PdfFilenameCompetitiveResults = table.Column<string>(type: "TEXT", nullable: true),
                    PdfFilenameNoncompetitiveResults = table.Column<string>(type: "TEXT", nullable: true),
                    PdfFilenameSpecialAnnouncement = table.Column<string>(type: "TEXT", nullable: true),
                    PricePer100 = table.Column<string>(type: "TEXT", nullable: true),
                    PrimaryDealerAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    PrimaryDealerTendered = table.Column<string>(type: "TEXT", nullable: true),
                    RefCpiOnDatedDate = table.Column<string>(type: "TEXT", nullable: true),
                    RefCpiOnIssueDate = table.Column<string>(type: "TEXT", nullable: true),
                    Reopening = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityTerm = table.Column<string>(type: "TEXT", nullable: false),
                    SecurityTermDayMonth = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityTermWeekYear = table.Column<string>(type: "TEXT", nullable: true),
                    SecurityType = table.Column<int>(type: "INTEGER", nullable: false),
                    Series = table.Column<string>(type: "TEXT", nullable: true),
                    SomaAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    SomaHoldings = table.Column<string>(type: "TEXT", nullable: true),
                    SomaIncluded = table.Column<string>(type: "TEXT", nullable: true),
                    SomaTendered = table.Column<string>(type: "TEXT", nullable: true),
                    Spread = table.Column<string>(type: "TEXT", nullable: true),
                    StandardInterestPaymentPer1000 = table.Column<string>(type: "TEXT", nullable: true),
                    Strippable = table.Column<string>(type: "TEXT", nullable: true),
                    TiinConversionFactorPer1000 = table.Column<string>(type: "TEXT", nullable: true),
                    TintCusip1 = table.Column<string>(type: "TEXT", nullable: true),
                    TintCusip2 = table.Column<string>(type: "TEXT", nullable: true),
                    Tips = table.Column<string>(type: "TEXT", nullable: true),
                    TotalAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    TotalTendered = table.Column<string>(type: "TEXT", nullable: true),
                    TreasuryDirectAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    TreasuryDirectTendersAccepted = table.Column<string>(type: "TEXT", nullable: true),
                    UnadjustedAccruedInterestPer1000 = table.Column<string>(type: "TEXT", nullable: true),
                    UnadjustedPrice = table.Column<string>(type: "TEXT", nullable: true),
                    XmlFilenameAnnouncement = table.Column<string>(type: "TEXT", nullable: true),
                    XmlFilenameCompetitiveResults = table.Column<string>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Term = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treasuries", x => new { x.Cusip, x.IssueDate });
                });

            migrationBuilder.CreateTable(
                name: "Investments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TreasuryCusip = table.Column<string>(type: "TEXT", nullable: false),
                    TreasuryIssueDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Par = table.Column<int>(type: "INTEGER", nullable: false),
                    Confirmation = table.Column<string>(type: "TEXT", nullable: true),
                    Reinvestments = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investments_Treasuries_TreasuryCusip_TreasuryIssueDate",
                        columns: x => new { x.TreasuryCusip, x.TreasuryIssueDate },
                        principalTable: "Treasuries",
                        principalColumns: new[] { "Cusip", "IssueDate" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Treasuries",
                columns: new[] { "Cusip", "IssueDate", "AccruedInterestPer100", "AccruedInterestPer1000", "AdjustedAccruedInterestPer1000", "AdjustedPrice", "AllocationPercentage", "AllocationPercentageDecimals", "AnnouncedCusip", "AnnouncementDate", "AuctionDate", "AuctionDateYear", "AuctionFormat", "AverageMedianDiscountMargin", "AverageMedianDiscountRate", "AverageMedianInvestmentRate", "AverageMedianPrice", "AverageMedianYield", "BackDated", "BackDatedDate", "BidToCoverRatio", "CallDate", "Callable", "CalledDate", "CashManagementBillCmb", "ClosingTimeCompetitive", "ClosingTimeNoncompetitive", "CompetitiveAccepted", "CompetitiveBidDecimals", "CompetitiveTendered", "CompetitiveTendersAccepted", "CorpusCusip", "CpiBaseReferencePeriod", "CurrentlyOutstanding", "DatedDate", "DirectBidderAccepted", "DirectBidderTendered", "EstimatedAmountOfPubliclyHeldMaturingSecuritiesByType", "FimaIncluded", "FimaNoncompetitiveAccepted", "FimaNoncompetitiveTendered", "FirstInterestPaymentDate", "FirstInterestPeriod", "FloatingRate", "FrnIndexDeterminationDate", "FrnIndexDeterminationRate", "HighDiscountMargin", "HighDiscountRate", "HighInvestmentRate", "HighPrice", "HighYield", "IndexRatioOnIssueDate", "IndirectBidderAccepted", "IndirectBidderTendered", "InterestPaymentFrequency", "InterestRate", "LowDiscountMargin", "LowDiscountRate", "LowInvestmentRate", "LowPrice", "LowYield", "MaturingDate", "MaturityDate", "MaximumCompetitiveAward", "MaximumNoncompetitiveAward", "MaximumSingleBid", "MinimumBidAmount", "MinimumStripAmount", "MinimumToIssue", "MultiplesToBid", "MultiplesToIssue", "NlpExclusionAmount", "NlpReportingThreshold", "NoncompetitiveAccepted", "NoncompetitiveTendersAccepted", "OfferingAmount", "OriginalCusip", "OriginalDatedDate", "OriginalIssueDate", "OriginalSecurityTerm", "PdfFilenameAnnouncement", "PdfFilenameCompetitiveResults", "PdfFilenameNoncompetitiveResults", "PdfFilenameSpecialAnnouncement", "PricePer100", "PrimaryDealerAccepted", "PrimaryDealerTendered", "RefCpiOnDatedDate", "RefCpiOnIssueDate", "Reopening", "SecurityTerm", "SecurityTermDayMonth", "SecurityTermWeekYear", "SecurityType", "Series", "SomaAccepted", "SomaHoldings", "SomaIncluded", "SomaTendered", "Spread", "StandardInterestPaymentPer1000", "Strippable", "Term", "TiinConversionFactorPer1000", "TintCusip1", "TintCusip2", "Tips", "TotalAccepted", "TotalTendered", "TreasuryDirectAccepted", "TreasuryDirectTendersAccepted", "Type", "UnadjustedAccruedInterestPer1000", "UnadjustedPrice", "XmlFilenameAnnouncement", "XmlFilenameCompetitiveResults" },
                values: new object[,]
                {
                    { "912797GK7", new DateOnly(2024, 5, 9), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new DateOnly(2024, 8, 8), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "13-Week", null, null, 0, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 0, null, null, null, null },
                    { "912797GL5", new DateOnly(2024, 6, 6), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new DateOnly(2024, 9, 5), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "13-Week", null, null, 0, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 0, null, null, null, null },
                    { "912797GL5", new DateOnly(2024, 7, 25), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new DateOnly(2024, 9, 5), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "42-Day", null, null, 0, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 0, null, null, null, null },
                    { "912797KX4", new DateOnly(2024, 6, 18), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new DateOnly(2024, 8, 13), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, "8-Week", null, null, 0, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, 0, null, null, null, null }
                });

            migrationBuilder.InsertData(
                table: "Investments",
                columns: new[] { "Id", "Confirmation", "Par", "Reinvestments", "TreasuryCusip", "TreasuryIssueDate" },
                values: new object[,]
                {
                    { 1, "FOO", 100000, 0, "912797GL5", new DateOnly(2024, 7, 25) },
                    { 2, "BAR", 55000, 0, "912797KX4", new DateOnly(2024, 6, 18) },
                    { 3, "BAZ", 2000400, 0, "912797GK7", new DateOnly(2024, 5, 9) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Investments_TreasuryCusip_TreasuryIssueDate",
                table: "Investments",
                columns: new[] { "TreasuryCusip", "TreasuryIssueDate" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investments");

            migrationBuilder.DropTable(
                name: "Treasuries");
        }
    }
}
