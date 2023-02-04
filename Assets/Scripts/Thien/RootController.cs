using UnityEngine;

namespace Thien
{
    public class RootController : MonoBehaviour
    {
        [Header("References Object")]
        [SerializeField] private CameraController cameraController;
        
        [Header("Setting variable")]
        [SerializeField] private float capStreng = 0.8f;
        [SerializeField] private float capWater = 1.4f;
     
        private float timer;
        
        public float health = 10f;
        public float maxHealth = 10f;
        public LineRenderer root;
        public Camera mainCamera;
        public Transform rootTarget;
        public float rootTipLenght;
        public float timeUntilNewRootPoint;
        public float currentTimeUntilNewRootPoint;
        public int rootPointIndex = 1;
        
        public bool dead = true;
        public bool growing = false;

        public Color rootBase, rootTip, rootWater, rootEnd, rootHurt, rootMagic;

        public float poisonned = 0f;

        public float depth = 0;
        Vector3[] startPositions;

        private void Start()
        {
            currentTimeUntilNewRootPoint = timeUntilNewRootPoint;
            health = maxHealth;
            startPositions = new Vector3[] {root.GetPosition(0), root.GetPosition(1), root.GetPosition(2)};
        }

        public void StartGame()
        {
            dead = false;
            growing = false;
            currentTimeUntilNewRootPoint = timeUntilNewRootPoint + 1f;
            health = maxHealth;
            root.positionCount = 3;
            rootPointIndex = 2;
            depth = 0;
            root.SetPositions(startPositions);

        }
        

        private void Update()
        {
            if (dead)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartGame();
                }
                return;
            }

            UpdateRootTarget();
            UpdateRoot();
            UpdateRootWidth();
            
            if (dead || !growing)
            {
                return;
            }

            UpdateHealth();
        }

        private void UpdateHealth()
        {
            timer += Time.deltaTime;
            if (poisonned > 0f)
            {
                health -= Time.deltaTime * capStreng;
                poisonned -= Time.deltaTime;
            }

            health -= Time.deltaTime * capWater;
            if (health <= 0 && !dead)
            {
                dead = true;
                cameraController.Death();
            }
        }

        void UpdateRootTarget()
        {
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

            worldPosition = new Vector3(worldPosition.x, worldPosition.y, 0);

            rootTarget.position = worldPosition;
        }


        void UpdateRoot()
        {
            UpdateRootTip();

            Vector3 lastPosition = root.GetPosition(rootPointIndex - 1);
            Vector3 direction = rootTarget.position - lastPosition;
            direction = direction.normalized * rootTipLenght;

            Move();
        }

        void UpdateRootTip()
        {
            Vector3 lastPosition = root.GetPosition(rootPointIndex - 1);
            Vector3 direction = rootTarget.position - lastPosition;
            direction = direction.normalized * rootTipLenght;
            root.SetPosition(rootPointIndex, lastPosition + direction);
        }

        void UpdateRootWidth()
        {
            float rootIndex = (float) rootPointIndex;
            AnimationCurve curve = new AnimationCurve();
            curve.AddKey(0, 1);
            curve.AddKey((rootIndex - 7f) / rootIndex, 0.9f);
            curve.AddKey((rootIndex - 6f) / rootIndex, 0.9f);
            curve.AddKey((rootIndex) / rootIndex, 0.2f);
            curve.AddKey(1, 0);
            curve.SmoothTangents(1, 1f);
            curve.SmoothTangents(2, 1f);
            root.widthCurve = curve;
            root.widthMultiplier = Mathf.Min(0.2f, root.positionCount * 0.0002f + 0.1f);


            float index = (float) rootPointIndex;
            float alpha = 1.0f;
            Gradient gradient = new Gradient();
            gradient.SetKeys(
                new GradientColorKey[]
                {
                    new GradientColorKey(rootBase, 0.0f),
                    new GradientColorKey(rootBase, (index - health * 2f) / index - 1f / index),
                    new GradientColorKey(poisonned <= 0 ? rootWater : rootHurt, (index - health * 2f) / index),
                    new GradientColorKey(poisonned <= 0 ? rootWater : rootHurt,
                        (index - health * 2f) / index + 1f / index),
                    new GradientColorKey(poisonned <= 0 ? rootWater : rootHurt, 1.0f)
                },
                new GradientAlphaKey[] {new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f)}
            );
            root.colorGradient = gradient;
        }

        void Move()
        {
            growing = true;
            Vector3 lastPosition = root.GetPosition(rootPointIndex - 1);
            if (lastPosition.y < depth)
            {
                depth = lastPosition.y;
            }

            Vector3 direction = rootTarget.position - lastPosition;
            if (direction.normalized.y < -0.7f)
            {
                currentTimeUntilNewRootPoint -= Time.fixedDeltaTime;
            }
            else
            {
                currentTimeUntilNewRootPoint -=
                    Time.fixedDeltaTime * (Mathf.Max(0, (-direction.normalized.y + 0.7f)));
            }

            if (currentTimeUntilNewRootPoint <= 0 &&
                Vector3.Distance(rootTarget.position, lastPosition) > rootTipLenght * 5f)
            {
                root.positionCount = root.positionCount + 1;
                rootPointIndex++;
                currentTimeUntilNewRootPoint = timeUntilNewRootPoint * 1.1f;
                UpdateRootTip();

            }
        }
    }
}