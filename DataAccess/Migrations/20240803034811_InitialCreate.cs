using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Pip.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Treasury",
                columns: table => new
                {
                    Cusip = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: false),
                    AccruedInterestPer1000 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccruedInterestPer100 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustedAccruedInterestPer1000 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdjustedPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllocationPercentage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllocationPercentageDecimals = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnouncedCusip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AnnouncementDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AuctionDate = table.Column<DateOnly>(type: "date", nullable: true),
                    AuctionDateYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuctionFormat = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageMedianDiscountRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageMedianInvestmentRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageMedianPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageMedianDiscountMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AverageMedianYield = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackDated = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BackDatedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    BidToCoverRatio = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CallDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Callable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CalledDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CashManagementBillCMB = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosingTimeCompetitive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClosingTimeNoncompetitive = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitiveAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitiveBidDecimals = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitiveTendered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompetitiveTendersAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CorpusCusip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CpiBaseReferencePeriod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CurrentlyOutstanding = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DatedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    DirectBidderAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectBidderTendered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedAmountOfPubliclyHeldMaturingSecuritiesByType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FimaIncluded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FimaNoncompetitiveAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FimaNoncompetitiveTendered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstInterestPeriod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirstInterestPaymentDate = table.Column<DateOnly>(type: "date", nullable: true),
                    FloatingRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FrnIndexDeterminationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    FrnIndexDeterminationRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighDiscountRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighInvestmentRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighDiscountMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HighYield = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndexRatioOnIssueDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndirectBidderAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IndirectBidderTendered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterestPaymentFrequency = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InterestRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LowDiscountRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LowInvestmentRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LowPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LowDiscountMargin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LowYield = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaturingDate = table.Column<DateOnly>(type: "date", nullable: true),
                    MaturityDate = table.Column<DateOnly>(type: "date", nullable: true),
                    MaximumCompetitiveAward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaximumNoncompetitiveAward = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaximumSingleBid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumBidAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumStripAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinimumToIssue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultiplesToBid = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MultiplesToIssue = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NlpExclusionAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NlpReportingThreshold = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoncompetitiveAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NoncompetitiveTendersAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferingAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalCusip = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginalDatedDate = table.Column<DateOnly>(type: "date", nullable: true),
                    OriginalIssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    OriginalSecurityTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilenameAnnouncement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilenameCompetitiveResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilenameNoncompetitiveResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PdfFilenameSpecialAnnouncement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePer100 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryDealerAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryDealerTendered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefCPIOnDatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefCpiOnIssueDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Reopening = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityTerm = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityTermDayMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityTermWeekYear = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Series = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SomaAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SomaHoldings = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SomaIncluded = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SomaTendered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Spread = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StandardInterestPaymentPer1000 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Strippable = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TiinConversionFactorPer1000 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TintCusip1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TintCusip2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Tips = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalTendered = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TreasuryDirectAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TreasuryDirectTendersAccepted = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnadjustedAccruedInterestPer1000 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UnadjustedPrice = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XmlFilenameAnnouncement = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    XmlFilenameCompetitiveResults = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Term = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Treasury", x => new { x.Cusip, x.IssueDate });
                });

            migrationBuilder.InsertData(
                table: "Treasury",
                columns: new[] { "Cusip", "IssueDate", "AccruedInterestPer100", "AccruedInterestPer1000", "AdjustedAccruedInterestPer1000", "AdjustedPrice", "AllocationPercentage", "AllocationPercentageDecimals", "AnnouncedCusip", "AnnouncementDate", "AuctionDate", "AuctionDateYear", "AuctionFormat", "AverageMedianDiscountMargin", "AverageMedianDiscountRate", "AverageMedianInvestmentRate", "AverageMedianPrice", "AverageMedianYield", "BackDated", "BackDatedDate", "BidToCoverRatio", "CallDate", "Callable", "CalledDate", "CashManagementBillCMB", "ClosingTimeCompetitive", "ClosingTimeNoncompetitive", "CompetitiveAccepted", "CompetitiveBidDecimals", "CompetitiveTendered", "CompetitiveTendersAccepted", "CorpusCusip", "CpiBaseReferencePeriod", "CurrentlyOutstanding", "DatedDate", "DirectBidderAccepted", "DirectBidderTendered", "EstimatedAmountOfPubliclyHeldMaturingSecuritiesByType", "FimaIncluded", "FimaNoncompetitiveAccepted", "FimaNoncompetitiveTendered", "FirstInterestPaymentDate", "FirstInterestPeriod", "FloatingRate", "FrnIndexDeterminationDate", "FrnIndexDeterminationRate", "HighDiscountMargin", "HighDiscountRate", "HighInvestmentRate", "HighPrice", "HighYield", "IndexRatioOnIssueDate", "IndirectBidderAccepted", "IndirectBidderTendered", "InterestPaymentFrequency", "InterestRate", "LowDiscountMargin", "LowDiscountRate", "LowInvestmentRate", "LowPrice", "LowYield", "MaturingDate", "MaturityDate", "MaximumCompetitiveAward", "MaximumNoncompetitiveAward", "MaximumSingleBid", "MinimumBidAmount", "MinimumStripAmount", "MinimumToIssue", "MultiplesToBid", "MultiplesToIssue", "NlpExclusionAmount", "NlpReportingThreshold", "NoncompetitiveAccepted", "NoncompetitiveTendersAccepted", "OfferingAmount", "OriginalCusip", "OriginalDatedDate", "OriginalIssueDate", "OriginalSecurityTerm", "PdfFilenameAnnouncement", "PdfFilenameCompetitiveResults", "PdfFilenameNoncompetitiveResults", "PdfFilenameSpecialAnnouncement", "PricePer100", "PrimaryDealerAccepted", "PrimaryDealerTendered", "RefCPIOnDatedDate", "RefCpiOnIssueDate", "Reopening", "SecurityTerm", "SecurityTermDayMonth", "SecurityTermWeekYear", "SecurityType", "Series", "SomaAccepted", "SomaHoldings", "SomaIncluded", "SomaTendered", "Spread", "StandardInterestPaymentPer1000", "Strippable", "Term", "TiinConversionFactorPer1000", "TintCusip1", "TintCusip2", "Tips", "TotalAccepted", "TotalTendered", "TreasuryDirectAccepted", "TreasuryDirectTendersAccepted", "Type", "UnadjustedAccruedInterestPer1000", "UnadjustedPrice", "XmlFilenameAnnouncement", "XmlFilenameCompetitiveResults" },
                values: new object[,]
                {
                    { "912797GK7", new DateOnly(2024, 5, 9), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new DateOnly(2024, 8, 8), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
                    { "912797GL5", new DateOnly(2024, 7, 25), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new DateOnly(2024, 9, 5), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null },
                    { "912797KX4", new DateOnly(2024, 6, 18), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, new DateOnly(2024, 8, 13), null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Treasury");
        }
    }
}
