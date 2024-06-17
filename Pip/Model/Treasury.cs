// Root myDeserializedClass = JsonSerializer.Deserialize<Root>(myJsonResponse);

using System.Text.Json.Serialization;

namespace Pip.Model;

public class Treasury
{
    [JsonPropertyName("accruedInterestPer1000")]
    public string? AccruedInterestPer1000 { get; set; }

    [JsonPropertyName("accruedInterestPer100")]
    public string? AccruedInterestPer100 { get; set; }

    [JsonPropertyName("adjustedAccruedInterestPer1000")]
    public string? AdjustedAccruedInterestPer1000 { get; set; }

    [JsonPropertyName("adjustedPrice")] public string? AdjustedPrice { get; set; }

    [JsonPropertyName("allocationPercentage")]
    public string? AllocationPercentage { get; set; }

    [JsonPropertyName("allocationPercentageDecimals")]
    public string? AllocationPercentageDecimals { get; set; }

    [JsonPropertyName("announcedCusip")] public string? AnnouncedCusip { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("announcementDate")]
    public DateTime? AnnouncementDate { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("auctionDate")]
    public DateTime? AuctionDate { get; set; }

    [JsonPropertyName("auctionDateYear")] public string? AuctionDateYear { get; set; }

    [JsonPropertyName("auctionFormat")] public string? AuctionFormat { get; set; }

    [JsonPropertyName("averageMedianDiscountRate")]
    public string? AverageMedianDiscountRate { get; set; }

    [JsonPropertyName("averageMedianInvestmentRate")]
    public string? AverageMedianInvestmentRate { get; set; }

    [JsonPropertyName("averageMedianPrice")]
    public string? AverageMedianPrice { get; set; }

    [JsonPropertyName("averageMedianDiscountMargin")]
    public string? AverageMedianDiscountMargin { get; set; }

    [JsonPropertyName("averageMedianYield")]
    public string? AverageMedianYield { get; set; }

    [JsonPropertyName("backDated")] public string? BackDated { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("backDatedDate")]
    public DateTime? BackDatedDate { get; set; }

    [JsonPropertyName("bidToCoverRatio")] public string? BidToCoverRatio { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("callDate")]
    public DateTime? CallDate { get; set; }

    [JsonPropertyName("callable")] public string? Callable { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("calledDate")]
    public DateTime? CalledDate { get; set; }

    [JsonPropertyName("cashManagementBillCMB")]
    public string? CashManagementBillCMB { get; set; }

    [JsonPropertyName("closingTimeCompetitive")]
    public string? ClosingTimeCompetitive { get; set; }

    [JsonPropertyName("closingTimeNoncompetitive")]
    public string? ClosingTimeNoncompetitive { get; set; }

    [JsonPropertyName("competitiveAccepted")]
    public string? CompetitiveAccepted { get; set; }

    [JsonPropertyName("competitiveBidDecimals")]
    public string? CompetitiveBidDecimals { get; set; }

    [JsonPropertyName("competitiveTendered")]
    public string? CompetitiveTendered { get; set; }

    [JsonPropertyName("competitiveTendersAccepted")]
    public string? CompetitiveTendersAccepted { get; set; }

    [JsonPropertyName("corpusCusip")] public string? CorpusCusip { get; set; }

    [JsonPropertyName("cpiBaseReferencePeriod")]
    public string? CpiBaseReferencePeriod { get; set; }

    [JsonPropertyName("currentlyOutstanding")]
    public string? CurrentlyOutstanding { get; set; }

    [JsonPropertyName("cusip")] public string? Cusip { get; set; }

    [JsonPropertyName("datedDate")]
    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    public DateTime? DatedDate { get; set; }

    [JsonPropertyName("directBidderAccepted")]
    public string? DirectBidderAccepted { get; set; }

    [JsonPropertyName("directBidderTendered")]
    public string? DirectBidderTendered { get; set; }

    [JsonPropertyName("estimatedAmountOfPubliclyHeldMaturingSecuritiesByType")]
    public string? EstimatedAmountOfPubliclyHeldMaturingSecuritiesByType { get; set; }

    [JsonPropertyName("fimaIncluded")] public string? FimaIncluded { get; set; }

    [JsonPropertyName("fimaNoncompetitiveAccepted")]
    public string? FimaNoncompetitiveAccepted { get; set; }

    [JsonPropertyName("fimaNoncompetitiveTendered")]
    public string? FimaNoncompetitiveTendered { get; set; }

    [JsonPropertyName("firstInterestPeriod")]
    public string? FirstInterestPeriod { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("firstInterestPaymentDate")]
    public DateTime? FirstInterestPaymentDate { get; set; }

    [JsonPropertyName("floatingRate")] public string? FloatingRate { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("frnIndexDeterminationDate")]
    public DateTime? FrnIndexDeterminationDate { get; set; }

    [JsonPropertyName("frnIndexDeterminationRate")]
    public string? FrnIndexDeterminationRate { get; set; }

    [JsonPropertyName("highDiscountRate")] public string? HighDiscountRate { get; set; }

    [JsonPropertyName("highInvestmentRate")]
    public string? HighInvestmentRate { get; set; }

    [JsonPropertyName("highPrice")] public string? HighPrice { get; set; }

    [JsonPropertyName("highDiscountMargin")]
    public string? HighDiscountMargin { get; set; }

    [JsonPropertyName("highYield")] public string? HighYield { get; set; }

    [JsonPropertyName("indexRatioOnIssueDate")]
    public string? IndexRatioOnIssueDate { get; set; }

    [JsonPropertyName("indirectBidderAccepted")]
    public string? IndirectBidderAccepted { get; set; }

    [JsonPropertyName("indirectBidderTendered")]
    public string? IndirectBidderTendered { get; set; }

    [JsonPropertyName("interestPaymentFrequency")]
    public string? InterestPaymentFrequency { get; set; }

    [JsonPropertyName("interestRate")] public string? InterestRate { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("issueDate")]
    public DateTime? IssueDate { get; set; }

    [JsonPropertyName("lowDiscountRate")] public string? LowDiscountRate { get; set; }

    [JsonPropertyName("lowInvestmentRate")]
    public string? LowInvestmentRate { get; set; }

    [JsonPropertyName("lowPrice")] public string? LowPrice { get; set; }

    [JsonPropertyName("lowDiscountMargin")]
    public string? LowDiscountMargin { get; set; }

    [JsonPropertyName("lowYield")] public string? LowYield { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("maturingDate")]
    public DateTime? MaturingDate { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("maturityDate")]
    public DateTime? MaturityDate { get; set; }

    [JsonPropertyName("maximumCompetitiveAward")]
    public string? MaximumCompetitiveAward { get; set; }

    [JsonPropertyName("maximumNoncompetitiveAward")]
    public string? MaximumNoncompetitiveAward { get; set; }

    [JsonPropertyName("maximumSingleBid")] public string? MaximumSingleBid { get; set; }

    [JsonPropertyName("minimumBidAmount")] public string? MinimumBidAmount { get; set; }

    [JsonPropertyName("minimumStripAmount")]
    public string? MinimumStripAmount { get; set; }

    [JsonPropertyName("minimumToIssue")] public string? MinimumToIssue { get; set; }

    [JsonPropertyName("multiplesToBid")] public string? MultiplesToBid { get; set; }

    [JsonPropertyName("multiplesToIssue")] public string? MultiplesToIssue { get; set; }

    [JsonPropertyName("nlpExclusionAmount")]
    public string? NlpExclusionAmount { get; set; }

    [JsonPropertyName("nlpReportingThreshold")]
    public string? NlpReportingThreshold { get; set; }

    [JsonPropertyName("noncompetitiveAccepted")]
    public string? NoncompetitiveAccepted { get; set; }

    [JsonPropertyName("noncompetitiveTendersAccepted")]
    public string? NoncompetitiveTendersAccepted { get; set; }

    [JsonPropertyName("offeringAmount")] public string? OfferingAmount { get; set; }

    [JsonPropertyName("originalCusip")] public string? OriginalCusip { get; set; }

    [JsonConverter(typeof(EmptyStringConverter<DateTime>))]
    [JsonPropertyName("originalDatedDate")]
    public DateTime? OriginalDatedDate { get; set; }

    [JsonPropertyName("originalIssueDate")]
    public DateTime? OriginalIssueDate { get; set; }

    [JsonPropertyName("originalSecurityTerm")]
    public string? OriginalSecurityTerm { get; set; }

    [JsonPropertyName("pdfFilenameAnnouncement")]
    public string? PdfFilenameAnnouncement { get; set; }

    [JsonPropertyName("pdfFilenameCompetitiveResults")]
    public string? PdfFilenameCompetitiveResults { get; set; }

    [JsonPropertyName("pdfFilenameNoncompetitiveResults")]
    public string? PdfFilenameNoncompetitiveResults { get; set; }

    [JsonPropertyName("pdfFilenameSpecialAnnouncement")]
    public string? PdfFilenameSpecialAnnouncement { get; set; }

    [JsonPropertyName("pricePer100")] public string? PricePer100 { get; set; }

    [JsonPropertyName("primaryDealerAccepted")]
    public string? PrimaryDealerAccepted { get; set; }

    [JsonPropertyName("primaryDealerTendered")]
    public string? PrimaryDealerTendered { get; set; }

    [JsonPropertyName("refCPIOnDatedDate")]
    public string? RefCPIOnDatedDate { get; set; }

    [JsonPropertyName("refCpiOnIssueDate")]
    public string? RefCpiOnIssueDate { get; set; }

    [JsonPropertyName("reopening")] public string? Reopening { get; set; }

    [JsonPropertyName("securityTerm")] public string? SecurityTerm { get; set; }

    [JsonPropertyName("securityTermDayMonth")]
    public string? SecurityTermDayMonth { get; set; }

    [JsonPropertyName("securityTermWeekYear")]
    public string? SecurityTermWeekYear { get; set; }

    [JsonPropertyName("securityType")] public string? SecurityType { get; set; }

    [JsonPropertyName("series")] public string? Series { get; set; }

    [JsonPropertyName("somaAccepted")] public string? SomaAccepted { get; set; }

    [JsonPropertyName("somaHoldings")] public string? SomaHoldings { get; set; }

    [JsonPropertyName("somaIncluded")] public string? SomaIncluded { get; set; }

    [JsonPropertyName("somaTendered")] public string? SomaTendered { get; set; }

    [JsonPropertyName("spread")] public string? Spread { get; set; }

    [JsonPropertyName("standardInterestPaymentPer1000")]
    public string? StandardInterestPaymentPer1000 { get; set; }

    [JsonPropertyName("strippable")] public string? Strippable { get; set; }

    [JsonPropertyName("tiinConversionFactorPer1000")]
    public string? TiinConversionFactorPer1000 { get; set; }

    [JsonPropertyName("tintCusip1")] public string? TintCusip1 { get; set; }

    [JsonPropertyName("tintCusip2")] public string? TintCusip2 { get; set; }

    [JsonPropertyName("tips")] public string? Tips { get; set; }

    [JsonPropertyName("totalAccepted")] public string? TotalAccepted { get; set; }

    [JsonPropertyName("totalTendered")] public string? TotalTendered { get; set; }

    [JsonPropertyName("treasuryDirectAccepted")]
    public string? TreasuryDirectAccepted { get; set; }

    [JsonPropertyName("treasuryDirectTendersAccepted")]
    public string? TreasuryDirectTendersAccepted { get; set; }

    [JsonPropertyName("unadjustedAccruedInterestPer1000")]
    public string? UnadjustedAccruedInterestPer1000 { get; set; }

    [JsonPropertyName("unadjustedPrice")] public string? UnadjustedPrice { get; set; }

    [JsonPropertyName("xmlFilenameAnnouncement")]
    public string? XmlFilenameAnnouncement { get; set; }

    [JsonPropertyName("xmlFilenameCompetitiveResults")]
    public string? XmlFilenameCompetitiveResults { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("term")] public string? Term { get; set; }
}
