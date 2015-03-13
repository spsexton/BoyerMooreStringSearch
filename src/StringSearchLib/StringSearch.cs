using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace StringSearchLib
{
    public static class StringSearch
    {
        /// <summary>
        /// Search a string buffer for a specified search string.  Replicates 
        /// behavior of String.IndexOf, but in simple "brute force" approach.
        /// Goal is to compare performance--i.e. demonstrate that standard
        /// String.IndexOf function is faster than our brute force approach.
        /// <param name="sBuffer">Input buffer to search</param>
        /// <param name="sSearchString">String to search for</param>
        /// <returns>0-based index of 1st occurrence, or -1 if not found</returns>
        /// </summary>
        public static int IndexOfBruteForce(this string sBuffer, string sSearchString)
        {
            int nIndex = -1;     // Return value

            int nBufferInd = 0;
            while (nBufferInd <= sBuffer.Length - sSearchString.Length)
            {
                int nSearchInd = 0;

                while (nSearchInd < sSearchString.Length &&
                       (sBuffer[nBufferInd+nSearchInd] == sSearchString[nSearchInd]))
                    nSearchInd++;

                if (nSearchInd == sSearchString.Length)
                {
                    // Found it
                    nIndex = nBufferInd;
                    break;      // Stop searching
                }

                nBufferInd++;
            }

            return nIndex;
        }

        /// <summary>
        /// Search for substring using Boyer-Moore, variation 1.  Just use single
        /// jump table--the "last occurrence of character" table.
        /// </summary>
        /// <param name="sBuffer"></param>
        /// <param name="sPattern"></param>
        /// <returns></returns>
        public static int IndexOfBM1(this string sBuffer, string sPattern)
        {
            int nIndex = -1;     // Return value

            // Trivial reject: pattern longer than buffer
            if (sPattern.Length > sBuffer.Length)
                return nIndex;

            //=== Calculate values for first jump table -- how far from end of pattern is LAST ===
            // occurrence of each character?  (Default is entire length of pattern, if the character
            // does NOT appear in the pattern).
            int[] T1 = new int[256];
            for (int i = 0; i < 256; i++)
                T1[i] = sPattern.Length;

            // Note that we don't normally have an entry in the table for the
            //   last character of the pattern, unless it appears in the pattern
            //   more than once.  So we start at the 2nd-to-last character and calculate,
            //   for each, the distance to the last character.  If there are duplicates
            //   for a given character, we only measure this distance for the last
            //   occurrence of a character in the pattern.
            // We move from right to left in the pattern and record distance info if 
            //   this is the FIRST time we've encountered this character (not already
            //   in table.  This gives us info for rightmost instance of each char.
            int nIndexLastChar = sPattern.Length - 1;
            for (int i = nIndexLastChar - 1; i >= 0; i--)
                if (T1[sPattern[i]] == sPattern.Length)
                    T1[sPattern[i]] = nIndexLastChar - i;

            //=== Now the actual search algorithm, using the jump table
            int nIndBuff;       // Index in buffer where next compare will occur
            int nIndPatt;       // Index in pattern where next compare will occur

            // First search is when start of pattern aligns w/start of buffer
            //   (start compare at END of pattern, rather than beginning)
            nIndBuff = sPattern.Length - 1;
            
            // Search for occurrence of pattern at next location in buffer
            while (nIndBuff < sBuffer.Length)
            {
                // Each search iteration, we start comparing at end of pattern
                nIndPatt = sPattern.Length - 1;

                // Match as much of the pattern as possible, right to left
                while ((nIndPatt >= 0) &&
                       (sBuffer[nIndBuff] == sPattern[nIndPatt]))
                {
                    nIndBuff--;
                    nIndPatt--;
                }

                // Did we match entire thing?
                if (nIndPatt < 0)
                {
                    nIndex = nIndBuff + 1;
                    break;
                }

                // If we didn't match, calculate where to next look in buffer.  Start
                //   by calculating how far forward to shift our pattern.
                // Shift = T1(c) - # chars that matched
                //   where T1(c) = value in Table 1 for 1st char that did not match
                int nNumMatched = sPattern.Length - 1 - nIndPatt;
                int nShiftPattern = T1[sBuffer[nIndBuff]] - nNumMatched;

                // If last occurrence of mismatched character was in the substring 
                // already matched, shift would be negative.  So just shift 1.
                if (nShiftPattern < 1)
                    nShiftPattern = 1;

                // The "shift" of the pattern is just conceptual.  Really, what we want to know is--
                //   how far to shift our attention, after "shifting" the pattern.
                // Attention shift = pattern shift + # chars that matched
                //   (because we want to go back to end of pattern)
                int nShiftAttention = nShiftPattern + nNumMatched;

                // NOTE: This example includes calculation of pattern-shift and attention-shift
                //   values.  In reality, the nNumMatch terms cancel out, so you could simplify
                //   and just use T1(c) directly.  (as well as checking for the negative value case)

                // Finally, go ahead and "shift our attention" in buffer by moving the index.
                nIndBuff += nShiftAttention;
            }

            return nIndex;
        }

        public static int IndexOfBM2(this string sBuffer, string sPattern)
        {
            //=== Calculate values for the 2nd jump table: 
            // Positions of "rightmost plausible reoccurence of terminal substring"
            //int[] arSubstrings = new int[sPattern.Length];
            //arSubstrings[0] = 1;        // Mismatch at 1st char = no match, shift 1
            //for (int i = 1; i < sPattern.Length; i++)
            //{
            //    // i = length of matched terminal substring
            //    //   (i.e. final i chars of the pattern were matched)
            //}

            return -1;
        }
    }
}
