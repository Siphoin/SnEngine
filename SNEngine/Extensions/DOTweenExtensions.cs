﻿using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using UnityEngine;
using SNEngine.Animations;
using Cysharp.Threading.Tasks;
using System;
using SNEngine.Debugging;
using SNEngine.Repositories;

namespace SNEngine.Extensions
{
    public static class DOTweenExtensions
    {
        private const string PROPERTY_SHADER_AMOUNT_VALUE = "_Amount";

        private const float MAX_VALUE_CELIA_SHADER = 0.2f;

        public static TweenerCore<Vector3, Vector3, VectorOptions> DOParalax(this Transform target, Direction direction, float duration, bool snapping = false)
        {
            Vector3 endValue = target.position;

            target.position = target.GetScreenEdge(direction);



            TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = DOTween.To(() => target.position, delegate (Vector3 x)
            {
                target.position = x;
            }, endValue, duration);
            tweenerCore.SetOptions(snapping).SetTarget(target);
            return tweenerCore;
        }

        private static TweenerCore<float, float, FloatOptions> DOFloatMaterial (SpriteRenderer spriteRenderer, string nameMaterial, float value, float duration)
        {
            Material material = NovelGame.GetRepository<MaterialRepository>().GetMaterial(nameMaterial);

            if (!spriteRenderer.material.name.Contains(material.name))
            {
                spriteRenderer.material = new Material(material);
            }

            return spriteRenderer.material.DOFloat(value, PROPERTY_SHADER_AMOUNT_VALUE, duration);
        }

        public static TweenerCore<float, float, FloatOptions> DODissolve(this SpriteRenderer spriteRenderer, AnimationBehaviourType animationBehaviour, float duration, Texture2D texture = null)
        {
            float endValue = AnimationBehaviourHelper.GetValue(animationBehaviour);

            var operation = DOFloatMaterial(spriteRenderer, "dissolve", endValue, duration);

            if (texture != null)
            {
                spriteRenderer.material.SetTexture("_DissolveTexture", texture);
            }

            if (animationBehaviour == AnimationBehaviourType.Out)
            {
               ReturnMaterialToSpriteRenderer(spriteRenderer, duration).Forget();
            }
            return operation;
        }

        public static TweenerCore<float, float, FloatOptions> DOBlackAndWhite(this SpriteRenderer spriteRenderer, AnimationBehaviourType animationBehaviour, float duration)
        {
            float endValue = AnimationBehaviourHelper.GetValue(animationBehaviour);

            var operation = DOFloatMaterial(spriteRenderer, "blackAndWhite", endValue, duration);

            if (animationBehaviour == AnimationBehaviourType.Out)
            {
                ReturnMaterialToSpriteRenderer(spriteRenderer, duration).Forget();
            }

            return operation;
        }

        public static TweenerCore<float, float, FloatOptions> DOIllumination(this SpriteRenderer spriteRenderer, AnimationBehaviourType animationBehaviour, float duration)
        {
            float endValue = AnimationBehaviourHelper.GetValue(animationBehaviour);

            var operation = DOFloatMaterial(spriteRenderer, "illumination", endValue, duration);

            if (animationBehaviour == AnimationBehaviourType.Out)
            {
                ReturnMaterialToSpriteRenderer(spriteRenderer, duration).Forget();
            }

            return operation;
        }

        public static TweenerCore<float, float, FloatOptions> DOIllumination(this SpriteRenderer spriteRenderer, float value, float duration)
        {
            return DOFloatMaterial(spriteRenderer, "illumination", value, duration);
        }

        public static TweenerCore<float, float, FloatOptions> DOSolid(this SpriteRenderer spriteRenderer, AnimationBehaviourType animationBehaviour, float duration)
        {
            float endValue = AnimationBehaviourHelper.GetValue(animationBehaviour);

            var operation = DOFloatMaterial(spriteRenderer, "solid", endValue, duration);

            if (animationBehaviour == AnimationBehaviourType.Out)
            {
                ReturnMaterialToSpriteRenderer(spriteRenderer, duration).Forget();
            }

            return operation;
        }

        public static TweenerCore<float, float, FloatOptions> DOSolid(this SpriteRenderer spriteRenderer, float value, float duration)
        {
            return DOFloatMaterial(spriteRenderer, "solid", value, duration);
        }

        public static TweenerCore<float, float, FloatOptions> DOCelia(this SpriteRenderer spriteRenderer, AnimationBehaviourType animationBehaviour, float duration)
        {
            float endValue = AnimationBehaviourHelper.GetValue(animationBehaviour);

            endValue = Mathf.Clamp(endValue, 0.0f, MAX_VALUE_CELIA_SHADER);

            var operation = DOFloatMaterial(spriteRenderer, "celia", endValue, duration);

            if (animationBehaviour == AnimationBehaviourType.Out)
            {
                ReturnMaterialToSpriteRenderer(spriteRenderer, duration).Forget();
            }

            return operation;
        }

        public static TweenerCore<float, float, FloatOptions> DOCelia(this SpriteRenderer spriteRenderer, float value, float duration)
        {
            value = Mathf.Clamp(value, 0.0f, MAX_VALUE_CELIA_SHADER);

            return DOFloatMaterial(spriteRenderer, "celia", value, duration);
        }

        public static TweenerCore<float, float, FloatOptions> DOBlackAndWhite(this SpriteRenderer spriteRenderer, float value, float duration)
        {
            return DOFloatMaterial(spriteRenderer, "blackAndWhite", value, duration);
        }

        private static async UniTask ReturnMaterialToSpriteRenderer (SpriteRenderer spriteRenderer, float timeOut)
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(timeOut);

            await UniTask.Delay(timeSpan);

            spriteRenderer.ReturnDefaultMaterial();

            NovelGameDebug.Log($"Return material {spriteRenderer.material.name} to Sprite Renderer {spriteRenderer.gameObject.name}");
        }
    }
}
