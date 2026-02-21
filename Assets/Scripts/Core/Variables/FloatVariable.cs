// Developed by Sora
//
// Copyright(c) Sora 2023-2024
//
// This script is covered by a Non-Disclosure Agreement (NDA) and is Confidential.
// Destroy the file immediately if you have not been explicitly granted access.

using UnityEngine;

namespace Sora.Variables
{
    /// <summary>
    /// A scriptable object base to create an Int value that exists in the project
    /// </summary>
    [CreateAssetMenu(fileName = "SO_NewFloatVariable", menuName = "Sora/Variables/Float Variable")]
    public class FloatVariable : ScriptableObject
    {
        public float value;
        [TextArea(2, 5)]
        [Tooltip("A short description of this variable if not obvious.")]
        [SerializeField] private string description;

        [Space]
        [Space]
        // If resetToDefault is set to true, value = defaultValue when the project stops running
        [SerializeField] private bool resetValue;
        [SerializeField] private float defaultValue;

        private void OnEnable()
        {
            if (resetValue)
                value = defaultValue;
        }
    }
}