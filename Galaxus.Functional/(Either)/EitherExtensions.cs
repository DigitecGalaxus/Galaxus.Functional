﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Galaxus.Functional
{
    /// <summary>
    /// Extensions for Either
    /// </summary>
    public static class EitherExtensions
    {
        /// <summary>
        /// Select all "A" fields
        /// </summary>
        public static IEnumerable<A> SelectA<A, B>(this IEnumerable<Either<A, B>> eithers)
        {
            return eithers
                .Where(e => e.IsA)
                .Select(
                    e => e.Match(
                        a => a,
                        _ => throw new InvalidOperationException("Only A is possible")));
        }

        /// <summary>
        /// Select all "B" fields
        /// </summary>
        public static IEnumerable<B> SelectB<A, B>(this IEnumerable<Either<A, B>> eithers)
        {
            return eithers
                .Where(e => e.IsB)
                .Select(
                    e => e.Match(
                        _ => throw new InvalidOperationException("Only B is possible"),
                        b => b));
        }

        /// <summary>
        /// Select all "A" fields
        /// </summary>
        public static IEnumerable<A> SelectA<A, B, C>(this IEnumerable<Either<A, B, C>> eithers)
        {
            return eithers
                .Where(e => e.IsA)
                .Select(
                    e => e.Match(
                        a => a,
                        _ => throw new InvalidOperationException("Only A is possible"),
                        _ => throw new InvalidOperationException("Only A is possible")));
        }

        /// <summary>
        /// Select all "B" fields
        /// </summary>
        public static IEnumerable<B> SelectB<A, B, C>(this IEnumerable<Either<A, B, C>> eithers)
        {
            return eithers
                .Where(e => e.IsB)
                .Select(
                    e => e.Match(
                        _ => throw new InvalidOperationException("Only B is possible"),
                        b => b,
                        _ => throw new InvalidOperationException("Only B is possible")));
        }

        /// <summary>
        /// Select all "C" fields
        /// </summary>
        public static IEnumerable<C> SelectC<A, B, C>(this IEnumerable<Either<A, B, C>> eithers)
        {
            return eithers
                .Where(e => e.IsC)
                .Select(
                    e => e.Match(
                        _ => throw new InvalidOperationException("Only C is possible"),
                        _ => throw new InvalidOperationException("Only C is possible"),
                        c => c));
        }
    }
}