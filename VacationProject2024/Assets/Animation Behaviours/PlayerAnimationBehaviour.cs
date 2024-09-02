using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationBehaviour : StateMachineBehaviour
{
    Player m_player;
    protected Player player { get { if (m_player == null) m_player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>(); return m_player; } }
}
