using UnityEngine;

namespace TicTacToe3D
{
    public abstract class MenuView : MonoBehaviour
    {
        protected Animator Animator { get; private set; }

        public bool IsOpen
        {
            get { return Animator.GetBool("IsOpen"); }
            set { Animator.SetBool("IsOpen", value); }
        }

        protected void Awake()
        {
            Animator = GetComponent<Animator>();
        }
    }
}