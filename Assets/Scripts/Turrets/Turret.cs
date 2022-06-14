using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Turret : MonoBehaviour
{
    [Header("Construction Config")]
    [SerializeField] private float constructAnimationTime;
    public int constructionCost;
    public int deconstructionRefund;

    public HexBase HexBase { get; private set; }

    public bool IsConstructing { get; private set; } = false;
    public bool IsDeconstructing { get; private set; } = false;

    private double constructAnimationStartTime;
    private Vector3 startScale;
    private Vector3 endScale;

    public void Init(HexBase hexBase)
    {
        HexBase = hexBase;

        Construct();

        MainInit();
    }

    private void Update()
    {
        if (IsConstructing || IsDeconstructing)
        {
            AnimateConstruction();
        }


        if (!IsConstructing && !IsDeconstructing)
        {
            MainUpdate();
        }
    }

    #region Construction
    private void AnimateConstruction()
    {
        float animationStep = (float)((Time.timeAsDouble - constructAnimationStartTime) / constructAnimationTime);
        transform.localScale = Vector3.Lerp(startScale, endScale, animationStep);
        if (animationStep >= 1)
        {
            transform.localScale = endScale;
            if (IsConstructing)
            {
                IsConstructing = false;
            }
            if (IsDeconstructing)
            {
                HexBase.FinishDeconstruct();
                Destroy(gameObject);
            }
        }
    }
    private void Construct()
    {
        IsConstructing = true;
        constructAnimationStartTime = Time.timeAsDouble;
        startScale = Vector3.zero;
        endScale = transform.localScale;
        transform.localScale = startScale;
    }
    public void Deconstruct()
    {
        if (IsConstructing)
        {
            IsConstructing = false;
        }

        IsDeconstructing = true;
        constructAnimationStartTime = Time.timeAsDouble;
        startScale = transform.localScale;
        endScale = Vector3.zero;
    }
    #endregion Construction


    public abstract void MainInit();
    public abstract void MainUpdate();
}
