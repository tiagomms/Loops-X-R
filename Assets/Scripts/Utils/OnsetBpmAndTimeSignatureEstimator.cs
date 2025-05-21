using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Estimates the BPM and an approximate time signature (beats per bar) from raw audio samples.
/// Designed for XR prototyping where simplicity and speed are prioritized over perfect accuracy.
/// </summary>
public class OnsetBpmAndTimeSignatureEstimator
{
    private readonly float sampleRate;
    private readonly int windowSize;
    private readonly float silenceThreshold;
    private readonly float energyIncreaseRatio;

    /// <summary>
    /// Constructor to configure onset detection parameters.
    /// </summary>
    /// <param name="sampleRate">Audio clip sample rate (e.g., 44100 Hz)</param>
    /// <param name="windowSize">Window size for energy analysis (e.g., 1024 samples)</param>
    /// <param name="silenceThreshold">Minimum RMS energy to consider a region "audible"</param>
    /// <param name="energyIncreaseRatio">Threshold ratio to detect an onset</param>
    public OnsetBpmAndTimeSignatureEstimator(
        float sampleRate = 44100f,
        int windowSize = 1024,
        float silenceThreshold = 0.01f,
        float energyIncreaseRatio = 1.3f)
    {
        this.sampleRate = sampleRate;
        this.windowSize = windowSize;
        this.silenceThreshold = silenceThreshold;
        this.energyIncreaseRatio = energyIncreaseRatio;
    }

    /// <summary>
    /// Estimates BPM and an approximate time signature (beats per bar).
    /// </summary>
    /// <param name="samples">Raw mono audio samples</param>
    /// <returns>Tuple of (estimated BPM, estimated beats per bar)</returns>
    public (float bpm, int beatsPerBar) Estimate(float[] samples)
    {
        if (samples == null || samples.Length < windowSize * 2)
            return (0f, 0);

        var onsets = DetectOnsets(samples);

        if (onsets.Count < 2)
            return (0f, 0);

        // Compute time intervals (seconds) between onsets
        List<float> intervals = new();
        for (int i = 1; i < onsets.Count; i++)
        {
            float secondsBetween = (onsets[i] - onsets[i - 1]) / sampleRate;
            intervals.Add(secondsBetween);
        }

        // Get median interval to reject outliers (e.g., doubled/halved tempos)
        float avgInterval = Median(intervals);
        float bpm = 60f / avgInterval;

        // Time signature estimation: group onset intervals into clusters
        int beatsPerBar = EstimateBeatsPerBar(intervals, avgInterval);

        return (Mathf.Round(bpm), beatsPerBar);
    }

    /// <summary>
    /// Returns sample indices where onsets (energy spikes) occur.
    /// </summary>
    private List<int> DetectOnsets(float[] samples)
    {
        List<int> onsets = new();
        float previousEnergy = 0f;

        for (int i = 0; i <= samples.Length - windowSize; i += windowSize)
        {
            // Simple RMS energy calculation
            float energy = 0f;
            for (int j = 0; j < windowSize; j++)
            {
                float s = samples[i + j];
                energy += s * s;
            }
            energy /= windowSize;

            // Detect energy jump as onset
            if (energy > silenceThreshold && energy > previousEnergy * energyIncreaseRatio)
            {
                onsets.Add(i);
            }

            previousEnergy = energy;
        }

        return onsets;
    }

    /// <summary>
    /// Calculates median value from a list.
    /// </summary>
    private float Median(List<float> values)
    {
        if (values == null || values.Count == 0)
            return 0f;

        values.Sort();
        int mid = values.Count / 2;

        return values.Count % 2 == 0
            ? (values[mid - 1] + values[mid]) / 2f
            : values[mid];
    }

    /// <summary>
    /// Attempts to estimate how many beats occur in a repeating bar structure.
    /// </summary>
    private int EstimateBeatsPerBar(List<float> intervals, float referenceBeatDuration)
    {
        if (intervals == null || intervals.Count < 2)
            return 4; // default fallback

        float tolerance = referenceBeatDuration * 0.25f;

        List<int> beatGroups = new();
        int currentGroup = 1;

        for (int i = 1; i < intervals.Count; i++)
        {
            bool similar = Mathf.Abs(intervals[i] - referenceBeatDuration) < tolerance;

            if (similar)
            {
                currentGroup++;
            }
            else
            {
                beatGroups.Add(currentGroup);
                currentGroup = 1;
            }
        }

        if (currentGroup > 1)
            beatGroups.Add(currentGroup);

        return beatGroups.Count > 0 ? Mode(beatGroups) : 4;
    }

    /// <summary>
    /// Returns the mode (most frequent value) in a list of integers.
    /// </summary>
    private int Mode(List<int> values)
    {
        if (values == null || values.Count == 0)
            return 4;

        Dictionary<int, int> counts = new();
        foreach (int v in values)
        {
            if (!counts.ContainsKey(v))
                counts[v] = 0;
            counts[v]++;
        }

        return counts.Aggregate((a, b) => a.Value > b.Value ? a : b).Key;
    }
}
