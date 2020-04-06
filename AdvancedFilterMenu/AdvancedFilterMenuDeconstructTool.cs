﻿using Harmony;
using ModFramework;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace AdvancedFilterMenu {
    public sealed class AdvancedFilterMenuDeconstructTool : MultiFilteredDragTool {
        public static AdvancedFilterMenuDeconstructTool Instance { get; private set; }

        public AdvancedFilterMenuDeconstructTool() {
            Instance = this;
        }

        public static void DestroyInstance() {
            Instance = null;
        }

        protected override void OnPrefabInit() {
            base.OnPrefabInit();

            visualizer = Util.KInstantiate(Traverse.Create(DeconstructTool.Instance).Field("visualizer").GetValue<GameObject>());

            visualizer.SetActive(false);
            visualizer.transform.SetParent(transform);

            FieldInfo areaVisualizerField = AccessTools.Field(typeof(DragTool), "areaVisualizer");
            FieldInfo areaVisualizerSpriteRendererField = AccessTools.Field(typeof(DragTool), "areaVisualizerSpriteRenderer");

            GameObject areaVisualizer = Util.KInstantiate(Traverse.Create(DeconstructTool.Instance).Field("areaVisualizer").GetValue<GameObject>());
            areaVisualizer.SetActive(false);

            areaVisualizer.name = "AdvancedFilterMenuDecontructToolAreaVisualizer";
            areaVisualizerSpriteRendererField.SetValue(this, areaVisualizer.GetComponent<SpriteRenderer>());
            areaVisualizer.transform.SetParent(transform);

            areaVisualizerField.SetValue(this, areaVisualizer);
            gameObject.AddComponent<CancelToolHoverTextCard>();
        }

        protected override void OnDragTool(int cell, int distFromOrigin) {
            for (int index = 0; index < 40; ++index) {
                GameObject gameObject = Grid.Objects[cell, index];

                if (gameObject != null && MultiToolParameterMenu.Instance.IsActiveLayer(MultiToolParameterMenu.GetFilterLayerFromGameObject(gameObject))) {
                    gameObject.Trigger(-790448070, null);
                    Prioritizable prioritizable = gameObject.GetComponent<Prioritizable>();

                    if (prioritizable != null) {
                        prioritizable.SetMasterPriority(ToolMenu.Instance.PriorityScreen.GetLastSelectedPriority());
                    }
                }
            }
        }

        protected override void OnActivateTool() {
            base.OnActivateTool();

            ToolMenu.Instance.PriorityScreen.Show(true);
        }

        protected override void OnDeactivateTool(InterfaceTool newTool) {
            base.OnDeactivateTool(newTool);

            ToolMenu.Instance.PriorityScreen.Show(false);
        }
    }
}