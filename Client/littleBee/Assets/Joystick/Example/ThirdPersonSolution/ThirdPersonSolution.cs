// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.

namespace zFrame.Example
{
    using UnityEngine;
    using zFrame.UI;
    public class ThirdPersonSolution : MonoBehaviour
    {
        [SerializeField] Joystick joystick;
        public float speed = 5;
        CharacterController controller;
        void Start()
        {
            controller = GetComponent<CharacterController>();

            joystick.OnValueChanged.AddListener(v =>
            {
                if (v.magnitude != 0)
                {
                    Vector3 direction = new Vector3(v.x.AsFloat(), 0, v.y.AsFloat());
                    controller.Move(direction * speed * Time.deltaTime);
                    transform.rotation = Quaternion.LookRotation(new Vector3(v.x.AsFloat(), 0, v.y.AsFloat()));
                }
            });
        }
    }
}
