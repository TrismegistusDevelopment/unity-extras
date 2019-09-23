using UnityEngine;
using UnityEngine.Events;

namespace Trismegistus.Core.Types {
	public class UnityStringEvent : UnityEvent<string> { }

	public class UnityFloatEvent : UnityEvent<float> { }

	public class UnityIntEvent : UnityEvent<int> { }

	public class UnityVec2Event : UnityEvent<Vector2> { }

	public class UnityVec3Event : UnityEvent<Vector3> { }

	public class UnityQuaternionEvent : UnityEvent<Quaternion> { }

	public class UnityTransformEvent : UnityEvent<Transform> { }

	public class UnityGameObjectEvent : UnityEvent<GameObject> { }
}