﻿using System;
using UnityEngine;

public class Assets : ScriptableObject
{
	[Serializable]
	public class MaterialAndSpritePair
	{
		public Material Material;
		public Sprite UIImage;
	}

	[Serializable]
	public class MeshAndSpritePair
	{
		public GameObject Mesh;
		public Sprite UIImage;
	}

	public MeshAndSpritePair[] HeadAccessories;
	public MaterialAndSpritePair[] MaskMaterials;
	public MaterialAndSpritePair[] ShirtMaterials;
	public MaterialAndSpritePair[] PantsMaterials;
}
