// Copyright (c) Bian Shanghai
// https://github.com/Bian-Sh/UniJoystick
// Licensed under the MIT license. See the LICENSE.md file in the project root for more information.
namespace zFrame.Example
{
    using UnityEngine;
    using UnityEngine.UI;
    using zFrame.UI;
    public class ConfigurationDemo : MonoBehaviour
    {
        public Joystick joystick;
        Text text;
        public Text text2;
        void Start()
        {
            text = GetComponent<Text>();
            joystick.OnValueChanged.AddListener(v => text.text = string.Format("Horizontal ：{0} \nVertical：{1}", v.x, v.y));
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                joystick.DynamicJoystick = !joystick.DynamicJoystick;
                text2.text = joystick.DynamicJoystick ? "切换为动态摇杆" : "切换成静态摇杆";
            }
        }
    }
}
