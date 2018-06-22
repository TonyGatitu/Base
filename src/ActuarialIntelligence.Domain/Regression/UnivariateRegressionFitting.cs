﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace ActuarialIntelligence.Domain.Regression
{
    /// <summary>
    /// We know that Γ(r,λ)=1Γ(r)λrxr−1e−λx if x≥0. In this case the likelihood function L is
        //∏iΓ(r,λ)xi=1Γ(r)nλnrxr−11xr−12...xr−1ne−λT
    /// </summary>
    public class UnivariateRegressionFitting
    {
        private readonly IList<decimal> pointsInternal;
        private static decimal λ = 0m, α = 0m, k = 0m;
        private static decimal mean, variance;
        private static double[] coefficients;
        /// <summary>
        /// These inputs need to be time specific inputs.
        /// </summary>
        /// <param name="pointsInternal"></param>
        public UnivariateRegressionFitting(IList<decimal> pointsInternal)
        {
            this.pointsInternal = pointsInternal;
            ExponentialDistributionFit(pointsInternal);
            //GammaDistributionFit(pointsInternal);
            //GetNormalFit(pointsInternal);
        }


        private static void ExponentialDistributionFit(IList<decimal> points)
        {
            var sum = 0m;
            int n = points.Count();
            foreach (var d in points)
            {
                sum += d;
            }
            λ = n / sum;
        }

        public double ExponentialPDF(double x)
        {
            var result = (double)λ * Math.Pow(Math.E, ((-1) * (double)λ * x));
            return result;
        }

        private static void GammaDistributionFit(IList<decimal> points)
        {
            var sum = 0m;
            var sumxLNx = 0m;
            var sumLNx = 0m;
            int n = points.Count();
            foreach (var d in points)
            {
                sum += d;
                sumxLNx += d * ((decimal)Math.Log((double)d));
                sumLNx += (decimal)Math.Log((double)d);
            }

            k = ((n * sum) / ((n * sumxLNx) - (sumLNx * sum)));
            α = (decimal)((1 / (Math.Pow((double)n, 2))) * (double)((n * sumxLNx) - (sumLNx * sum)));
        }

        //public double GammaPDF(double x)
        //{
        //    var gammaPortion = 1 / AdvancedMath.Gamma((double)α);
        //    var xPow = Math.Pow(x, (double)(α - 1));
        //    var gammaDist = gammaPortion * xPow * Math.Pow(Math.E, (-1) * x);
        //    return gammaDist;
        //}

        private void GetNormalFit(IList<decimal> points)
        {
            mean = BasicRegresssionCalcs.Mean(points);
            variance = BasicRegresssionCalcs.Variance(points);

        }

        public double NormalDistributionValueAt(double x)
        {
            var firstTerm = (1 / Math.Sqrt(2 * Math.PI * (double)variance));
            var secondTerm = Math.Pow(Math.E, ((-1) * Math.Pow((x - (double)mean), 2)) / (2 * (double)variance));
            var result = firstTerm * secondTerm;
            return result;
        }

    }
}
