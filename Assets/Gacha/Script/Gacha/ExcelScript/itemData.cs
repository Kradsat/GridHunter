using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExcelAsset]
public class itemData : ScriptableObject
{
	public List<nomalItemEntity> nomalItemData;
	public List<rareItemEntity> rareItemData;
	public List<SRareItemEntity> SRareItemData;
	public List<SSRareItemEntity> SSRareItemData;
	public List<probabilityEntity> probability;
	//public List<EntityType> nomalItemData; // Replace 'EntityType' to an actual type that is serializable.
	//public List<EntityType> rareItemData; // Replace 'EntityType' to an actual type that is serializable.
	//public List<EntityType> SRareItemData; // Replace 'EntityType' to an actual type that is serializable.
	//public List<EntityType> SSRareItemData; // Replace 'EntityType' to an actual type that is serializable.
	//public List<EntityType> probability; // Replace 'EntityType' to an actual type that is serializable.
}
