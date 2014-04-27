// ************************************************************************** //
// SerializedPropertyExtension.cs
// Copyright 2013 - Artific Games LLC
// 
// Created by: Brian Marshburn
// Created on: 10/15/2013
// ************************************************************************** //
using UnityEditor;
using UnityEngine;

/// <summary>
/// Provides extension methods for the SerializedProperty class.
/// </summary>
public static class SerializedPropertyExtension
{

    /// <summary>
    /// Gets an array of the specified type from the SerializedProperty.
    /// </summary>
    /// <typeparam name="T">The type of the members of the SerializedProperty array.</typeparam>
    /// <param name="serializedProperty">The SerializedProperty.</param>
    /// <returns>An array of the values in the SerializedProperty array.</returns>
    public static T[] GetArray<T>(this SerializedProperty serializedProperty) where T : UnityEngine.Object
    {
        if (serializedProperty == null || !serializedProperty.isArray || serializedProperty.arraySize == 0)
            return new T[0];

        T[] array = new T[serializedProperty.arraySize];
        int i = 0;
        foreach (SerializedProperty item in serializedProperty)
            array[i++] = item.objectReferenceValue as T;

        return array;
    }

}
