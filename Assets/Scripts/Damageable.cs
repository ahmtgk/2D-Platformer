using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Damageable : MonoBehaviour
{
    public UnityEvent<int, Vector2> damageableHit;
    public UnityEvent damageableDeath;
    public UnityEvent<int, int> healthChanged;

    Animator animator;

    [SerializeField] 
    private int _maxHealth = 100;

    public int MaxHealth
    {
        get { return _maxHealth; }
        set { _maxHealth = value; }
    }

    [SerializeField]
    private int _health = 100;
    public int Health
    {
        get
        {
            return _health;
        }
        set 
        { 
            _health = value; 
            healthChanged?.Invoke(_health, MaxHealth);
            
            if (_health <= 0)
            {
                IsAlive = false;
            }   
        }    
    }

    [SerializeField]
    private bool _isAlive = true;

    [SerializeField]
    private bool isInvicible = false;
    
    private float timeSinceHit = 0;
    public float invicibilityTime = 0.25f;

    public bool IsAlive
    {
        get { return _isAlive; }
        set
        {
            _isAlive = value;
            animator.SetBool(AnimationStrings.isAlive, value);
            Debug.Log("IsAlive set " + value);
            
            if(value == false)
            {
                damageableDeath.Invoke();
            }    
        }
    }
    public bool LockVelocity 
    { 
        get
        {   
            return animator.GetBool(AnimationStrings.lockVelocity);
        }
        set
        {
            animator.SetBool(AnimationStrings.lockVelocity, value);
        }
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isInvicible)
        {
            if(timeSinceHit > invicibilityTime)
            {
                isInvicible = false;
                timeSinceHit = 0f;
            }
            
            timeSinceHit += Time.deltaTime;
        }

    }

    public bool Hit(int damage, Vector2 knockback)
    {
        if (IsAlive && !isInvicible)
        {
            Health -= damage;
            isInvicible = true;

            animator.SetTrigger(AnimationStrings.hitTrigger);
            LockVelocity = true;
            damageableHit?.Invoke(damage, knockback);
            CharacterEvents.characterDamaged.Invoke(gameObject, damage);
            
            return true;

        }
        return false;
    } 

    public bool Heal(int healthRestore)
    {
        if (IsAlive && Health < MaxHealth)
        {
            int maxHeal = Mathf.Max(MaxHealth - Health, 0);
            int actualHeal = Mathf.Min(maxHeal, healthRestore);
            Health += actualHeal;
            CharacterEvents.characterHealed(gameObject, actualHeal);
            return true;
        }
        return false;
    }
}
