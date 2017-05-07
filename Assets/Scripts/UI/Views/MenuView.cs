using System;
using UnityEngine;

namespace TicTacToe3D
{
    public abstract class MenuView : MonoBehaviour
    {
        protected Animator Animator { get; private set; }
        protected event Action MenuOpened;

        public bool IsOpen
        {
            get
            {
                return Animator.GetBool("IsOpen");
            }
            set
            {
                Animator.SetBool("IsOpen", value);
                if (value && MenuOpened != null)
                {
                    MenuOpened();
                }
            }
        }

        protected virtual void Awake()
        {
            Animator = GetComponent<Animator>();
        }
    }
}