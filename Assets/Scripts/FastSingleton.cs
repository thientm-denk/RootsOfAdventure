using UnityEngine;

/// <summary>
/// FastSingleton MonoBehaviour ---> For fasted reference purpose
/// Update:2020-04-21 by hungtx
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class FastSingleton<T> : MonoBehaviour where T : FastSingleton<T> {
	/// <summary>
	/// The public static reference to the instance
	/// Call if you sure the reference is available
	/// </summary>
	private static T _instance;
	private static bool _instantiated; //checking bool is faster than checking null
	/// <summary>
	/// This is safe reference
	/// Call to get reference in Awake Function
	/// </summary>
	public static T instance {
		get {
			if (_instantiated) {
				return _instance;
			}
			_instance = (T) FindObjectOfType(typeof(T));
			if (!_instance) {
				GameObject gameObject = new GameObject(typeof(T).ToString());
				return gameObject.AddComponent<T>();
			}
			if (_instance) {
				_instantiated = true;
			}
			return _instance;
		}
	}

	protected virtual void Awake() {
		// Make instance in Awake to make reference performance uniformly.
		if (!_instance) {
			_instance = (T) this;
			_instantiated = true;
		}
		// If there is an instance already in the same scene, destroy this script.
		else if (_instance != this) {
			Debug.LogWarning("Singleton " + typeof(T) + " is already exists.");
			Destroy(this);
		}
	}
	protected virtual void OnDestroy() {
		if (_instance == this) {
			_instance = null;
			_instantiated = false;
		}
	}
}