using System;
using Nez;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace NezTest
{
    class ImpulseMover : Component, IUpdatable
    {
        public float speed = 100f;
        VirtualButton _thrustInput;
        
        ArcadeRigidbody _body;
        [Inspectable]
        float Angle;
        [Inspectable]
        VirtualJoystick _leftStick;

        public override void onAddedToEntity()
        {
            base.onAddedToEntity();
            _body = entity.getComponent<ArcadeRigidbody>();
            SetupInput();
        }

        public override void onRemovedFromEntity()
        {
            base.onRemovedFromEntity();
            _thrustInput.deregister();
            _leftStick.deregister();
        }

        void SetupInput()
        {
            _thrustInput = new VirtualButton();
            _thrustInput.nodes.Add(new VirtualButton.KeyboardKey(Keys.Z));
            _thrustInput.nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

            _leftStick = new VirtualJoystick(true);
            _leftStick.nodes.Add(new VirtualJoystick.PadLeftStick());
            _leftStick.nodes.Add(new VirtualJoystick.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right, Keys.Up, Keys.Down));
        }

        public void update()
        {
            if (_leftStick.value.Length() > 0.1)
            {
                Angle = (float)Math.Atan2(_leftStick.value.Y, _leftStick.value.X);
                entity.transform.rotation = Angle;
                //_body.addImpulse(new Vector2(_leftStick.value.X * 5, _leftStick.value.Y * 5));
                _body.setVelocity(_leftStick.value * 300);
            }
        }
    }
}
