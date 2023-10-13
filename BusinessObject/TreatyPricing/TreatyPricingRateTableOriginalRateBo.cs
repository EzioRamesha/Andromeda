using System;

namespace BusinessObject.TreatyPricing
{
    public class TreatyPricingRateTableOriginalRateBo
    {
        public int Id { get; set; }

        public int TreatyPricingRateTableVersionId { get; set; }

        public TreatyPricingRateTableVersionBo TreatyPricingRateTableVersionBo { get; set; }

        public int Age { get; set; }

        public double? MaleNonSmoker { get; set; }

        public double? MaleSmoker { get; set; }

        public double? FemaleNonSmoker { get; set; }

        public double? FemaleSmoker { get; set; }

        public double? Male { get; set; }

        public double? Female { get; set; }

        public double? Unisex { get; set; }

        public double? UnitRate { get; set; }

        public double? OccupationClass { get; set; }

        // string
        public string MaleNonSmokerStr { get; set; }

        public string MaleSmokerStr { get; set; }

        public string FemaleNonSmokerStr { get; set; }

        public string FemaleSmokerStr { get; set; }

        public string MaleStr { get; set; }

        public string FemaleStr { get; set; }

        public string UnisexStr { get; set; }

        public string UnitRateStr { get; set; }

        public string OccupationClassStr { get; set; }

        public int CreatedById { get; set; }

        public DateTime CreatedAt { get; set; }

        public int? UpdatedById { get; set; }

        public static string[] InsertFields()
        {
            return new string[]
            {
                "[TreatyPricingRateTableVersionId]",
                "[Age]",
                "[MaleNonSmoker]",
                "[MaleSmoker]",
                "[FemaleNonSmoker]",
                "[FemaleSmoker]",
                "[Male]",
                "[Female]",
                "[Unisex]",
                "[UnitRate]",
                "[OccupationClass]",
                "[CreatedAt]",
                "[UpdatedAt]",
                "[UpdatedById]",
                "[CreatedById]",
            };
        }

        public static string[] QueryFields()
        {
            return new string[]
            {
                "[Age]",
                "[MaleNonSmoker]",
                "[MaleSmoker]",
                "[FemaleNonSmoker]",
                "[FemaleSmoker]",
                "[Male]",
                "[Female]",
                "[Unisex]",
                "[UnitRate]",
                "[OccupationClass]",
            };
        }

        public bool IsEmpty()
        {
            if ((!MaleNonSmoker.HasValue || MaleNonSmoker == 0) &&
                (!MaleSmoker.HasValue || MaleSmoker == 0) &&
                (!FemaleNonSmoker.HasValue || FemaleNonSmoker == 0) &&
                (!FemaleSmoker.HasValue || FemaleSmoker == 0) &&
                (!Male.HasValue || Male == 0) &&
                (!Female.HasValue || Female == 0) &&
                (!Unisex.HasValue || Unisex == 0) &&
                (!UnitRate.HasValue || UnitRate == 0) &&
                (!OccupationClass.HasValue || OccupationClass == 0))
                return true;
            return false;
        }

        public static TreatyPricingRateTableOriginalRateBo Clone(TreatyPricingRateTableOriginalRateBo bo = null)
        {
            if (bo == null)
                return new TreatyPricingRateTableOriginalRateBo();

            return new TreatyPricingRateTableOriginalRateBo
            {
                MaleNonSmoker = bo.MaleNonSmoker,
                MaleSmoker = bo.MaleSmoker,
                FemaleNonSmoker = bo.FemaleNonSmoker,
                FemaleSmoker = bo.FemaleSmoker,
                Male = bo.Male,
                Female = bo.Female,
                Unisex = bo.Unisex,
                UnitRate = bo.UnitRate,
                OccupationClass = bo.OccupationClass,
            };
        }

        public void SetDefaultRate(TreatyPricingRateTableOriginalRateBo currentRateBo)
        {
            MaleNonSmoker = 0;
            MaleSmoker = 0;
            FemaleNonSmoker = 0;
            FemaleSmoker = 0;
            Male = 0;
            Female = 0;
            Unisex = 0;
            UnitRate = currentRateBo?.UnitRate ?? 0;
            OccupationClass = currentRateBo?.OccupationClass ?? 0;
        }

        public void ConvertRateToANxB(string ageBasis, TreatyPricingRateTableOriginalRateBo previousRateBo, TreatyPricingRateTableOriginalRateBo currentRateBo)
        {
            if (string.IsNullOrEmpty(ageBasis) || ageBasis.Equals(PickListDetailBo.AgeBasisANxB))
                return;

            if (ageBasis.Equals(PickListDetailBo.AgeBasisANrB))
            {
                if (Age == 0)
                    SetDefaultRate(currentRateBo);

                if (Age >= 1 && Age <= 100)
                {
                    MaleNonSmoker = (previousRateBo.MaleNonSmoker + currentRateBo.MaleNonSmoker) / 2;
                    MaleSmoker = (previousRateBo.MaleSmoker + currentRateBo.MaleSmoker) / 2;
                    FemaleNonSmoker = (previousRateBo.FemaleNonSmoker + currentRateBo.FemaleNonSmoker) / 2;
                    FemaleSmoker = (previousRateBo.FemaleSmoker + currentRateBo.FemaleSmoker) / 2;
                    Male = (previousRateBo.Male + currentRateBo.Male) / 2;
                    Female = (previousRateBo.Female + currentRateBo.Female) / 2;
                    Unisex = (previousRateBo.Unisex + currentRateBo.Unisex) / 2;
                    UnitRate = currentRateBo.UnitRate;
                    OccupationClass = currentRateBo.OccupationClass;
                }
            }
            else
            {
                if (Age == 0)
                    SetDefaultRate(currentRateBo);

                if (Age >= 1 && Age <= 100)
                {
                    MaleNonSmoker = previousRateBo.MaleNonSmoker;
                    MaleSmoker = previousRateBo.MaleSmoker;
                    FemaleNonSmoker = previousRateBo.FemaleNonSmoker;
                    FemaleSmoker = previousRateBo.FemaleSmoker;
                    Male = previousRateBo.Male;
                    Female = previousRateBo.Female;
                    Unisex = previousRateBo.Unisex;
                    UnitRate = currentRateBo.UnitRate;
                    OccupationClass = currentRateBo.OccupationClass;
                }
            }
        }

        public void ConvertSmokerAggregatedRate()
        {
            if (!Male.HasValue && (MaleNonSmoker.HasValue || MaleSmoker.HasValue))
            {
                var mns = MaleNonSmoker.HasValue ? MaleNonSmoker * 0.7 : 0;
                var ms = MaleSmoker.HasValue ? MaleSmoker * 0.3 : 0;
                Male = mns + ms;
            }

            if (!Female.HasValue && (FemaleNonSmoker.HasValue || FemaleSmoker.HasValue))
            {
                var fns = FemaleNonSmoker.HasValue ? FemaleNonSmoker * 0.87 : 0;
                var fs = FemaleSmoker.HasValue ? FemaleSmoker * 0.13 : 0;
                Female = fns + fs;
            }

            if (!Unisex.HasValue && (Male.HasValue || Female.HasValue))
            {
                var m = Male.HasValue ? Male * 0.7 : 0;
                var f = Female.HasValue ? Female * 0.3 : 0;
                Unisex = m + f;
            }
        }
    }
}
