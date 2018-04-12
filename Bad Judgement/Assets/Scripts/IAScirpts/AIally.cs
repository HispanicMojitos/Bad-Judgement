using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIally : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private Transform player;
    [SerializeField] private Target allyHealthState;
    private float MaxDistance = 10;
    private float vieActuelle;
    
    public bool allyEstRéanimé = false;

	void Start ()
    {
        vieActuelle = allyHealthState.vie;
	}

	void Update ()
    {
        if (allyHealthState.vie != 0)
        {
            if (Vector3.Distance(this.transform.position, player.position) > 5)
            {
                Vector3 direction = player.position - this.transform.position;
                direction.y = 0;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), 0.1f);
                this.transform.Translate(0, 0, 0.05f);
                SetAnimation(isWalking: true);
            }
            else SetAnimation(isIdle: true);
        }
        else if (allyHealthState.vie == 0 )
        {
            SetAnimation(isDead: true);
        }
    }

    private void SetAnimation(bool isAimKneel = false, bool isKneel = false, bool kneeGrenad = false, bool isIdle = false, bool isAiming = false, bool isAttack = false, bool isWalking = false, bool isRunning = false, bool isAttackingCloser = false, bool isDead = false) // Utilisation de prametre nomé, a récuperer en argument nommer pour décider quelle animation sera jouée
    {
        anim.SetBool("IsAimKneel", isAimKneel);
        anim.SetBool("IsKneel", isKneel);
        anim.SetBool("IsGrenade", kneeGrenad);
        anim.SetBool("IsIdle", isIdle);
        anim.SetBool("IsAiming", isAiming);
        anim.SetBool("IsAttacking", isAttack);
        anim.SetBool("IsWalking", isWalking);
        anim.SetBool("IsRunning", isRunning);
        anim.SetBool("IsAttackingCloser", isAttackingCloser);
        anim.SetBool("IsDead", isDead);
    }
}
