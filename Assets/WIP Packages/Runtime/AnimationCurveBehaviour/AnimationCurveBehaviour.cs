using UnityEngine;

namespace Paalo.WIP
{
	public class AnimationCurveBehaviour : MonoBehaviour
	{
		public float velocity = 2f;

		//Default values for: minVelocityThreshold, volumeRangeLow, maxVelocityClamp, volumeRangeHigh
		public AnimationCurve velocityToDecibelAnimCurve = AnimationCurve.EaseInOut(2f, -12f, 10f, 0f);

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				// Get the different keys
				Keyframe firstKey = velocityToDecibelAnimCurve.keys[0];
				Keyframe lastKey = velocityToDecibelAnimCurve.keys[velocityToDecibelAnimCurve.length - 1];

				// Min / Max speed
				float minVelocityThreshold = firstKey.time;
				float maxVelocityClamp = lastKey.time;

				// Volume Range
				float volumeRangeLow = firstKey.value;
				float volumeRangeHigh = lastKey.value;

				// Calculate volume reduction
				float volumeReductionDB = velocityToDecibelAnimCurve.Evaluate(velocity);
				Debug.Log($"Velocity:\t{velocity}\n" +
						$"VolumeReductionDB:\t{volumeReductionDB} dB");
			}

			// How to change the value of a key in runtime
			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				velocityToDecibelAnimCurve.MoveKey(velocityToDecibelAnimCurve.length - 1, new Keyframe(10f, -6f));
				Debug.Log($"Set 'VolumeRangeHigh' to -6 dB");
			}
		}
	}
}