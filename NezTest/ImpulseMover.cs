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
        VirtualAxis _xAxisInput;
        VirtualAxis _yAxisInput;
        ArcadeRigidbody _body;
        [Inspectable]
        float Angle;

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
            _xAxisInput.deregister();
            _yAxisInput.deregister();
        }

        void SetupInput()
        {
            _thrustInput = new VirtualButton();
            _thrustInput.nodes.Add(new VirtualButton.KeyboardKey(Keys.Z));
            _thrustInput.nodes.Add(new VirtualButton.GamePadButton(0, Buttons.A));

            _xAxisInput = new VirtualAxis();
            _xAxisInput.nodes.Add(new VirtualAxis.GamePadLeftStickX());
            _xAxisInput.nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Left, Keys.Right));
            _yAxisInput = new VirtualAxis();
            _yAxisInput.nodes.Add(new VirtualAxis.GamePadLeftStickY());
            _yAxisInput.nodes.Add(new VirtualAxis.KeyboardKeys(VirtualInput.OverlapBehavior.TakeNewer, Keys.Up, Keys.Down));
        }

        public void update()
        {
            Angle = (float)Math.Atan2(_yAxisInput, _xAxisInput);
            entity.transform.rotation = Angle;
            _body.addImpulse(new Vector2(_xAxisInput.value * 5, _yAxisInput.value * 5));
            _body.velocity = Vector2.Clamp(_body.velocity, new Vector2(-100, -100), new Vector2(100,100));
        }
    }
}
