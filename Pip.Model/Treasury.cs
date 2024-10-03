using Microsoft.EntityFrameworkCore;
using Pip.Model.Converters;
using System.Text.Json.Serialization;

namespace Pip.Model;

[PrimaryKey(nameof(Cusip), nameof(IssueDate))]
public class Treasury : IEquatable<Treasury>
{
	[JsonPropertyName("accruedInterestPer1000")]
	public string? AccruedInterestPer1000 { get; init; }

	[JsonPropertyName("accruedInterestPer100")]
	public string? AccruedInterestPer100 { get; init; }

	[JsonPropertyName("adjustedAccruedInterestPer1000")]
	public string? AdjustedAccruedInterestPer1000 { get; init; }

	[JsonPropertyName("adjustedPrice")] public string? AdjustedPrice { get; init; }

	[JsonPropertyName("allocationPercentage")]
	public string? AllocationPercentage { get; init; }

	[JsonPropertyName("allocationPercentageDecimals")]
	public string? AllocationPercentageDecimals { get; init; }

	[JsonPropertyName("announcedCusip")] public string? AnnouncedCusip { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("announcementDate")]
	public DateOnly? AnnouncementDate { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("auctionDate")]
	public DateOnly? AuctionDate { get; init; }

	[JsonPropertyName("auctionDateYear")] public string? AuctionDateYear { get; init; }

	[JsonPropertyName("auctionFormat")] public string? AuctionFormat { get; init; }

	[JsonPropertyName("averageMedianDiscountRate")]
	public string? AverageMedianDiscountRate { get; init; }

	[JsonPropertyName("averageMedianInvestmentRate")]
	public string? AverageMedianInvestmentRate { get; init; }

	[JsonPropertyName("averageMedianPrice")]
	public string? AverageMedianPrice { get; init; }

	[JsonPropertyName("averageMedianDiscountMargin")]
	public string? AverageMedianDiscountMargin { get; init; }

	[JsonPropertyName("averageMedianYield")]
	public string? AverageMedianYield { get; init; }

	[JsonPropertyName("backDated")] public string? BackDated { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("backDatedDate")]
	public DateOnly? BackDatedDate { get; init; }

	[JsonPropertyName("bidToCoverRatio")] public string? BidToCoverRatio { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("callDate")]
	public DateOnly? CallDate { get; init; }

	[JsonPropertyName("callable")] public string? Callable { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("calledDate")]
	public DateOnly? CalledDate { get; init; }

	[JsonPropertyName("cashManagementBillCMB")]
	public string? CashManagementBillCmb { get; init; }

	[JsonPropertyName("closingTimeCompetitive")]
	public string? ClosingTimeCompetitive { get; init; }

	[JsonPropertyName("closingTimeNoncompetitive")]
	public string? ClosingTimeNoncompetitive { get; init; }

	[JsonPropertyName("competitiveAccepted")]
	public string? CompetitiveAccepted { get; init; }

	[JsonPropertyName("competitiveBidDecimals")]
	public string? CompetitiveBidDecimals { get; init; }

	[JsonPropertyName("competitiveTendered")]
	public string? CompetitiveTendered { get; init; }

	[JsonPropertyName("competitiveTendersAccepted")]
	public string? CompetitiveTendersAccepted { get; init; }

	[JsonPropertyName("corpusCusip")] public string? CorpusCusip { get; init; }

	[JsonPropertyName("cpiBaseReferencePeriod")]
	public string? CpiBaseReferencePeriod { get; init; }

	[JsonPropertyName("currentlyOutstanding")]
	public string? CurrentlyOutstanding { get; init; }

	[JsonPropertyName("cusip")] public required string Cusip { get; init; }

	[JsonPropertyName("datedDate")]
	[JsonConverter(typeof(DateOnlyConverter))]
	public DateOnly? DatedDate { get; init; }

	[JsonPropertyName("directBidderAccepted")]
	public string? DirectBidderAccepted { get; init; }

	[JsonPropertyName("directBidderTendered")]
	public string? DirectBidderTendered { get; init; }

	[JsonPropertyName("estimatedAmountOfPubliclyHeldMaturingSecuritiesByType")]
	public string? EstimatedAmountOfPubliclyHeldMaturingSecuritiesByType { get; init; }

	[JsonPropertyName("fimaIncluded")] public string? FimaIncluded { get; init; }

	[JsonPropertyName("fimaNoncompetitiveAccepted")]
	public string? FimaNoncompetitiveAccepted { get; init; }

	[JsonPropertyName("fimaNoncompetitiveTendered")]
	public string? FimaNoncompetitiveTendered { get; init; }

	[JsonPropertyName("firstInterestPeriod")]
	public string? FirstInterestPeriod { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("firstInterestPaymentDate")]
	public DateOnly? FirstInterestPaymentDate { get; init; }

	[JsonPropertyName("floatingRate")] public string? FloatingRate { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("frnIndexDeterminationDate")]
	public DateOnly? FrnIndexDeterminationDate { get; init; }

	[JsonPropertyName("frnIndexDeterminationRate")]
	public string? FrnIndexDeterminationRate { get; init; }

	[JsonPropertyName("highDiscountRate")] public string? HighDiscountRate { get; init; }

	[JsonPropertyName("highInvestmentRate")]
	public string? HighInvestmentRate { get; init; }

	[JsonPropertyName("highPrice")] public string? HighPrice { get; init; }

	[JsonPropertyName("highDiscountMargin")]
	public string? HighDiscountMargin { get; init; }

	[JsonPropertyName("highYield")] public string? HighYield { get; init; }

	[JsonPropertyName("indexRatioOnIssueDate")]
	public string? IndexRatioOnIssueDate { get; init; }

	[JsonPropertyName("indirectBidderAccepted")]
	public string? IndirectBidderAccepted { get; init; }

	[JsonPropertyName("indirectBidderTendered")]
	public string? IndirectBidderTendered { get; init; }

	[JsonPropertyName("interestPaymentFrequency")]
	public string? InterestPaymentFrequency { get; init; }

	[JsonPropertyName("interestRate")] public string? InterestRate { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("issueDate")]
	public DateOnly? IssueDate { get; init; }

	[JsonPropertyName("lowDiscountRate")] public string? LowDiscountRate { get; init; }

	[JsonPropertyName("lowInvestmentRate")]
	public string? LowInvestmentRate { get; init; }

	[JsonPropertyName("lowPrice")] public string? LowPrice { get; init; }

	[JsonPropertyName("lowDiscountMargin")]
	public string? LowDiscountMargin { get; init; }

	[JsonPropertyName("lowYield")] public string? LowYield { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("maturingDate")]
	public DateOnly? MaturingDate { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("maturityDate")]
	public DateOnly? MaturityDate { get; init; }

	[JsonPropertyName("maximumCompetitiveAward")]
	public string? MaximumCompetitiveAward { get; init; }

	[JsonPropertyName("maximumNoncompetitiveAward")]
	public string? MaximumNoncompetitiveAward { get; init; }

	[JsonPropertyName("maximumSingleBid")] public string? MaximumSingleBid { get; init; }

	[JsonPropertyName("minimumBidAmount")] public string? MinimumBidAmount { get; init; }

	[JsonPropertyName("minimumStripAmount")]
	public string? MinimumStripAmount { get; init; }

	[JsonPropertyName("minimumToIssue")] public string? MinimumToIssue { get; init; }

	[JsonPropertyName("multiplesToBid")] public string? MultiplesToBid { get; init; }

	[JsonPropertyName("multiplesToIssue")] public string? MultiplesToIssue { get; init; }

	[JsonPropertyName("nlpExclusionAmount")]
	public string? NlpExclusionAmount { get; init; }

	[JsonPropertyName("nlpReportingThreshold")]
	public string? NlpReportingThreshold { get; init; }

	[JsonPropertyName("noncompetitiveAccepted")]
	public string? NoncompetitiveAccepted { get; init; }

	[JsonPropertyName("noncompetitiveTendersAccepted")]
	public string? NoncompetitiveTendersAccepted { get; init; }

	[JsonPropertyName("offeringAmount")] public string? OfferingAmount { get; init; }

	[JsonPropertyName("originalCusip")] public string? OriginalCusip { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("originalDatedDate")]
	public DateOnly? OriginalDatedDate { get; init; }

	[JsonConverter(typeof(DateOnlyConverter))]
	[JsonPropertyName("originalIssueDate")]
	public DateOnly? OriginalIssueDate { get; init; }

	[JsonPropertyName("originalSecurityTerm")]
	public string? OriginalSecurityTerm { get; init; }

	[JsonPropertyName("pdfFilenameAnnouncement")]
	public string? PdfFilenameAnnouncement { get; init; }

	[JsonPropertyName("pdfFilenameCompetitiveResults")]
	public string? PdfFilenameCompetitiveResults { get; init; }

	[JsonPropertyName("pdfFilenameNoncompetitiveResults")]
	public string? PdfFilenameNoncompetitiveResults { get; init; }

	[JsonPropertyName("pdfFilenameSpecialAnnouncement")]
	public string? PdfFilenameSpecialAnnouncement { get; init; }

	[JsonPropertyName("pricePer100")] public string? PricePer100 { get; init; }

	[JsonPropertyName("primaryDealerAccepted")]
	public string? PrimaryDealerAccepted { get; init; }

	[JsonPropertyName("primaryDealerTendered")]
	public string? PrimaryDealerTendered { get; init; }

	[JsonPropertyName("refCPIOnDatedDate")]
	public string? RefCpiOnDatedDate { get; init; }

	[JsonPropertyName("refCpiOnIssueDate")]
	public string? RefCpiOnIssueDate { get; init; }

	[JsonPropertyName("reopening")] public string? Reopening { get; init; }

	[JsonPropertyName("securityTerm")] public required string SecurityTerm { get; init; }

	[JsonPropertyName("securityTermDayMonth")]
	public string? SecurityTermDayMonth { get; init; }

	[JsonPropertyName("securityTermWeekYear")]
	public string? SecurityTermWeekYear { get; init; }

	[JsonPropertyName("securityType")] public required TreasurySecurityType SecurityType { get; init; }

	[JsonPropertyName("series")] public string? Series { get; init; }

	[JsonPropertyName("somaAccepted")] public string? SomaAccepted { get; init; }

	[JsonPropertyName("somaHoldings")] public string? SomaHoldings { get; init; }

	[JsonPropertyName("somaIncluded")] public string? SomaIncluded { get; init; }

	[JsonPropertyName("somaTendered")] public string? SomaTendered { get; init; }

	[JsonPropertyName("spread")] public string? Spread { get; init; }

	[JsonPropertyName("standardInterestPaymentPer1000")]
	public string? StandardInterestPaymentPer1000 { get; init; }

	[JsonPropertyName("strippable")] public string? Strippable { get; init; }

	[JsonPropertyName("tiinConversionFactorPer1000")]
	public string? TiinConversionFactorPer1000 { get; init; }

	[JsonPropertyName("tintCusip1")] public string? TintCusip1 { get; init; }

	[JsonPropertyName("tintCusip2")] public string? TintCusip2 { get; init; }

	[JsonPropertyName("tips")] public string? Tips { get; init; }

	[JsonPropertyName("totalAccepted")] public string? TotalAccepted { get; init; }

	[JsonPropertyName("totalTendered")] public string? TotalTendered { get; init; }

	[JsonPropertyName("treasuryDirectAccepted")]
	public string? TreasuryDirectAccepted { get; init; }

	[JsonPropertyName("treasuryDirectTendersAccepted")]
	public string? TreasuryDirectTendersAccepted { get; init; }

	[JsonPropertyName("unadjustedAccruedInterestPer1000")]
	public string? UnadjustedAccruedInterestPer1000 { get; init; }

	[JsonPropertyName("unadjustedPrice")] public string? UnadjustedPrice { get; init; }

	[JsonPropertyName("xmlFilenameAnnouncement")]
	public string? XmlFilenameAnnouncement { get; init; }

	[JsonPropertyName("xmlFilenameCompetitiveResults")]
	public string? XmlFilenameCompetitiveResults { get; init; }

	[JsonPropertyName("type")] public required TreasuryType Type { get; init; }

	[JsonPropertyName("term")] public string? Term { get; init; }

	public ICollection<Investment>? Investments { get; init; }

	public bool Equals(Treasury? other)
	{
		return other is not null &&
			   (ReferenceEquals(this, other) || (Cusip == other.Cusip && IssueDate.Equals(other.IssueDate)));
	}

	public override bool Equals(object? obj)
	{
		if (obj is null) return false;
		return ReferenceEquals(this, obj) || (obj.GetType() == GetType() && Equals((Treasury)obj));
	}

	public override int GetHashCode()
	{
		return HashCode.Combine(Cusip, IssueDate);
	}
}

[JsonConverter(typeof(JsonStringEnumConverter<TreasuryType>))]
public enum TreasuryType
{
	Bill,
	Note,
	Bond,
	CMB,
	TIPS,
	FRN
}

[JsonConverter(typeof(JsonStringEnumConverter<TreasurySecurityType>))]
public enum TreasurySecurityType
{
	Bill,
	Note,
	Bond
}
