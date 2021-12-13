using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRMaterialSwapper : MonoBehaviour
{
    public MeshRenderer[] meshRenderers = null;

    public Material hoverMaterial = null;
    public Material selectMaterial = null;
    public Material activateMaterial = null;

    bool isHovered = false;
    bool isSelected = false;
    bool isActive = false;
    Material[] originalMaterialsCache = null;

    XRBaseInteractable interactable;

    void CacheMaterials()
    {
        if (meshRenderers == null || meshRenderers.Length == 0) return;

        if (originalMaterialsCache == null || originalMaterialsCache.Length != meshRenderers.Length)
        {
            originalMaterialsCache = new Material[meshRenderers.Length];
            int i = 0;
            foreach (MeshRenderer renderer in meshRenderers)
            {
                originalMaterialsCache[i++] = renderer.material;
            }
        }
    }

    void SetMaterials(Material mat)
    {
        CacheMaterials();

        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material = mat;
        }
    }

    void ResetMaterials()
    {
        int i = 0;
        foreach (MeshRenderer renderer in meshRenderers)
        {
            renderer.material = originalMaterialsCache[i++];
        }
    }

    void OnEnable()
    {
        if (meshRenderers == null || meshRenderers.Length == 0)
        {
            meshRenderers = GetComponentsInChildren<MeshRenderer>();
        }
        CacheMaterials();
        if(interactable == null)
        {
            interactable = GetComponentInParent<XRBaseInteractable>();
        }
        if(interactable != null)
        {
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
            interactable.selectEntered.AddListener(OnSelectEntered);
            interactable.selectExited.AddListener(OnSelectExited);
            interactable.activated.AddListener(OnActivated);
            interactable.deactivated.AddListener(OnDeactivated);
        }
    }

    void OnDisable()
    {
        if (interactable != null)
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
            interactable.selectEntered.RemoveListener(OnSelectEntered);
            interactable.selectExited.RemoveListener(OnSelectExited);
            interactable.activated.RemoveListener(OnActivated);
            interactable.deactivated.RemoveListener(OnDeactivated);
        }
    }

    public void OnHoverEntered(HoverEnterEventArgs args)
    {
        if (meshRenderers != null && meshRenderers.Length > 0 && hoverMaterial)
        {
            isHovered = true;
            if (!isSelected && !isActive)
            {
                SetMaterials(hoverMaterial);
            }
        }
    }

    public void OnHoverExited(HoverExitEventArgs args)
    {
        if (meshRenderers != null && meshRenderers.Length > 0 && hoverMaterial)
        {
            isHovered = false;
            if (!isSelected && !isActive)
            {
                ResetMaterials();
            }
        }
    }

    public void OnSelectEntered(SelectEnterEventArgs args)
    {
        if (meshRenderers != null && meshRenderers.Length > 0 && selectMaterial)
        {
            isSelected = true;
            SetMaterials(selectMaterial);
        }
    }

    public void OnSelectExited(SelectExitEventArgs args)
    {
        if (meshRenderers != null && meshRenderers.Length > 0 && selectMaterial)
        {
            isSelected = false;
            if(isHovered)
            {
                SetMaterials(hoverMaterial);
            }
            else
            {
                ResetMaterials();
            }
        }
    }
   
    public void OnActivated(ActivateEventArgs args)
    {
        if (meshRenderers != null && meshRenderers.Length > 0 && activateMaterial)
        {
            SetMaterials(activateMaterial);
        }
    }

    public void OnDeactivated(DeactivateEventArgs args)
    {
        if (meshRenderers != null && meshRenderers.Length > 0 && activateMaterial)
        {
            if(isSelected)
            {
                SetMaterials(selectMaterial);
            }
            else if (isHovered)
            {
                SetMaterials(hoverMaterial);
            }
            else
            {
                ResetMaterials();
            }
        }
    }

}