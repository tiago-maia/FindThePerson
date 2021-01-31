using System;

public class PersonAssets : IEquatable<PersonAssets>
{
	public Assets.MeshAndSpritePair HeadAccessory;
	public Assets.MaterialAndSpritePair MaskMaterial;
	public Assets.MaterialAndSpritePair ShirtMaterial;
	public Assets.MaterialAndSpritePair PantsMaterial;

	public bool Equals(PersonAssets other)
	{
		return this.HeadAccessory == other.HeadAccessory
			&& this.MaskMaterial == other.MaskMaterial
			&& this.ShirtMaterial == other.ShirtMaterial
			// && this.PantsMaterial == other.PantsMaterial
			;
	}
}
