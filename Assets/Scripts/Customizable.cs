using System;
using System.Collections.Generic;
using UnityEngine;

public class Customizable : MonoBehaviour
{
    public List<Customization> Customizations;
    public  int _currentCustomizationIndex;
    public Customization CurrentCustomization { get; private set; }

    void Awake()
    {
        foreach (var customization in Customizations)
        {
            customization.UpdateRenderers();
            customization.UpdateSubObjects();
        }
    }
    
    void Update()
    {
        SelectCustomizationWithUpDownArrows();

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            CurrentCustomization.NextMaterial();
            CurrentCustomization.NextSubObject();
        }
    }

    //Selection input
    void SelectCustomizationWithUpDownArrows()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow))
            _currentCustomizationIndex++;
        if (Input.GetKeyDown(KeyCode.UpArrow))
            _currentCustomizationIndex--;
        if (_currentCustomizationIndex < 0)
            _currentCustomizationIndex = Customizations.Count - 1;
        if (_currentCustomizationIndex >= Customizations.Count)
            _currentCustomizationIndex = 0;
        CurrentCustomization = Customizations[_currentCustomizationIndex];
    }
}

[Serializable]
public class Customization
{
    public string DisplayName;

    public List<Renderer> Renderers;
    public List<Material> Materials;
    public List<GameObject> SubObjects;
    public int _materialIndex;
    public int _subObjectIndex;

    public void NextMaterial()
    {
        _materialIndex++;
        if (_materialIndex >= Materials.Count)
            _materialIndex = 0;

        UpdateRenderers();
    }

    public void PreviousMaterial()
    {
        _materialIndex--;
        if (_materialIndex < 0)
            _materialIndex = Materials.Count - 1;
        UpdateRenderers();
    }

    public void NextSubObject()
    {
        _subObjectIndex++;
        if (_subObjectIndex >= SubObjects.Count)
            _subObjectIndex = 0;

        UpdateSubObjects();
    }

    public void PreviousSubObject()
    {
        _subObjectIndex--;
        if (_subObjectIndex < 0)
            _subObjectIndex = SubObjects.Count-1;
        UpdateSubObjects();

    }

    public void UpdateSubObjects()
    {
        for (var i = 0; i < SubObjects.Count; i++)
            if (SubObjects[i])
                SubObjects[i].SetActive(i == _subObjectIndex);
    }

    public void UpdateRenderers()
    {
        foreach (var renderer in Renderers)
            if (renderer)
                renderer.material = Materials[_materialIndex];
    }

   
}
