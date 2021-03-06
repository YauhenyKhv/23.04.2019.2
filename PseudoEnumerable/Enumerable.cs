﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

namespace PseudoEnumerable
{
    public static class Enumerable
    {
        #region Public methods
        /// <summary>
        /// Filters a sequence of values based on a predicate.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source</typeparam>
        /// <param name="source">An <see cref="IEnumerable{TSource}"/> to filter.</param>
        /// <param name="predicate">A function to test each source element for a condition</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> that contains elements from the input
        ///     sequence that satisfy the condition.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="predicate"/> is null.</exception>
        public static IEnumerable<TSource> Filter<TSource>(this IEnumerable<TSource> source,
            Func<TSource,bool> predicate)
        {
            CheckArguments(source, predicate);

            return FilterLogic(source, predicate);
        }

        /// <summary>
        /// Transforms each element of a sequence into a new form.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TResult">The type of the value returned by transformer.</typeparam>
        /// <param name="source">A sequence of values to invoke a transform function on.</param>
        /// <param name="transformer">A transform function to apply to each source element.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TResult}"/> whose elements are the result of
        ///     invoking the transform function on each element of source.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="transformer"/> is null.</exception>
        public static IEnumerable<TResult> Transform<TSource, TResult>(this IEnumerable<TSource> source,
            Func<TSource, TResult> transformer)
        {
            CheckArguments(source, transformer);

            return TransformLogic(source, transformer);
        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by key.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="key">A function to extract a key from an element.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="key"/> is null.</exception>
        public static IEnumerable<TSource> SortBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> key)
        {
            CheckArguments(source, key);

            IComparer<TKey> comparer;

            try
            {
                comparer = Comparer<TKey>.Default;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("It is necessary to pass a custom comparer if it is not implemented by the default type.");
            }

            return SortBy(source, key, comparer);

        }

        /// <summary>
        /// Sorts the elements of a sequence in ascending order according by using a specified comparer for a key .
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by key.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="key">A function to extract a key from an element.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="comparer"/> is null.</exception>
        public static IEnumerable<TSource> SortBy<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> key, IComparer<TKey> comparer)
        {
            CheckArguments(source, key);

            try
            {
                comparer = Comparer<TKey>.Default;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("It is necessary to pass a custom comparer if it is not implemented by the default type.");
            }

            List<TSource> array = new List<TSource>(source);

            int i = 0;
            TSource temp;

            while (i < array.Count)
            {

                for (int j = 0; j < array.Count - 1; j++)
                {
                    if (comparer.Compare(key.Invoke(array[j]), key.Invoke(array[j + 1])) > 0)
                    {
                        temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }

                i++;
            }

            return Iterator(array);
        }


        /// <summary>
        /// Sorts the elements of a sequence in descending order according to a key.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by key.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="key">A function to extract a key from an element.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="key"/> is null.</exception>
        public static IEnumerable<TSource> SortByDescending<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> key)
        {
            CheckArguments(source, key);

            IComparer<TKey> comparer;

            try
            {
                comparer = Comparer<TKey>.Default;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("It is necessary to pass a custom comparer if it is not implemented by the default type.");
            }

            return SortByDescending(source, key, comparer);

        }

        /// <summary>
        /// Sorts the elements of a sequence in descending order according by using a specified comparer for a key .
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <typeparam name="TKey">The type of the key returned by key.</typeparam>
        /// <param name="source">A sequence of values to order.</param>
        /// <param name="key">A function to extract a key from an element.</param>
        /// <param name="comparer">An <see cref="IComparer{T}"/> to compare keys.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{TSource}"/> whose elements are sorted according to a key.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="key"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="comparer"/> is null.</exception>
        public static IEnumerable<TSource> SortByDescending<TSource, TKey>(this IEnumerable<TSource> source,
            Func<TSource, TKey> key, IComparer<TKey> comparer)
        {
            CheckArguments(source, key);

            try
            {
                comparer = Comparer<TKey>.Default;
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("It is necessary to pass a custom comparer if it is not implemented by the default type.");
            }

            List<TSource> array = new List<TSource>(source);

            int i = 0;
            TSource temp;

            while (i < array.Count)
            {

                for (int j = 0; j < array.Count - 1; j++)
                {
                    if (comparer.Compare(key.Invoke(array[j]), key.Invoke(array[j + 1])) < 0)
                    {
                        temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }

                i++;
            }

            return Iterator(array);
        }

        /// <summary>
        /// Casts the elements of an IEnumerable to the specified type.
        /// </summary>
        /// <typeparam name="TResult">The type to cast the elements of source to.</typeparam>
        /// <param name="source">The <see cref="IEnumerable"/> that contains the elements to be cast to type TResult.</param>
        /// <returns>
        ///     An <see cref="IEnumerable{T}"/> that contains each element of the source sequence cast to the specified type.
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="InvalidCastException">An element in the sequence cannot be cast to type TResult.</exception>
        public static IEnumerable<TResult> CastTo<TResult>(this IEnumerable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException($"{nameof(source)} must not be null");
            }

            if (source is IEnumerable<TResult> resultSource)
            {
                return resultSource;
            }

            return Iterator();

            IEnumerable<TResult> Iterator()
            {
                foreach (var item in source)
                {
                    yield return (TResult)item;
                }
            }
        }

        /// <summary>
        /// Determines whether all elements of a sequence satisfy a condition.
        /// </summary>
        /// <typeparam name="TSource">The type of the elements of source.</typeparam>
        /// <param name="source">A sequence of values.</param>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <returns>
        ///     true if every element of the source sequence passes the test in the specified predicate,
        ///     or if the sequence is empty; otherwise, false
        /// </returns>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="source"/> is null.</exception>
        /// <exception cref="ArgumentNullException">Throws if <paramref name="predicate"/> is null.</exception>
        public static bool ForAll<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            CheckArguments(source, predicate);

            return ForAllLogic(source, predicate);
        }

        /// <summary>
        /// Сount sequence generator of integers.
        /// </summary>
        /// <param name="count">Count of elements.</param>
        /// <param name="start">Start with this number.</param>
        /// <returns>An <see cref="IEnumerable{T}"/> which contains integers.</returns>
        public static IEnumerable<int> Generator(int count, int start)
        {
            if (count <= 0)
            {
                throw new ArgumentException($"{nameof(count)} must not be less than zero");
            }

            return Iterator();

            IEnumerable<int> Iterator()
            {
                for(int i = 0; i<count; i++, start++)
                {
                    yield return start;
                }    
            }
        }

        #endregion

        #region Private methods

        private static void CheckArguments<TSource>(IEnumerable<TSource> source, dynamic predicate)
        {
            if (source == null)
            {
                throw new ArgumentNullException($"{nameof(source)} must not be  null");
            }

            if (predicate == null)
            {
                throw new ArgumentNullException($"{nameof(predicate)} must not be null");
            }
        }

        private static bool ForAllLogic<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            bool result = true;

            foreach(TSource item in source)
            {
                if (!predicate(item))
                {
                    result = false;
                }
            }

            return result;
        }

        private static IEnumerable<TSource> FilterLogic<TSource>(IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (TSource item in source)
            {
                if (predicate(item))
                {
                    yield return item;
                }
            }
        }

        private static IEnumerable<TResult> TransformLogic<TSource, TResult>(this IEnumerable<TSource> source,
           Func<TSource, TResult> transformer)
        {
            foreach (TSource item in source)
            {
                yield return transformer.Invoke(item);
            }
        }

        private static IEnumerable<TKey> Iterator<TKey>(IEnumerable<TKey> array)
        {
            foreach (var item in array)
            {   
                yield return item;
            }
        }

        #endregion

    }
}