using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class VampireZone : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private int _stealingHealth;
    [SerializeField] private float _radius;
    [SerializeField] private Color _changingColor;
    [SerializeField] private float _waitTime;
    
    private Color _defaultColor;
    private Coroutine _coroutine;
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _defaultColor = _image.color;
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.RightControl))
        {
            _image.color = _changingColor;

            HandleColliders();
        }
        else if (Input.GetKeyUp(KeyCode.RightControl))
        {
            _image.color = _defaultColor;
            _coroutine = null;

            StopAllCoroutines();
        }
    }

    private IEnumerator StealHealth(Enemy enemy)
    {
        yield return new WaitForSeconds(_waitTime);

        enemy.TakeDamage(_stealingHealth);
        _player.Heal(_stealingHealth);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, _radius);
    }

    private void HandleColliders()
    {
        foreach (Collider2D collider in GetColliders())
        {
            if (collider.gameObject.TryGetComponent(out Enemy enemy))
            {
                if (_coroutine == null)
                {
                    _coroutine = StartCoroutine(StealHealth(enemy));
                }
            }
        }
    }

    private Collider2D[] GetColliders()
    {
        return Physics2D.OverlapCircleAll(transform.position, _radius);
    }
}