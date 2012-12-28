﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HimaLib.Debug;
using HimaLib.Render;
using HimaLib.Math;
using HimaLib.Model;
using ccm.Input;
using ccm.Camera;

namespace ccm.Scene
{
    public class ModelViewerScene : SceneBase
    {
        BasicCamera camera = new BasicCamera();

        ModelViewerCameraUpdater cameraUpdater;

        IModel model;

        SimpleModelRenderParameter renderParam = new SimpleModelRenderParameter();
        
        public ModelViewerScene()
        {
            UpdateState = UpdateStateInit;
            DrawState = DrawStateInit;

            Name = "ModelViewer";

            cameraUpdater = new ModelViewerCameraUpdater(camera, InputAccessor.GetController(ControllerLabel.Main));
        }

        void UpdateStateInit()
        {
            InitCamera();

            InitRenderer();

            InitModel();

            UpdateState = UpdateStateMain;
            DrawState = DrawStateMain;
        }

        void InitCamera()
        {
            camera.Eye.Z = 30.0f;
            camera.Far = 300.0f;
        }

        void InitRenderer()
        {
            renderParam.Camera = camera;
            renderParam.Alpha = 1.0f;
            renderParam.AmbientLightColor = Vector3.One * 0.4f;
            renderParam.DirLight0Direction = Vector3.One * -1.0f;
            renderParam.DirLight0DiffuseColor = new Vector3(0.5f, 0.6f, 0.8f);
        }

        void InitModel()
        {
            model = ModelFactory.Instance.Create("cube003");
        }

        void DrawStateInit()
        {
        }

        void UpdateStateMain()
        {
            DebugFont.Add(Name, 50.0f, 60.0f);

            if (InputAccessor.IsPush(ControllerLabel.Main, BooleanDeviceLabel.Exit))
            {
                ChangeScene(new BootScene());
                return;
            }

            cameraUpdater.Update(Vector3.Zero);
        }

        void DrawStateMain()
        {
            model.Render(renderParam);
        }
    }
}
