using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JunkCollector : MonoBehaviour
{
    [SerializeField] GameObject comboMultiplier;

    GameObject currComboCanvas = null;

    public int Score { get; private set; }
    private float _timer = 0.0f;
    [SerializeField] float MaxStreekTime = 0.5f;
    private int _scoreIncrease = 1;
    private ParticleSystem collectParticles = null;
    public int playerCollectorIndex = 0;
    private GameManagerScript gameManagerScript = null;

    private AudioSource coinSound = null;

    // Start is called before the first frame update
    void Start()
    {
        if (collectParticles == null)
            collectParticles = GetComponentInChildren<ParticleSystem>();

        if (gameManagerScript == null)
            gameManagerScript = FindObjectOfType<GameManagerScript>();

        if(coinSound == null)
            coinSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("FreeJunk"))
        {
            if (_timer > MaxStreekTime)
            {
                _scoreIncrease = 1;
            }
            else
            {
                _scoreIncrease *= 2;
            }
            Score += _scoreIncrease;
            other.transform.position = transform.position;
            other.gameObject.layer = LayerMask.NameToLayer("CollectedJunk");
            _timer = 0.0f;
            gameManagerScript.DecreaseJunkCount();

            // Spawn ComboText
            int combo = (int)Mathf.Log((float)_scoreIncrease, 2.0f) + 1;
            if (combo > 1)
            {
                if (currComboCanvas)
                {
                    Destroy(currComboCanvas);
                    currComboCanvas = null;
                }

                currComboCanvas = Instantiate(comboMultiplier);
                currComboCanvas.transform.position = transform.position + new Vector3(
                    0, 1, 0
                    );
                ComboCanvas canvas = currComboCanvas.GetComponent<ComboCanvas>();
                if (canvas)
                    canvas.SetCombo(combo);
            }

            //PARTICLES TEST
            if (collectParticles != null)
                collectParticles.Play();
            if(coinSound != null)
                coinSound.Play();
        }
    }

    public void SetPlayerIndex(int index)
    {
        playerCollectorIndex = index;
    }
}
