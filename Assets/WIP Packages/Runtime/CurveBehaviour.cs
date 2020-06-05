using UnityEngine;

namespace Paalo.WIP
{
	public class CurveBehaviour : MonoBehaviour
	{
		public float speed = 2f;

		//Default values for: minSpeedThreshold, volumeRangeLow, maxSpeedClamp, volumeRangeHigh
		public AnimationCurve animationCurve = AnimationCurve.EaseInOut(2f, -12f, 10f, 0f);

		private void Update()
		{
			if (Input.GetKeyDown(KeyCode.Alpha1))
			{
				//Get the different keys
				Keyframe firstKey = animationCurve.keys[0];
				Keyframe lastKey = animationCurve.keys[animationCurve.length - 1];

				// Min / Max speed
				float minSpeedThreshold = firstKey.time;
				float maxSpeedClamp = lastKey.time;

				// Volume Range
				float volumeRangeLow = firstKey.value;
				float volumeRangeHigh = lastKey.value;

				//Calculate volume reduction
				float volumeReductionDB = animationCurve.Evaluate(speed);
				Debug.Log($"VolumeReductionDB: {volumeReductionDB}");
			}

			if (Input.GetKeyDown(KeyCode.Alpha2))
			{
				Debug.Log($"Move key!");
				animationCurve.MoveKey(animationCurve.length - 1, new Keyframe(10f, -6f));
			}
		}
	}
}